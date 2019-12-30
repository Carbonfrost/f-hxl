//
// - HxlLangElement.cs -
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
using System.Linq;
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public abstract class HxlLangElement : ElementFragment {

        internal HxlLangElement(string tag) : base(tag) {}

        public override void Render() {
            throw FutureFeatures.InterpretedLanguageElements();
        }

        internal sealed override void AcceptVisitor(IHxlVisitor visitor) {
            IHxlLanguageVisitor v = visitor as IHxlLanguageVisitor;
            if (v == null)
                base.AcceptVisitor(visitor);
            else
                AcceptVisitor(v);
        }

        internal abstract void AcceptVisitor(IHxlLanguageVisitor visitor);

        internal abstract HxlRenderWorkElement ToIsland(IScriptGenerator gen);

        internal virtual void RewriteIslandChildren(HxlRenderWorkElement result) {}

        internal ExpressionValue EnsureExpValue(ref ExpressionValue test, string name, string dv = null) {
            if (test == null) {
                test = new ExpressionValue() { Value = this.Attribute(name) ?? dv };
                this.Attribute(name, test);
            }
            return test;
        }

        internal HxlRenderWorkElement BindConditional(Expression test, bool negate) {
            var testExp = RewriteExpressionSyntax.BindVariables(test).ToString();

            // TODO Should bind as a Boolean here
            if (negate) {
                testExp = string.Format("!({0})", testExp);
            }
            string[] pre =
            {
                string.Format("if ({0}) {{", testExp)
            };

            string[] post = { "}" };
            return new HxlRenderWorkElement(pre, post);
        }

    }

}
