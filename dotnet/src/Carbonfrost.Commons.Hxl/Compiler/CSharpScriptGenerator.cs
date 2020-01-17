//
// - CSharpScriptGenerator.cs -
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class CSharpScriptGenerator : IScriptGenerator {

        internal static readonly CSharpScriptGenerator Instance
            = new CSharpScriptGenerator();

        static readonly string[] END = { "}" };

        private static string EmitVarLoopStatusCurrent(HxlIterationElementBase e) {
            if (!string.IsNullOrEmpty(e.VarLoopStatus)) {
                return string.Format("{0}.Current = {1};", e.VarLoopStatus, e.Var);
            }

            return null;
        }

        private static string EmitVarLoopStatusIncrement(HxlIterationElementBase e) {
            if (!string.IsNullOrEmpty(e.VarLoopStatus)) {
                return string.Format("{0}.Index++;", e.VarLoopStatus);
            }

            return null;
        }

        private static string EmitVarLoopStatus(HxlIterationElementBase e) {
            if (!string.IsNullOrEmpty(e.VarLoopStatus)) {
                return string.Format("var {0} = new global::Carbonfrost.Commons.Hxl.LoopStatus();", e.VarLoopStatus);
            }

            return null;
        }

        // IScriptGenerator implementation
        public IEnumerable<string> Start(HxlForEachElement e) {
            yield return EmitVarLoopStatus(e);

            var inExp = RewriteExpressionSyntax.BindVariables(e.In);
            yield return string.Format("foreach (var {0} in {1}) {{", e.Var, inExp);
            yield return string.Format("__closure.{0} = {0};", e.Var);
            yield return EmitVarLoopStatusCurrent(e);
        }

        public IEnumerable<string> End(HxlForEachElement e) {
            yield return EmitVarLoopStatusIncrement(e);
            yield return "}";
        }

        public IEnumerable<string> Start(HxlForElement e) {
            yield return EmitVarLoopStatus(e);

            // TODO Expression.ToString() not currently correct, and implicit conversion is probably required on text
            yield return string.Format("for (var {0} = {1}; {0} <= {2}; {0} += {3}) {{",
                                       e.Var,
                                       e.From.ToString().Trim('\''),
                                       e.To.ToString().Trim('\''),
                                       e.Step.ToString().Trim('\''));
            yield return EmitVarLoopStatusCurrent(e);
        }

        public IEnumerable<string> End(HxlForElement e) {
            yield return EmitVarLoopStatusIncrement(e);
            yield return "}";
        }

        public IEnumerable<string> Start(HxlValueElement e) {
            if (!string.IsNullOrWhiteSpace(e.Format)) {
                // TODO Support format on value element
                throw new NotImplementedException();
            }

            yield return string.Format("__self.Write({0});", e.Expression);
        }

        public IEnumerable<string> End(HxlValueElement e) {
            return Empty<string>.List;
        }

        public IEnumerable<string> Start(HxlTextElement e) {
            yield return string.Format(
                "__self.Write(\"{0}\");", CodeUtility.Escape(e.Data));
        }

        public IEnumerable<string> End(HxlTextElement e) {
            return Empty<string>.List;
        }

        public IEnumerable<string> Start(HxlModelElement e) {
            return new[] {
                "// model set",
            };
        }

        public IEnumerable<string> End(HxlModelElement e) {
            return Empty<string>.List;
        }

        public IEnumerable<string> Start(HxlCommentElement e) {
            return new[] {
                "__self.Write(\"<!--\");",
            };
        }

        public IEnumerable<string> End(HxlCommentElement e) {
            return new[] {
                "__self.Write(\"-->\");"
            };
        }

        public IEnumerable<string> Start(HxlCaseElement e) {
            return Empty<string>.List;
        }

        public IEnumerable<string> End(HxlCaseElement e) {
            return Empty<string>.List;
        }

        public IEnumerable<string> Start(HxlWhenElement e) {
            return BindIf(e.Test, false);
        }

        public IEnumerable<string> End(HxlWhenElement e) {
            return new [] { "}" };
        }

        public IEnumerable<string> Start(HxlOtherwiseElement e) {
            return new [] { "{" };
        }

        public IEnumerable<string> End(HxlOtherwiseElement e) {
            return new [] { "}" };
        }

        public IEnumerable<string> Start(HxlRootElement e) {
            return Empty<string>.List;
        }

        public IEnumerable<string> End(HxlRootElement e) {
            return Empty<string>.List;
        }

        private string[] BindIf(Expression test, bool negate) {
            var testExp = RewriteExpressionSyntax.BindVariables(test).ToString();

            // TODO Should bind as a Boolean here

            if (negate) {
                testExp = string.Format("!({0})", testExp);
            }
            return new []
            {
                string.Format("if ({0}) {{", testExp)
            };
        }
    }
}
