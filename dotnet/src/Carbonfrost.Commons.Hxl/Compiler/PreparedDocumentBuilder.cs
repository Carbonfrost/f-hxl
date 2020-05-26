//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;

using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Html;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class PreparedDocumentBuilder {

        private readonly IList<string> usings = new List<string>();
        // private readonly HtmlDocumentFragment _sourceDocument;
        private readonly IDomNodeFactory _nodeFactory;
        private HxlServices _services;

        // private HxlDocumentFragment _preparedDocument;
        // private readonly HxlCompilerSettings _settings;
        // private readonly IHxlCompilerReferencePath _referencePath;

        private HxlServices Services {
            get {
                return _services;
            }
        }

        public PreparedDocumentBuilder(IHxlTemplateBuilder builder, IDomNodeFactory nodeFactory, IHxlCompilerReferencePath referencePath) {
            var container = new ServiceContainer(ServiceProvider.Current);
            container.AddService(typeof(IDomNodeFactory), nodeFactory);
            container.AddService(typeof(IHxlCompilerReferencePath), referencePath);
            container.AddService(typeof(IHxlTemplateBuilder), builder);

            _nodeFactory = nodeFactory;
            _services = new HxlServices(container);
        }

        internal HxlDocumentFragment Prepare(HtmlDocumentFragment source, IProfilerScope metrics) {
            metrics.StartPreprocessor();
            // ParsedTemplate parsed = this;

            var all = CreateCompilerProcessors();
            var converter = new DomConverter(Services);
            var myDoc = converter.Convert(source);

            foreach (var c in all) {
                c.Preprocess(myDoc, Services);
            }

            // TODO Combine adjacent text uses (performance)
            // TODO ToArray() is wasteful (performance)

            // Convert document
            HxlDocumentFragment result = NewDocument();
            foreach (var m in myDoc.ChildNodes.ToArray()) {
                var conv = HxlCompilerConverter.ChooseConverter(m);
                conv.ConvertAndAppend(result, m, CSharpScriptGenerator.Instance);
            }

            metrics.EndPreprocessor();

            // TODO This is a compiler settings property
            bool skipOptimizations = false;
            metrics.TemplateOptimizerStarting("cs", !skipOptimizations);

            if (!skipOptimizations) {
                metrics.StartOptimizer();
                OptimizeRenderIslands.Rewrite(result);
                metrics.EndOptimizer();
            }

            return result;
        }

        private IEnumerable<IHxlCompilerProcessor> CreateCompilerProcessors() {
            return new IHxlCompilerProcessor[] {
                new ApplyPreprocessorNodes(Services),
                RewriteLanguageAttributes.Instance,
                RewriteExpressionSyntax.Instance,
                MarkRetainedNodes.Instance,
                RemoveNonSignificantWhitespace.Instance,
            };
        }

        private HxlDocumentFragment NewDocument() {
            var factory = new HxlProviderFactory(_nodeFactory);
            return (HxlDocumentFragment) new HxlDocument(factory).CreateDocumentFragment();
        }
    }
}
