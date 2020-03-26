//
// Copyright 2013, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

namespace Carbonfrost.Commons.Hxl.Compiler {

    public sealed class HxlTextElement : HxlLangElement {

        // Implements an element whose text is treated as a text node regardless
        // of whether it contains possible markup.
        // <c:text>Hello, <em>$Person</em></c:text>
        //   <==> Hello, <em>$$Person</em>

        // TODO This property shouldn't be necessary (we really want inner text without
        // inner text automatically escaping - say < into &lt;)
        public string Data { get; set; }
        public string Format { get; set; }

        // TODO Currently public to support serialization (but it should use the
        // document or not be used at all)
        public HxlTextElement() : base("c:text") {}

        internal override void AcceptVisitor(IHxlLanguageVisitor visitor) {
            visitor.Visit(this);
        }

        internal override HxlRenderWorkElement ToIsland(IScriptGenerator gen) {
            return new HxlRenderWorkElement(gen.Start(this), gen.End(this));
        }
    }
}
