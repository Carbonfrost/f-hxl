//
// - ProviderDomNodeFactory.cs -
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
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    // Considers all attribute/element fragments using providers
    class ProviderDomNodeFactory : HxlDomNodeFactory {

        private readonly LookupBuffer attributes;
        private readonly LookupBuffer elements;

        public ProviderDomNodeFactory()
        {
            attributes = new LookupBuffer(typeof(AttributeFragment));
            elements = new LookupBuffer(typeof(ElementFragment));
        }

        // TODO Instances should be created using the .ctor(string) overload
        // if it is available so that we can make sure that the node is
        // initialized with the name it matched.

        public override DomAttribute CreateAttribute(HxlQualifiedName name) {
            Type type = attributes.GetValueOrDefault(name.QualifiedName);

            if (type != null) {
                using (HxlCompilerContext.Set(name.Prefix)) {
                    return (DomAttribute) Activator.CreateInstance(type);
                }
            } else
                return null;
        }

        public override DomElement CreateElement(HxlQualifiedName name) {
            Type type =  elements.GetValueOrDefault(name.QualifiedName);

            if (type != null) {
                using (HxlCompilerContext.Set(name.Prefix)) {
                    return (DomElement) Activator.CreateInstance(type);
                }
            } else
                return null;
        }

        // TODO Parse PIs defined outside compiler asm

        //        private void RequireProviders() {
        //            if (provideInit)
        //                return;
//
        //            provideInit = true;
//
        //            // TODO Don't enumerate a dictionary, which forces all assemblies to load (performance)
        //            foreach (var t in App.DescribeProviders()
        //                     .GetProviderInfos(typeof(AttributeFragment))) {
        //                this.attributes.TryAdd(t.Name, t.Type, Traceables.ProviderDomNodeFactoryDuplicate);
        //            }
//
        //            foreach (var t in App.DescribeProviders()
        //                     .GetProviderInfos(typeof(ElementFragment))) {
        //                this.elements.TryAdd(t.Name, t.Type, Traceables.ProviderDomNodeFactoryDuplicate);
        //            }
        //        }

        private class LookupBuffer {

            private readonly Dictionary<QualifiedName, IProviderInfo> _elements
                = new Dictionary<QualifiedName, IProviderInfo>(Comparers.IgnoreCaseQualifiedName);

            private readonly IEnumerator<IProviderInfo> _items;

            public LookupBuffer(
                Type providerType)
            {
                var items = App.DescribeProviders().GetProviderInfos(providerType);
                _items = items.GetEnumerator();
            }

            public Type GetValueOrDefault(QualifiedName qn) {
                IProviderInfo result;
                if (_elements.TryGetValue(qn, out result))
                    return result.Type;

                while (_items.MoveNext()) {
                    var t = _items.Current;
                    _elements.TryAdd(t.Name, t, Traceables.ProviderDomNodeFactoryDuplicate);

                    if (Comparers.IgnoreCaseQualifiedName.Equals(qn, t.Name))
                        return t.Type;
                }

                return null;
            }
        }

    }

}
