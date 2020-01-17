//
// - HxlNamespaceResolver.Default.cs -
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
using System.Linq;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Hxl {

    partial class HxlNamespaceResolver {

        class DefaultImpl : IHxlNamespaceResolver {

            public Uri LookupNamespace(string prefix) {
                switch (prefix) {
                    case "c":
                        return Xmlns.HxlLangUri;

                    case "xml":
                        return Xmlns.XmlUri;

                    case "xmlns":
                        return Xmlns.XmlnsUri;

                    case "hxl":
                        return Xmlns.HxlUri;

                    default:
                        return null;
                }
            }

            public string LookupPrefix(Uri namespaceUri) {
                if (namespaceUri == Xmlns.HxlLangUri)
                    return "c";

                if (namespaceUri == Xmlns.HxlUri)
                    return "hxl";

                if (namespaceUri == Xmlns.XmlUri)
                    return "xml";

                if (namespaceUri == Xmlns.XmlnsUri)
                    return "xmlns";

                return null;
            }
        }

        class NullImpl : IHxlNamespaceResolver {

            public Uri LookupNamespace(string prefix) {
                return null;
            }

            public string LookupPrefix(Uri namespaceUri) {
                return null;
            }
        }

    }

}
