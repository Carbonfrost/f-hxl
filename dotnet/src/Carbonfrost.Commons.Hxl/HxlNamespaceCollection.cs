//
// Copyright 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Linq;
using Carbonfrost.Commons.PropertyTrees;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Hxl {

    public class HxlNamespaceCollection : Collection<HxlNamespace>, IHxlNamespaceResolver {

        private readonly Dictionary<string, HxlNamespace> map = new Dictionary<string, HxlNamespace>();

        public HxlNamespaceCollection() {}

        public HxlNamespaceCollection(IEnumerable<HxlNamespace> other) {
            if (other != null)
                Items.AddMany(other);
        }

        private void VerifyPrefix(string prefix, string argumentName) {
            if (prefix == null)
                throw new ArgumentNullException(argumentName);
            if (string.IsNullOrEmpty(prefix))
                throw Failure.EmptyString(argumentName);

            if (this.map.ContainsKey(prefix))
                throw HxlFailure.PrefixAlreadyDefined("prefix", prefix);

            switch (prefix) {
                case "hxl":
                case "c":
                case "xml":
                case "xmlns":
                    throw HxlFailure.CannotUseBuiltinPrefixes(argumentName);
            }
        }

        [Add(Name = "namespace")]
        public HxlNamespace AddNew(string prefix,
                                   Uri uri) {
            VerifyPrefix(prefix, "prefix");
            if (uri == null)
                throw new ArgumentNullException("uri");

            var result = new HxlNamespace(prefix, uri);

            this.Add(result);
            this.map.Add(prefix, result);
            return result;
        }

        public Uri LookupNamespace(string prefix) {
            if (prefix == null)
                throw new ArgumentNullException("prefix");
            if (prefix.Length == 0)
                throw Failure.EmptyString("prefix");

            var result = map.GetValueOrDefault(prefix);
            if (result == null)
                return null;

            return result.NamespaceUri;
        }

        public string LookupPrefix(Uri namespaceUri) {
            if (namespaceUri == null)
                throw new ArgumentNullException("namespaceUri");

            throw new NotImplementedException();
        }

        public HxlNamespaceCollection Clone() {
            return new HxlNamespaceCollection(this);
        }

        // `Collection' overrides
        protected override void InsertItem(int index, HxlNamespace item) {
            ThrowIfReadOnly();
            VerifyPrefix(item.Prefix, "item");
            base.InsertItem(index, item);
        }

        protected override void ClearItems() {
            ThrowIfReadOnly();
            base.ClearItems();
        }

        protected override void SetItem(int index, HxlNamespace item) {
            ThrowIfReadOnly();
            VerifyPrefix(item.Prefix, "item");
            base.SetItem(index, item);
        }

        protected override void RemoveItem(int index) {
            ThrowIfReadOnly();
            base.RemoveItem(index);
        }

        protected void ThrowIfReadOnly() {
            if (this.IsReadOnly)
                throw Failure.ReadOnlyCollection();
        }

        // `IMakeReadOnly' implementation
        public void MakeReadOnly() {
            this.IsReadOnly = true;
        }

        public bool IsReadOnly {
            get; private set;
        }
    }
}
