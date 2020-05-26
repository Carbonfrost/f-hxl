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
        private readonly HtmlDocumentFragment _sourceDocument;
        private readonly IDomNodeFactory _nodeFactory;

        private readonly IProfilerScope _metrics;
        private HxlDocumentFragment _preparedDocument;
        private readonly HxlCompilerSettings _settings;
        private readonly IHxlCompilerReferencePath _referencePath = new HxlCompilerReferencePath();

        public HxlCompilerSettings Settings {
            get {
                return _settings;
            }
        }

        internal IEnumerable<Assembly> ImplicitAssemblyReferences {
            get {
                return _referencePath.ImplicitAssemblyReferences;
            }
        }

        public string Signature {
            get;
            private set;
        }

        public HtmlDocumentFragment SourceDocument {
            get {
                return _sourceDocument;
            }
        }

        public HxlDocumentFragment PreparedDocument {
            get {
                return _preparedDocument;
            }
        }

        public ParsedTemplate(string text, string name, HxlCompilerSettings settings) {
            _metrics = Metrics.ForTemplateParsing();
            _metrics.StartParsing();
            _sourceDocument = HtmlDocumentFragment.Parse(text, new HtmlReaderSettings {
                Mode = HtmlTreeBuilderMode.Xml
            });
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
                session.ImplicitAssemblyReferences.AddMany(ImplicitAssemblyReferences);

                CSharpCodeEmitter emitter = new CSharpCodeEmitter(output, this);
                emitter.WriteCode(_metrics);
                output.Flush();
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

        private HxlDocumentFragment PrepareDocument() {
            var pp = new PreparedDocumentBuilder(this, _nodeFactory, _referencePath);
            return pp.Prepare(SourceDocument, _metrics);
        }
    }

}
