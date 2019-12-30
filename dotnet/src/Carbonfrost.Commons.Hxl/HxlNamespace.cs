//
// - HxlNamespace.cs -
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
using System.Reflection;

using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Hxl {

    public class HxlNamespace : IEquatable<HxlNamespace> {

        private readonly string prefix;
        private readonly Uri _namespaceUri;

        public string Prefix {
            get { return prefix; }
        }

        public Uri NamespaceUri {
            get { return _namespaceUri; }
        }

        public HxlNamespace(string prefix,
                            Uri namespaceUri) {

            if (prefix == null)
                throw new ArgumentNullException("prefix");
            if (prefix.Length == 0)
                throw Failure.EmptyString("prefix");

            if (namespaceUri == null)
                throw new ArgumentNullException("namespaceUri");

            this.prefix = prefix;
            this._namespaceUri = namespaceUri;
        }

        public override bool Equals(object obj) {
            HxlNamespace other = obj as HxlNamespace;
            return Equals((HxlNamespace) other);
        }

        public override int GetHashCode() {
            int hashCode = 0;
            unchecked {
                hashCode += 1000000007 * prefix.GetHashCode();
                hashCode += 1000000009 * _namespaceUri.GetHashCode();
            }
            return hashCode;
        }

        public static bool operator ==(HxlNamespace lhs, HxlNamespace rhs) {
            if (ReferenceEquals(lhs, rhs))
                return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(HxlNamespace lhs, HxlNamespace rhs) {
            return !(lhs == rhs);
        }

        public bool Equals(HxlNamespace other) {
            if (other == null)
                return false;
            return this.prefix == other.prefix
                && object.Equals(this._namespaceUri, other._namespaceUri);
        }

        public override string ToString() {
            return string.Format("{0} => {1}", prefix, NamespaceUri);
        }

        internal static string GetDefaultPrefix(NamespaceUri ns,
                                                Assembly assembly) {
            return AssemblyInfo.GetAssemblyInfo(assembly)
                .GetXmlNamespacePrefix(ns);
        }
    }
}
