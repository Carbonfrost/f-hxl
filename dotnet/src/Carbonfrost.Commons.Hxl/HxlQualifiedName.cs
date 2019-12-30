//
// - HxlQualifiedName.cs -
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Carbonfrost.Commons.Core;


namespace Carbonfrost.Commons.Hxl {

    // [ValueSerializer(typeof(HxlQualifiedNameConverter))]
    public class HxlQualifiedName : IHxlQualifiedName, IEquatable<HxlQualifiedName> {

        // TODO Support equality operators

        private readonly string prefix;
        private readonly ReadOnlyCollection<string> names;
        private readonly string variable;
        private readonly Uri namespaceUri;

        public string FullName {
            get {
                return HxlQualifiedNameHelper.GetFullName(this);
            }
        }

        public string Prefix {
            get {
                return prefix;
            }
        }

        public IReadOnlyList<string> Names {
            get {
                return names;
            }
        }

        // prefix:Name1
        public string Name {
            get {
                if (names.Count == 1)
                    return LocalName;
                else
                    return Prefix + ":" + LocalName;
            }
        }

        public string LocalName {
            get {
                return names[0];
            }
        }

        public string Property {
            get {
                return this.GetProperty();
            }
        }

        public string Variable {
            get {
                return variable;
            }
        }

        public Uri NamespaceUri {
            get {
                return namespaceUri;
            }
        }

        public QualifiedName QualifiedName {
            get {
                return QualifiedName.Create(NamespaceUri, LocalName);
            }
        }

        public HxlQualifiedName(string prefix,
                                IEnumerable<string> names, string variable, Uri namespaceUri)
        {
            this.namespaceUri = namespaceUri;
            this.variable = variable;
            this.names = new ReadOnlyCollection<string>(names.ToArray());
            this.prefix = prefix;
        }

        public static HxlQualifiedName Parse(string text, IHxlNamespaceResolver namespaceResolver) {
            HxlQualifiedName result;
            Exception ex = _TryParse(text, namespaceResolver, out result);
            if (ex == null)
                return result;
            else
                throw ex;
        }

        // TODO CORP RULES requires that this be IServiceProvider

        public static bool TryParse(string text, IHxlNamespaceResolver namespaceResolver, out HxlQualifiedName result) {
            return _TryParse(text, namespaceResolver, out result) == null;
        }

        static Exception _TryParse(string text, IHxlNamespaceResolver namespaceResolver, out HxlQualifiedName result) {
            result = null;

            if (text == null)
                return new ArgumentNullException("text");
            if (string.IsNullOrEmpty(text))
                return Failure.EmptyString("text");

            var helper = HxlQualifiedNameHelper.Parse(text);

            Uri ns = null;
            if (!string.IsNullOrEmpty(helper.Prefix))
                ns = namespaceResolver.LookupNamespace(helper.Prefix);

            result = helper.ToName(ns);
            return null;
        }

        public override bool Equals(object obj) {
            HxlQualifiedName other = obj as HxlQualifiedName;
            return Equals(other);
        }

        public bool Equals(HxlQualifiedName other) {
            if (other == null)
                return false;

            return this.prefix == other.prefix
                && this.names.SequenceEqual(other.names)
                && this.variable == other.variable
                && object.Equals(this.namespaceUri, other.namespaceUri);
        }

        public override int GetHashCode() {
            int hashCode = 0;
            unchecked {
                if (prefix != null)
                    hashCode += 1000000007 * prefix.GetHashCode();

                foreach (var m in Names)
                    hashCode += 37 * m.GetHashCode();

                if (variable != null)
                    hashCode += 1000000021 * variable.GetHashCode();

                if (namespaceUri != null)
                    hashCode += 1000000033 * namespaceUri.GetHashCode();
            }

            return hashCode;
        }

        public override string ToString() {
            return FullName;
        }

        public static bool operator ==(HxlQualifiedName lhs, HxlQualifiedName rhs) {
            if (ReferenceEquals(lhs, rhs))
                return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(HxlQualifiedName lhs, HxlQualifiedName rhs) {
            return !(lhs == rhs);
        }
    }
}
