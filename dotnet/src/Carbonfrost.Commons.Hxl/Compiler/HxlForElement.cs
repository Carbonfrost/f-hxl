//
// - HxlForElement.cs -
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

    public class HxlForElement : HxlIterationElementBase, IHxlIterationElement {

        private ExpressionValue _to, _step, _from;

        public Expression From {
            get { return EnsureFromExp().Expression; }
            set { EnsureFromExp().Expression = value; }
        }

        public Expression To {
            get { return EnsureToExp().Expression; }
            set { EnsureToExp().Expression = value; }
        }

        public Expression Step {
            get { return EnsureStepExp().Expression; }
            set { EnsureStepExp().Expression = value; }
        }

        internal HxlForElement() : base("c:for") {
        }

        internal override void AcceptVisitor(IHxlLanguageVisitor visitor) {
            visitor.Visit(this);
        }

        internal override HxlRenderWorkElement ToIsland(IScriptGenerator gen) {
            return new HxlRenderWorkElement(gen.Start(this), gen.End(this));
        }

        private ExpressionValue EnsureToExp() {
            return EnsureExpValue(ref _to, "to");
        }

        private ExpressionValue EnsureFromExp() {
            return EnsureExpValue(ref _from, "from", "1");
        }

        private ExpressionValue EnsureStepExp() {
            return EnsureExpValue(ref _step, "step", "1");
        }

    }
}
