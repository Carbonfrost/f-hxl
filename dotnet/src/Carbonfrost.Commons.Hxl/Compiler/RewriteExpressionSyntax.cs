//
// - RewriteExpressionSyntax.cs -
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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    sealed class RewriteExpressionSyntax : HxlCompilerProcessor {

        internal static readonly RewriteExpressionSyntax Instance
            = new RewriteExpressionSyntax();

        private static readonly HashSet<string> KEYWORDS = new HashSet<string> {
            "this",
        };

        // TODO Accept any character; need lookbehind on $$
        // TODO Use bracket counting ... the expression $(a+b+(c+d)) is valid, for instance

        private static readonly Regex EXPR_FORMAT = new Regex(@"\$ (
(\{ (?<Expression> [^\}]+) \}) | (?<Expression> [:a-z0-9_\.]+) )", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        internal static MatchCollection MatchVariables(string text) {
            MatchCollection matches = EXPR_FORMAT.Matches(text);
            return matches;
        }

        public override void Preprocess(DomDocument document, IServiceProvider serviceProvider) {
            Rewrite(document);
        }

        internal static void MatchVariablesAndEmit(IHxlTemplateEmitter builder,
                                                   string text) {
            var c = MatchVariables(text);
            ProcessVariablesAndEmit(c, builder, text);
        }

        private static void ProcessVariablesAndEmit(MatchCollection matches,
                                                    IHxlTemplateEmitter builder,
                                                    string text) {
            int previousIndex = 0;
            foreach (Match match in matches) {
                builder.EmitLiteral(text.Substring(previousIndex, match.Index - previousIndex));

                string expText = match.Groups["Expression"].Value;
                var e = ParseAndBindExpr(expText);
                builder.EmitValue(e);
                previousIndex = match.Index + match.Length;
            }
            builder.EmitLiteral(text.Substring(previousIndex, text.Length - previousIndex));
        }

        internal static Expression BindVariables(Expression e) {
            ExpressionBinder eb = new ExpressionBinder();
            return eb.Visit(e);
        }

        public static void Rewrite(DomDocument document) {
            ProcessElement(document);
        }

        static Expression ParseAndBindExpr(string expText) {
            if (expText.IndexOf("::", StringComparison.Ordinal) > 0)
                throw FutureFeatures.ExpressionNSSyntax();

            bool skipBind = false;
            if (expText.StartsWith("::", StringComparison.Ordinal)) {
                expText = expText.Substring(2);
                skipBind = true;
            }

            Expression e = Expression.Parse(expText);
            if (!skipBind)
                e = BindVariables(e);
            return e;
        }

        static IEnumerable<DomNode> ExtractNodes(DomDocument doc, string text) {
            int previousIndex = 0;
            string outText;
            var matches = MatchVariables(text);
            foreach (Match match in matches) {
                outText = text.Substring(previousIndex, match.Index - previousIndex);
                if (outText.Length > 0)
                    yield return doc.CreateText(outText);

                var expText = match.Groups["Expression"].Value;
                var e = ParseAndBindExpr(expText);

                yield return new HxlValueElement { Expression =  e };
                previousIndex = match.Index + match.Length;
            }

            outText = text.Substring(previousIndex, text.Length - previousIndex);

            if (outText.Length > 0)
                yield return doc.CreateText(outText);
        }

        static void ProcessElement(DomNode e) {
            if (e.NodeType == DomNodeType.Text && !Utility.IsData((DomText) e)) {
                var domText = (DomText) e;
                e.ReplaceWith(ExtractNodes(domText.OwnerDocument, domText.Data));
                return;
            }

            if (e.ChildNodes.Count == 0)
                return;

            // TODO Array allocation to allow modification to children is wasteful (performance)
            foreach (var c in e.ChildNodes.ToArray())
                ProcessElement(c);
        }

        private class ExpressionBinder : ExpressionRewriter {

            protected override Expression VisitMemberAccessExpression(MemberAccessExpression expression) {
                var nm = Utility.PascalCase(expression.Name);
                return Expression.MemberAccess(this.Visit(expression.Expression),
                                               nm);
            }

            protected override Expression VisitNameExpression(NameExpression expression) {
                if (KEYWORDS.Contains(expression.Name))
                    return expression;

                // TODO Real resolve ignore list
                var nm = Utility.PascalCase(expression.Name);
                return Expression.MemberAccess(Expression.Name("__closure"), nm);
            }

        }

    }
}
