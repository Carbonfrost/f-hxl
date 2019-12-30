//
// - DirectiveFactory.cs -
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

using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public abstract class DirectiveFactory {

        public static readonly DirectiveFactory Default;
        public static readonly DirectiveFactory Null;

        static readonly Dictionary<Assembly, DirectiveFactory> assemblies;

        static DirectiveFactory() {
            // TODO Default should be a composite on BuildManager
            assemblies = new Dictionary<Assembly, DirectiveFactory>();
            Null = new NullDirectiveFactory();
            Default = FromAssembly(typeof(DirectiveFactory).GetTypeInfo().Assembly);
        }

        public static DirectiveFactory Compose(IEnumerable<DirectiveFactory> items) {
            if (items == null)
                throw new ArgumentNullException("items");

            var realItems = items.Distinct().Except(Null).ToArray();

            if (realItems.Length == 0)
                return DirectiveFactory.Null;

            else if (realItems.Length == 1)
                return realItems[0];

            else
                return new CompositeDirectiveFactory(realItems);
        }

        public static DirectiveFactory Compose(params DirectiveFactory[] items) {
            if (items == null)
                throw new ArgumentNullException("items");

            return Compose((IEnumerable<DirectiveFactory>) items);
        }

        public static DirectiveFactory FromAssembly(Assembly assembly) {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            if (assemblies == null)
                return FromAssemblyInternal(assembly);
            else
                return assemblies.GetValueOrCache(assembly, FromAssemblyInternal);
        }

        public HxlDirective Create(string directive) {
            if (directive == null)
                throw new ArgumentNullException("directive");
            if (directive.Length == 0)
                throw Failure.EmptyString("directive");

            var items = HxlDirective.Parse(directive).ToArray();
            if (items == null)
                return HxlDirective.Null;

            Type type = GetDirectiveType(items[0].Key);
            if  (type == null)
                return null;

            throw new NotImplementedException();
            // return CreateDirectiveCore(type, items.Skip(1));
        }

        public abstract Type GetDirectiveType(string directiveName);

        protected virtual HxlDirective CreateDirectiveCore(Type directiveType,
                                                           IEnumerable<KeyValuePair<string, string>> attributes) {
            if (directiveType == null)
                throw new ArgumentNullException("directiveType");

            var s = attributes.Select(t => new KeyValuePair<string, object>(t.Key, t.Value));
            return (HxlDirective) Activation.CreateInstance(directiveType, s);
        }

        static DirectiveFactory FromAssemblyInternal(Assembly assembly) {
            ReflectedDirectiveFactory result = new ReflectedDirectiveFactory(assembly);
            if (result.ExportedDefs == 0)
                return Null;
            else
                return result;
        }
    }
}
