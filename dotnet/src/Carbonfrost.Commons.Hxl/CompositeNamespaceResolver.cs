//
// Copyright 2013, 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Hxl {

    sealed class CompositeNamespaceResolver : IHxlNamespaceResolver {

        private readonly IHxlNamespaceResolver[] items;

        public CompositeNamespaceResolver(IHxlNamespaceResolver[] items) {
            this.items = items;
        }

        public Uri LookupNamespace(string prefix) {
            return items.FirstNonNull(t => t.LookupNamespace(prefix));
        }

        public string LookupPrefix(Uri namespaceUri) {
            if (namespaceUri == null)
                throw new ArgumentNullException("namespaceUri");

            return items.FirstNonNull(t => t.LookupPrefix(namespaceUri));
        }
    }
}
