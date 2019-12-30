//
// - CodeUtility.cs -
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

using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Hxl.Compiler;

namespace Carbonfrost.Commons.Hxl {

    static class CodeUtility {

        static readonly Regex replaceChars = new Regex(@"[^a-z0-9_]+", RegexOptions.IgnoreCase); // \-/:?#\[\]@!$&'()*+,;=\s
        const int maxLength = 256;

        public static string Slug(string text) {
            if (string.IsNullOrEmpty(text))
                return text;

            string slug = replaceChars.Replace(text, "_").Trim('_');
            if (slug.Length > maxLength) {
                slug = slug.Substring(0, maxLength);
            }

            if (slug.Length == 0)
                slug = "template";

            // Add leader
            if (char.IsDigit(slug[0])) {
                slug = "_" + slug;
            }
            return slug.ToLower();
        }

        public static string AppendDomText(string text) {
            return string.Format("__self.Write(\"{0}\");",
                                 CodeUtility.Escape(text));
        }

        public static bool IsValidIdentifier(string value) {
            // TODO Check for keywords (uncommon)
            return Regex.IsMatch(value, "@?[a-z_][a-z0-9_]*", RegexOptions.IgnoreCase);
        }

        // Escape a quoted string
        public static StringBuilder Escape(string text) {
            StringBuilder fragmentCache = new StringBuilder(text);
            fragmentCache.Replace(@"\", @"\\");
            fragmentCache.Replace("\"", "\\\"");
            fragmentCache.Replace("\r", @"\r");
            fragmentCache.Replace("\n", @"\n");
            return fragmentCache.Replace("\t", @"\t");
        }

        public static string EmitInstantiation(ExpressionSerializationManager manager,
                                               TextWriter output,
                                               object component) {

            var exp = manager.Serialize(component);
            var cache = new StringWriter();

            var emit = new CSharpExpressionEmitter(cache);
            emit.Visit(exp);

            foreach (var m in emit.decls)
                output.WriteLine(m);

            output.WriteLine(cache);

            // First element of block should be name
            var block = (BlockExpression) exp;
            var name = (NameExpression) ((BinaryExpression) block.Expressions[0]).Left;
            return name.Name;
        }

    }
}
