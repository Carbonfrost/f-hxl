//
// - HxlNamespaceResolver.cs -
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
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl {

    [Composable, Providers]
    public abstract partial class HxlNamespaceResolver : IHxlNamespaceResolver {

        public static readonly IHxlNamespaceResolver Default = new DefaultImpl();
        public static readonly IHxlNamespaceResolver Null = new NullImpl();

        public static IHxlNamespaceResolver Compose(params IHxlNamespaceResolver[] items) {
            if (items == null || items.Length == 0)
                return Null;
            if (items.Length == 1)
                return items[0];
            else
                return new CompositeNamespaceResolver(items);
        }

        [HxlNamespaceResolverUsage]
        public static IHxlNamespaceResolver Compose(IEnumerable<IHxlNamespaceResolver> items) {
            if (items == null)
                return Null;

            return Compose(items.ToArray());
        }

        public virtual Uri LookupNamespace(string prefix) {
            return null;
        }

        public virtual string LookupPrefix(Uri namespaceUri) {
            if (namespaceUri == null)
                throw new ArgumentNullException("namespaceUri");

            return null;
        }
    }

}
