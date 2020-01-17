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
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Hxl {

    public class HxlCompiler {

        private readonly HxlCompilerSettings settings;

        public HxlCompilerSettings Settings {
            get { return settings; }
        }

        public static bool CommandLineHost {
            get {
                return Environment.GetEnvironmentVariable("HXLC_HOST") == "1";
            }
            internal set {
                Environment.SetEnvironmentVariable("HXLC_HOST", value ? "1" : "0");
            }
        }

        public HxlCompiler() : this(null) {}

        private HxlCompiler(HxlCompilerSettings settings) {
            settings = settings ?? HxlCompilerSettings.Default;
            if (settings.IsReadOnly)
                this.settings = settings;
            else {
                this.settings = settings.Clone();
                this.settings.MakeReadOnly();
            }
        }

        public static HxlCompiler Create(HxlCompilerSettings settings = null) {
            if (settings == null) {
                settings = HxlCompilerSettings.Default;
            }

            return new HxlCompiler(settings);
        }

        public HxlTemplate LoadTemplate(string fileName) {
            return LoadTemplate(StreamContext.FromFile(fileName));
        }

        public HxlTemplate LoadTemplate(Uri source) {
            return LoadTemplate(StreamContext.FromSource(source));
        }

        public HxlTemplate LoadTemplate(Stream stream) {
            return LoadTemplate(StreamContext.FromStream(stream));
        }

        public HxlTemplate LoadTemplate(StreamContext input) {
            if (input == null)
                throw new ArgumentNullException("input");

            string name = null;

            // Take last segment of the name by default
            if (input.Uri != null) {
                Match m = Regex.Match(input.Uri.ToString(), "[^/]+$");
                if (m.Success) {
                    name = m.Value;
                }
            }

            return new ParsedTemplate(input.ReadAllText(), name, Settings);
        }

        public HxlTemplate ParseTemplate(string text) {
            return new ParsedTemplate(text ?? string.Empty, null, Settings);
        }

        public void GenerateSource(TextWriter outputWriter,
                                   IEnumerable<HxlTemplate> templates) {
            if (outputWriter == null)
                throw new ArgumentNullException("outputWriter");

            RequireTemplates(templates);

            var session = new HxlCompilerSession(Settings);
            foreach (var template in templates)
                GenerateOneSourceFile(session, template, outputWriter);

            // TODO Support template factory emit
            if (this.Settings.EmitTemplateFactory) {
            }
        }

        public void GenerateSource(TextWriter outputWriter,
                                   params HxlTemplate[] templates) {
            if (templates == null)
                throw new ArgumentNullException("templates");

            GenerateSource(outputWriter, (IEnumerable<HxlTemplate>) templates);
        }

        public string GenerateSource(params HxlTemplate[] templates) {
            StringWriter sw = new StringWriter();
            GenerateSource(sw, templates);

            return sw.ToString();
        }

        public HxlCompilerResults Compile(params HxlTemplate[] templates) {
            return Compile((IEnumerable<HxlTemplate>) templates);
        }

        public HxlCompilerResults Compile(IEnumerable<HxlTemplate> templates) {
            RequireTemplates(templates);

#if NET
            var internalCompiler = new CodeDomInternalCompiler();
            return internalCompiler.Compile(Settings, templates);
#else
            var internalCompiler = new RoslynInternalCompiler();
            return internalCompiler.Compile(Settings, templates);
#endif
        }

        internal static void GenerateOneSourceFile(HxlCompilerSession session,
                                                   HxlTemplate template,
                                                   TextWriter outputWriter) {
            // TODO Soft check here instead of casting
            IHxlEmittableTemplate t = (IHxlEmittableTemplate) template;
            t.GenerateSource(session, outputWriter);
        }

        private void RequireTemplates(IEnumerable<HxlTemplate> templates) {
            if (templates == null)
                throw new ArgumentNullException("templates");
            if (!templates.Any())
                throw Failure.EmptyCollection("templates");
            if (templates.Any(t => t == null))
                throw Failure.CollectionContainsNullElement("templates");
        }
    }
}
