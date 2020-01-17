//
// - HxlValueElement.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public sealed class HxlValueElement : HxlLangElement {

        // Implements an element whose inner text is expression markup.
        // This element represents the DOM representation of expression
        // syntax.
        // ${text} ==> <c:value>text</c:value>

        // TODO c:attribute, c:class
        // TODO c:data -- for working with data- attributes
        // TODO c:script, c:body

        [Value]
        public Expression Expression { get; set; }

        public string Format { get; set; }

        public HxlValueElement() : base("c:value") {
        }

        internal override void AcceptVisitor(IHxlLanguageVisitor visitor) {
            visitor.Visit(this);
        }

        internal override HxlRenderWorkElement ToIsland(IScriptGenerator gen) {
            return new HxlRenderWorkElement(gen.Start(this), gen.End(this));
        }
    }
}
