//
// - HxlTemplateFactory.cs -
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

namespace Carbonfrost.Commons.Hxl {

    public abstract class HxlTemplateFactory : IHxlTemplateFactory {

        readonly IList<IHxlTemplateFactory> _factories = new List<IHxlTemplateFactory>();

        static readonly IDictionary<Assembly, IHxlTemplateFactory> map
            = new Dictionary<Assembly, IHxlTemplateFactory>();

        public static readonly IHxlTemplateFactory Null = new NullImpl();

        public static readonly IHxlTemplateFactory Default
            = Compose(App.DescribeAssemblies().Select(FromAssembly));

        public static IHxlTemplateFactory LateBound(TypeReference type) {
            if (type == null)
                throw new ArgumentNullException("type");

            return new LateBoundHxlTemplateFactory(type);
        }

        public HxlTemplate CreateTemplate(string templateName, string templateType, IServiceProvider serviceProvider) {
            if (templateName == null)
                throw new ArgumentNullException("templateName");
            if (string.IsNullOrEmpty(templateName))
                throw Failure.EmptyString("templateName");

            Type type = GetTemplateType(templateName, templateType, serviceProvider);
            Traceables.HxlTemplateFactoryCreateTemplate(templateName, templateType, type);
            if (type == null)
                return null;
            else
                return (HxlTemplate) Activator.CreateInstance(type);
        }

        public abstract Type GetTemplateType(string templateName, string templateType, IServiceProvider serviceProvider);

        public static IHxlTemplateFactory Compose(params IHxlTemplateFactory[] factories) {
            if (factories == null || factories.Length == 0)
                return Null;

            if (factories.Length == 1)
                return factories[0];
            else
                return Compose((IEnumerable<IHxlTemplateFactory>) factories);
        }

        public static IHxlTemplateFactory Compose(IEnumerable<IHxlTemplateFactory> factories) {
            if (factories == null)
                return Null;

            var myFactories = factories.Where(t => t != null && !(t is NullImpl)).ToArray();

            // TODO Implementation as an array will cause Default to sync appdomain loading (performance)

            if (myFactories.Length == 0)
                return Null;
            else if (myFactories.Length == 1)
                return myFactories[0];
            else
                return new CompositeTemplateFactory(myFactories);
        }

        public static IHxlTemplateFactory FromAssembly(Assembly assembly) {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            return map.GetValueOrCache(assembly, FromAssemblyInternal);
        }

        static IHxlTemplateFactory FromAssemblyInternal(Assembly assembly) {
            var d = assembly.GetCustomAttributes(typeof(HxlTemplateFactoryAttribute))
                .Cast<HxlTemplateFactoryAttribute>();

            if (d.Any()) {
                Traceables.HxTemplateFactoryFromAssemblyExplicit(assembly, d);
                var all = d.Select(t => (IHxlTemplateFactory) Activator.CreateInstance(t.FactoryType));
                return Compose(all);
            }

            // Default slow implementation
            // Don't scan App_Web assemblies for templates
            // HACK Using the name of the generated assembly (might be better to check for [GeneratedCode] if this works with Mono)
            if (Utility.IsHxlAssembly(assembly) && !assembly.GetName().Name.StartsWith("App_Web_", StringComparison.Ordinal)) {
                var result = new ReflectedTemplateFactory(assembly);
                Traceables.HxTemplateFactoryFromAssemblySlow(assembly, result);
                return result;

            } else {
                Traceables.HxTemplateFactoryFromAssemblySkip(assembly);
                return HxlTemplateFactory.Null;
            }
        }

        sealed class NullImpl : IHxlTemplateFactory {

            public HxlTemplate CreateTemplate(string templateName, string templateType, IServiceProvider serviceProvider) {
                return null;
            }
        }
    }

}
