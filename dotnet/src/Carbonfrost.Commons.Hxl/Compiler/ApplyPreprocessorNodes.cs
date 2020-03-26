//
// - ApplyPreprocessorNodes.cs -
//
// Copyright 2014 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    sealed class ApplyPreprocessorNodes : IHxlCompilerProcessor {

        // TODO Technically, this applies much earlier than IHxlCompilerProcessors normally do (design)
        // suggesting it is not actually a compiler processor

        private readonly IHxlTemplateBuilder _builder;

        public ApplyPreprocessorNodes(IHxlTemplateBuilder builder) {
            this._builder = builder;
        }

        public void Preprocess(DomContainer document, IServiceProvider serviceProvider) {
            // Directives only appear at document level
            foreach (HxlProcessingInstruction node in document.ChildNodes.OfType<HxlProcessingInstruction>()) {
                node.Preprocess_(_builder);
            }
        }

        public void Optimize(HxlDocument template, IServiceProvider serviceProvider) {}

    }
}
