//
// - Xmlns.cs -
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

    static class Xmlns {

        public const string HxlLang = "http://ns.carbonfrost.com/commons/hxl/lang";
        public const string Hxl = "http://ns.carbonfrost.com/commons/hxl";

        internal static readonly Uri HxlLangUri = new Uri(Xmlns.HxlLang);
        internal static readonly Uri HxlUri = new Uri(Xmlns.Hxl);

        public static readonly Uri XmlUri = new Uri(NamespaceUri.Xml.NamespaceName);
        public static readonly Uri XmlnsUri = new Uri(NamespaceUri.Xmlns.NamespaceName);
    }
}
