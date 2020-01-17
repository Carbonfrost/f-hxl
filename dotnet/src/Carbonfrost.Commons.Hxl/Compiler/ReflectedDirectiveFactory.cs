//
// - ReflectedDirectiveFactory.cs -
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
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class ReflectedDirectiveFactory : DirectiveFactory {

        static readonly Regex DIRECTIVE = new Regex("(^Xsp)|(Directive$)", RegexOptions.IgnoreCase);

        private readonly IDictionary<string, Type> types = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        internal int ExportedDefs {
            get { return types.Count; }
        }

        public ReflectedDirectiveFactory(Assembly assembly) {
            // The built-in directives don't have to be public
            Func<TypeInfo, bool> predicate = (t) => (t.IsPublic || t.IsNestedPublic);
            if (assembly == typeof(ReflectedDirectiveFactory).GetTypeInfo().Assembly) {
                predicate = (_) => true;
            }

            foreach (Type type in assembly.GetTypes()) {
                if (predicate(type.GetTypeInfo()) && typeof(HxlDirective).GetTypeInfo().IsAssignableFrom(type)) {
                    DirectiveUsageAttribute da = (DirectiveUsageAttribute) type.GetTypeInfo().GetCustomAttribute(typeof(DirectiveUsageAttribute));
                    string name = GetDirectiveName(type, da);
                    types.Add(name, type);
                }
            }
        }

        static string GetDirectiveName(Type type, DirectiveUsageAttribute da) {
            if (da == null || string.IsNullOrEmpty(da.Name)) {
                return DIRECTIVE.Replace(type.Name, string.Empty);

            } else {
                return da.Name;
            }
        }

        public override Type GetDirectiveType(string directiveName) {
            return this.types.GetValueOrDefault(directiveName);
        }

    }

}
