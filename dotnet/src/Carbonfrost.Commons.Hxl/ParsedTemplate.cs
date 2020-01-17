//
// Copyright 2013, 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Html;
using System.Security.Cryptography;
using System.Text;

namespace Carbonfrost.Commons.Hxl {

    class ParsedTemplate : HxlTemplate, IHxlTemplateBuilder, IHxlEmittableTemplate {

        private readonly IList<string> usings = new List<string>();
        private readonly HtmlDocument _sourceDocument;
        private readonly IDomNodeFactory _nodeFactory;

        private readonly IProfilerScope _metrics;
        private readonly HashSet<Assembly> _implicitAssemblyReferences = new HashSet<Assembly>();
        private HxlDocument _preparedDocument;
        private readonly HxlCompilerSettings _settings;

        public HxlCompilerSettings Settings {
            get {
                return _settings;
            }
        }

        public ICollection<Assembly> ImplicitAssemblyReferences {
            get {
                return _implicitAssemblyReferences;
            }
        }

        public string Signature {
            get;
            private set;
        }

        public HtmlDocument SourceDocument {
            get {
                return _sourceDocument;
            }
        }

        public HxlDocument PreparedDocument {
            get {
                return _preparedDocument;
            }
        }

        public ParsedTemplate(string text, string name, HxlCompilerSettings settings) {
            _metrics = Metrics.ForTemplateParsing();
            _metrics.StartParsing();
            _sourceDocument = HtmlDocument.ParseXml(text, null);
            _metrics.EndParsing(name, text.Length);

            _settings = settings;
            _nodeFactory = DomNodeFactory.Compose(
                HxlDomNodeFactory.Compiler,
                _settings.NodeFactories,
                new InvalidFactory());

            Signature = string.Concat(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text))
                .Take(8)
                .Select(b => b.ToString("x2"))
            );

            if (string.IsNullOrEmpty(name)) {
                this.TemplateName = "Template" + this.Signature;
            } else {
                this.TemplateName = CodeUtility.Slug(name);
            }

            this.Namespace = "Generated";
            this.ClassName = this.TemplateName;

            using (UsingNamespaceResolver()) {
                this._preparedDocument = PrepareDocument();
            }
        }

        public override void Transform(TextWriter outputWriter, HxlTemplateContext context) {
            throw FutureFeatures.CompileAndEmit();
        }

        private IDisposable UsingNamespaceResolver() {
            var rt = HxlNamespaceResolver
                .Compose(HxlNamespaceResolver.Default, _settings.Namespaces, _settings.Assemblies);
            SetResolver(rt);

            return new DisposableAction(() => SetResolver(null));
        }

        void SetResolver(IHxlNamespaceResolver resolver) {
            _settings.NodeFactories.SetResolver(resolver);

            var factories = _nodeFactory as IEnumerable<IDomNodeFactory>;
            if (factories != null) {
                foreach (var factory in factories.OfType<IHxlDomNodeFactory>()) {
                    factory.SetResolver(resolver);
                }
            }
        }

        void IHxlEmittableTemplate.GenerateSource(HxlCompilerSession session, TextWriter output) {
            // Implicitly handle c: and hxl:
            _metrics.StartSourceGenerator();
            var settings = session.Settings;

            using (UsingNamespaceResolver()) {
                // TODO Init emitter.SkipOptimizations from compiler settings

                CSharpCodeEmitter emitter = new CSharpCodeEmitter(output, this);
                emitter.WriteCode(_metrics);
                output.Flush();
                session.ImplicitAssemblyReferences.AddMany(ImplicitAssemblyReferences);
            }

            _metrics.EndSourceGenerator();
            _metrics.Dispose();
        }

        public string Namespace { get; set; }
        public string ClassName { get; set; }
        public string TemplateName { get; set; }
        public TypeReference ModelType { get; set; }
        public TypeReference BaseClass { get; set; }

        public IList<string> Imports {
            get {
                return usings;
            }
        }

        public IList<AssemblyName> AssemblyReferences {
            get {
                return null;
            }
        }

        private HxlDocument PrepareDocument() {
            _metrics.StartPreprocessor();
            ParsedTemplate parsed = this;

            // TODO Consider which services belong here
            var sp = new ServiceContainer(ServiceProvider.Current);

            var all = CreateCompilerProcessors();
            var converter = new DomConverter();
            var myDoc = converter.Convert(parsed.SourceDocument,
                                          NewDocument(),
                                          t => _implicitAssemblyReferences.Add(t.GetTypeInfo().Assembly));

            foreach (var c in all)
                c.Preprocess(myDoc, sp);

            // TODO Combine adjacent text uses (performance)
            // TODO ToArray() is wasteful (performance)

            // Convert document
            HxlDocument result = NewDocument();
            foreach (var m in myDoc.ChildNodes.ToArray()) {
                var conv = HxlCompilerConverter.ChooseConverter(m);
                conv.ConvertAndAppend(result, m, CSharpScriptGenerator.Instance);
            }

            _metrics.EndPreprocessor();

            // TODO This is a compiler settings property
            bool skipOptimizations = false;
            _metrics.TemplateOptimizerStarting("cs", !skipOptimizations);

            if (!skipOptimizations) {
                _metrics.StartOptimizer();
                OptimizeRenderIslands.Rewrite(result);
                _metrics.EndOptimizer();
            }

            return result;
        }

        private IEnumerable<IHxlCompilerProcessor> CreateCompilerProcessors() {
            return new IHxlCompilerProcessor[] {
                new ApplyPreprocessorNodes(this),
                RewriteLanguageAttributes.Instance,
                RewriteExpressionSyntax.Instance,
                MarkRetainedNodes.Instance,
                RemoveNonSignificantWhitespace.Instance,
            };
        }

        private HxlDocument NewDocument() {
            var factory = new HxlProviderFactory(_nodeFactory);
            return new HxlDocument(factory);
        }
    }

}
