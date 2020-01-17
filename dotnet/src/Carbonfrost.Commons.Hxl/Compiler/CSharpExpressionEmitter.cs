//
// - CSharpExpressionEmitter.cs -
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
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class CSharpExpressionEmitter : ExpressionPrinter {

        public CSharpExpressionEmitter(TextWriter output)
            : base(output) {
        }

        // TODO Would be cleaner to extract decl finder into its own visitor
        public readonly List<string> decls = new List<string>();

        protected override void VisitBlockExpression(BlockExpression expression) {
            foreach (var expr in expression.Expressions) {
                Visit(expr);
                Output.WriteLine(";");
            }
        }

        protected override void VisitBinaryExpression(BinaryExpression expression) {
            if (expression.Left is NameExpression && expression.Right is NewObjectExpression) {
                var name = ((NameExpression) expression.Left).Name;
                var type = ((NewObjectExpression) expression.Right).Expression;
                decls.Add(string.Format("{0} {1};", type, name));
            }

            base.VisitBinaryExpression(expression);
        }

        protected override void VisitNewArrayExpression(NewArrayExpression expression) {
            var tb = expression.Annotation<TypeBinding>();
            Output.Write("new ");
            Output.Write(tb.Type.FullName);
            Output.Write(" { ");
            foreach (var expr in expression.Expressions) {
                Visit(expr);
                Output.Write(", ");
            }
            Output.Write("}");
        }

        protected override void VisitConstantExpression(ConstantExpression expression) {
            var str = expression.Value as string;
            if (str != null) {
                Output.Write("\"{0}\"", CodeUtility.Escape(str));
                return;
            }

            base.VisitConstantExpression(expression);
        }

        private void WriteCastIfNecessary(Expression expression, Action action) {
            var tb = expression.Annotation<TypeBinding>();

            if (tb != null) {
                Output.Write("((global::{0}) ", tb.Type.FullName);
                action();
                Output.Write(")");

            } else {
                action();
            }
        }

    }
}
