//
// - HxlAttributeConverter.cs -
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
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    abstract class HxlAttributeConverter : HxlCompilerConverter {

        static readonly Regex EXPR = new Regex(@"\$[^$]");

        static new readonly HxlAttributeConverter Inline = new InlineAttributeConverter();
        static readonly HxlAttributeConverter Expression = new ExpressionAttributeConverter();

        internal static bool IsExpr(string value) {
            return EXPR.IsMatch(value);
        }

        public static HxlCompilerConverter GetAttributeConverter(DomAttribute attribute) {
            if (attribute is HxlAttribute)
                return Noop;

            if (IsExpr(attribute.Value ?? string.Empty))
                return HxlAttributeConverter.Expression;
            else
                return Inline;
        }

        public sealed override DomObject Convert(DomObject node, IScriptGenerator gen) {
            return Convert(node.OwnerDocument, (DomAttribute) node);
        }

        protected abstract DomObject Convert(DomDocument document, DomAttribute attribute);

        private class ExpressionAttributeConverter : HxlAttributeConverter {

            protected override DomObject Convert(DomDocument document, DomAttribute attribute) {
                var buffer = new CodeBuffer();
                RewriteExpressionSyntax.MatchVariablesAndEmit(buffer, attribute.Value);
                return new HxlExpressionAttribute(attribute.Name, buffer.ToString());
            }

        }

        private class InlineAttributeConverter : HxlAttributeConverter {

            // - Typical DOM attribute without any expressions
            protected override DomObject Convert(DomDocument document, DomAttribute attribute) {
                return attribute;
            }
        }
    }
}
