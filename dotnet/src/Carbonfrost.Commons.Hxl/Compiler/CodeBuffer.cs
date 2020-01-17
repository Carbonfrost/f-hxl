//
// - CodeBuffer.cs -
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
using System.Text;
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Hxl.Compiler {

    internal class CodeBuffer : IHxlTemplateEmitter {

        private readonly StringBuilder sb = new StringBuilder();

        public override string ToString() {
            if (sb.Length == 0)
                return "string.Empty";
            else
                return string.Format("string.Concat((object) {0})", sb);
        }

        void IHxlTemplateEmitter.EmitCode(string code) {
            // Unused by expression expander
        }

        void IHxlTemplateEmitter.EmitLiteral(string text) {
            if (string.IsNullOrEmpty(text))
                return;

            sb.AppendSeparator(", ");
            sb.Append("\"" + CodeUtility.Escape(text) + "\"");
        }

        void IHxlTemplateEmitter.EmitValue(Expression expr) {
            sb.AppendSeparator(", ");
            sb.Append("(");
            sb.Append(expr);
            sb.Append(")");
        }

    }


}
