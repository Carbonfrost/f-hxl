//
// - HxlCaseElement.cs -
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

namespace Carbonfrost.Commons.Hxl.Compiler {

    public class HxlCaseElement : HxlLangElement {

        internal HxlCaseElement() : base("c:case") {}

        internal override HxlRenderWorkElement ToIsland(IScriptGenerator gen) {
            return new HxlRenderWorkElement(gen.Start(this), gen.End(this));
        }

        internal override void RewriteIslandChildren(HxlRenderWorkElement result) {
            int index = 0;
            int count = result.Elements.Count;

            foreach (var m in result.Elements) {
                if (index++ < count - 1) {
                    var child = (HxlRenderWorkElement) m;

                    child.PostLines[child.PostLines.Length - 1]
                        += " else ";
                }
            }
            base.RewriteIslandChildren(result);
        }

        internal override void AcceptVisitor(IHxlLanguageVisitor visitor) {
            visitor.Visit(this);
        }
    }

}
