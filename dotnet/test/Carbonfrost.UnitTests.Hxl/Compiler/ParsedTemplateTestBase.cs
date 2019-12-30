//
// Copyright 2013, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Carbonfrost.Commons.Html;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Spec;
using System.Text.RegularExpressions;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public abstract class ParsedTemplateTestBase : TestClass {

        private readonly StringBuilder errors = new StringBuilder();
        private IReadOnlyList<string> _expectedSource;

        public Assembly Assembly {
            get;
            private set;
        }

        public string ActualHtml {
            get;
            private set;
        }

        public string ExpectedHtml {
            get;
            private set;
        }

        public IProperties Data {
            get;
            private set;
        }

        public string Source {
            get;
            private set;
        }

        public string FixtureName {
            get;
            private set;
        }

        public string GeneratedSource {
            get;
            private set;
        }

        public IReadOnlyList<string> ExpectedSource {
            get {
                return _expectedSource ?? new string[0];
            }
        }

        public HxlCompilerErrorCollection CompilerErrors { get; private set; }
        public IDictionary<string, string> OtherGeneratedSources {
            get; private set;
        }

        public string InputHtml {
            get;
            private set;
        }

        protected void LoadDefault(
            [CallerMemberName] string method = "") {
            Load(method.Replace("_", "-"));
        }

        protected void Load(string fixture) {
            var fixtureFileName = string.Format("Hxl/{0}.fixture", fixture);
            var myFixture = TestContext.LoadFixture(fixtureFileName).Items[0];
            errors.Clear();

            this.FixtureName = GetType().Name + "-" + fixture;
            this.Source = myFixture["input.hxl"];

            this.InputHtml = HtmlDocument.ParseXml(this.Source, null).OuterHtml;
            HxlCompilerSettings settings = new HxlCompilerSettings();
            settings.Namespaces.AddNew("test", new Uri("http://example.com/"));

            HxlCompiler c = HxlCompiler.Create(settings);
            HxlTemplate temp = c.ParseTemplate(this.Source);

            // UNDONE Currently, f-spec is using the stream context API from shared instead of f-core,
            // so we have to copy the data over and hack the generated template names to support the
            // proper fixture data names.
            var templates = myFixture.Where(t => t.Key.EndsWith(".hxl", StringComparison.Ordinal))
                .Select(t => {
                    var tpl = (ParsedTemplate) c.ParseTemplate(myFixture.GetStreamContext(t.Key).ReadAllText());
                    string name = WorkaroundTemplateName(fixtureFileName, t.Key);
                    tpl.TemplateName = name;
                    tpl.ClassName = name;
                    return tpl;
                });
            var results = c.Compile(templates);

            this.Data = new Properties();
            if (myFixture["data.properties"] != null) {
                var props = Properties.FromStream(myFixture.GetStreamContext("data.properties").OpenRead());

                // So that we get some iterable content:
                // Use array syntax [a,b,c]
                foreach (var kvp in props) {
                    string parsedValue = Convert.ToString(kvp.Value);

                    if (parsedValue.Length > 0 && parsedValue[0] == '[') {
                        var array = parsedValue.Substring(1, parsedValue.Length - 2).Split(',');
                        this.Data.SetProperty(kvp.Key, array);

                    } else {
                        this.Data.SetProperty(kvp.Key, parsedValue);
                    }
                }
            }

            var es = myFixture["output.cs"];
            if (es == null) {
                _expectedSource = null;
            } else {
                var reader = new StringReader(es);
                var lines = new List<string>();
                string line;
                while ((line = reader.ReadLine()) != null) {
                    lines.Add(line);
                }
                _expectedSource = lines;
            }

            GeneratedSource = c.GenerateSource(temp);
            OtherGeneratedSources = templates.Except(temp).ToDictionary(t => ((IHxlTemplateBuilder) t).TemplateName, t => c.GenerateSource(t));

            var allErrors = results.Errors.Where(t => !t.IsWarning);
            if (allErrors.Any()) {
                WriteCompilerOutput(string.Empty, string.Empty);
                Assert.Fail("One or more compiler errors: {0}", allErrors.First());
            }

            // UNDONE This is needed to support the load context used by fspec
            Assembly = ((Carbonfrost.Commons.Shared.AssemblyInfo) Carbonfrost.Commons.Shared.Runtime.Components.RuntimeComponent.Load(
                "assembly", new Uri("file://" + results.PathToAssembly)
            )).Assembly;

            ExpectedHtml = myFixture["generated.html"];
            CompilerErrors = results.Errors;
        }

        private static string WorkaroundTemplateName(string fixtureFileName, string key) {
            string input = fixtureFileName + "#" + key;
            // Take last segment of the name by default
            Match m = Regex.Match(input, "[^/]+$");
            if (m.Success) {
                input = m.Value;
            }
            return CodeUtility.Slug(input);
        }

        protected void GenerateAndAssert() {
            // TODO Stage the stream context fixture instead to avoid strange names
            var type = this.Assembly.GetTypes().Single(t => t.Name.EndsWith("input_hxl", StringComparison.Ordinal));
            var template = (HxlTemplate) Activator.CreateInstance(type);
            var expected = ExpectedHtml;
            string actual = null;

            try {
                HxlTemplateContext context = HxlTemplateContext.WithData(null, this.Data);
                var factory = new HxlTemplateFactoryCollection();
                factory.AddAssembly(this.Assembly);
                context.TemplateFactory = factory;

                actual = this.ActualHtml = template.TransformText(context);

            } catch (Exception ex) {
                string tempFile2 = WriteCompilerOutput("<error>", expected);
                Assert.Fail("Failed transforming text: {0} - {1}", tempFile2, ex);
            }

            // TODO Assert on generated code
            // TODO Whitespace normalization should be an option because future tests will need to look for it
            actual = NormalizeForComparison(actual);
            expected = NormalizeForComparison(expected);
            bool result = string.Equals((actual),
                                        (expected));
            if (!result) {
                string tempFile = WriteCompilerOutput(actual, expected);
                string message = string.Format("Comparison failed: {0}", tempFile);
                // Console.WriteLine(message);
                Assert.Equal(expected, actual);
            }

            // Check lines of generated source
            foreach (var m in ExpectedSource) {
                if (!this.GeneratedSource.Contains(m)) {
                    string temp = WriteCompilerOutput(actual, expected);
                    Assert.Fail("Generated source missing line: {0} ({1})", m, temp);
                }
            }

            // Cache the output
            WriteCompilerOutput(actual, expected);
        }

        protected void AssertNoWarnings() {
            // UNDONE Lint warnings - Assert.Empty(CompilerErrors);
        }

        static string NormalizeForComparison(string text) {
            var doc = HtmlDocument.ParseXml(text, null);
            doc.AcceptVisitor(new NormalizeWSVisitor());
            return doc.OuterHtml;
        }

        class NormalizeWSVisitor : HtmlNodeVisitor {

            public override void VisitText(HtmlText node) {
                node.Data = node.Data.Trim();
            }
        }

        private string WriteCompilerOutput(string actual, string expected) {
            string source = this.GeneratedSource;
            Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "ff-hxl"));
            string tempFile = Path.Combine(Path.GetTempPath(), "ff-hxl", FixtureName + ".cs");

            using (TextWriter tw = File.CreateText(tempFile)) {
                tw.WriteLine("source:");
                tw.WriteLine(source);
                tw.WriteLine();
                foreach (var kvp in this.OtherGeneratedSources) {
                    tw.WriteLine("source ({0}):", kvp.Key);
                    tw.WriteLine(kvp.Value);
                    tw.WriteLine();
                }

                tw.WriteLine("input:");
                tw.WriteLine(this.InputHtml);
                tw.WriteLine();
                tw.WriteLine("data:");
                tw.WriteLine(Data);
                tw.WriteLine();
                tw.WriteLine("actual:");
                tw.WriteLine(actual);
                tw.WriteLine();
                tw.WriteLine("expected:");
                tw.WriteLine(expected);
                tw.WriteLine();
                tw.WriteLine("expected code:");
                tw.WriteLine(string.Join("\n", ExpectedSource));
                tw.WriteLine();
                tw.WriteLine(errors.ToString());
            }
            return tempFile;
        }

        private bool Error(string text, params object[] args) {
            this.errors.AppendLine(string.Format(text, args));
            return false;
        }

    }
}
