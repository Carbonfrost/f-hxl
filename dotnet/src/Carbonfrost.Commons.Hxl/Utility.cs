//
// - Utility.cs -
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
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Html;

namespace Carbonfrost.Commons.Hxl {

    static class Utility {

        static readonly string ASM = typeof(HxlTemplateFactory).GetTypeInfo().Assembly.GetName().Name;
        static readonly Regex ELEMENT = new Regex("(^Xsp)|(Tag$)", RegexOptions.IgnoreCase);
        static readonly Regex ATTRIBUTE = new Regex("(^Hxl)|(Attribute$)", RegexOptions.IgnoreCase);
        static readonly Regex SPLIT = new Regex("(?=[A-Z])");
        static readonly Regex SLUG = new Regex(@"[\.\-/:?#\[\]@!$&'()*+,;=\s]+");

        static IDictionary<Type, string[]> nameCache = new Dictionary<Type, string[]>();

        public static PropertyInfo ReflectGetProperty(Type type, string propertyName) {
            var pd = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (pd == null || pd.GetIndexParameters().Length > 0) {
                return null;
            }

            return pd;
        }

        public static IEnumerable<PropertyInfo> ReflectGetProperties(Type sourceClrType) {
            // TODO Cache these results (performance)
            Type baseType = sourceClrType;
            HashSet<string> names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            while (baseType != null && baseType != typeof(object)) {
                foreach (PropertyInfo prop in baseType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
                    if (prop.GetIndexParameters().Length == 0 && names.Add(prop.Name)) {
                        yield return prop;
                    }
                }
                baseType = baseType == null ? null : baseType.GetTypeInfo().BaseType;
            }
        }

        public static string GetImplicitName(Type type) {
            return string.Join("-", GetWords(type)).ToLowerInvariant();
        }

        public static string GetImplicitControllerName(Type type) {
            return SplitWords(type).First();
        }

        public static string GetImplicitActionName(Type type) {
            return string.Join(string.Empty, SplitWords(type).Skip(1));
        }

        internal static IEnumerable<string> SplitWords(Type type) {
            // TODO Switch to using Inflection class (in particular two-letter abbreviations)
            return nameCache.GetValueOrCache(type, GetWords);
        }

        static string[] GetWords(Type t) {
            string name;

            if (typeof(HxlAttribute).IsAssignableFrom(t)) {
                name = ATTRIBUTE.Replace(t.Name, string.Empty);
            } else {
                name = ELEMENT.Replace(t.Name, string.Empty);
            }
            return SPLIT.Split(name).Skip(1).ToArray();
        }

        public static IEnumerable<T> Cons<T>(T item, IEnumerable<T> others) {
            return (new [] { item }).Concat(others);
        }

        public static bool IsSelfClosing(DomElement element) {
            switch (element.Name) {
                case "link":
                case "meta":
                case "br":
                case "img":
                case "input":
                    return true;
            }

            // TODO Missing whether elements are self-closing in schemas
            return false;
        }

        public static bool IsData(DomText text) {
            DomElement e = text.ParentElement;
            return e != null && (e.Name == "script" || e.Name == "style");
        }

        public static string JoinList<T>(string sep, IEnumerable<T> items, Action<StringBuilder, T> func) {
            var sb = new StringBuilder();
            var comma = false;

            foreach (var m in items) {
                if (comma)
                    sb.Append(sep);

                func(sb, m);
                comma = true;
            }

            return sb.ToString();
        }

        public static bool IsHxlAssembly(Assembly assembly) {
            // TODO Could have faster verdict for System assemblies
            return assembly.GetReferencedAssemblies()
                .Any(t => string.Equals(t.Name, ASM, StringComparison.OrdinalIgnoreCase));
        }

        public static string RandomID() {
            byte[] array = new byte[8];
            RandomNumberGenerator.Create().GetBytes(array);
            return CodeUtility.Slug(Convert.ToBase64String(array));
        }

        public static string PascalCase(string name) {
            return char.ToUpperInvariant(name[0]) + name.Substring(1);
        }

        public static string EscapeHtml(string text) {
            // HACK Re-escape entity references: ff-html currently unescapes entities as their real chars, but that
            // is unpredictable for various encodings
            return HtmlEncoder.Escape(text, Encoding.ASCII.GetEncoder(), EscapeMode.Base);
        }

        public static T Parse<T>(string text, TryParser<T> tryParser) {
            T result;
            Exception ex = tryParser(text, out result);
            if (ex == null)
                return result;
            else
                throw ex;
        }

        public delegate Exception TryParser<T>(string text, out T result);
    }
}
