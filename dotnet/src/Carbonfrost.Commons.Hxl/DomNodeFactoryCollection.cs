//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
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

using Carbonfrost.Commons.PropertyTrees;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class DomNodeFactoryCollection : NamedObjectCollection<IDomNodeFactory>, IDomNodeFactoryApiConventions {

        public DomNodeFactoryCollection() {}

        public DomNodeFactoryCollection(IEnumerable<IDomNodeFactory> other) {
            if (other != null)
                Items.AddMany(other);
        }

        [Add]
        public void AddNew(string name, IDomNodeFactory factory) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(name))
                throw Failure.EmptyString("name");
            if (factory == null)
                throw new ArgumentNullException("factory");

            Add(new NamedDomNodeFactory(name, factory));
        }

        internal void SetResolver(IHxlNamespaceResolver resolver) {
            SetResolver_(this, resolver);
        }

        private static void SetResolver_(IEnumerable<IDomNodeFactory> items, IHxlNamespaceResolver resolver) {
            foreach (var m in items) {
                IHxlDomNodeFactory f = m as IHxlDomNodeFactory;
                if (f != null) {
                    f.SetResolver(resolver);
                    continue;
                }

                IEnumerable<IDomNodeFactory> composite = m as IEnumerable<IDomNodeFactory>;
                if (composite != null)
                    SetResolver_(composite, resolver);
            }
        }

        public DomNodeFactoryCollection Clone() {
            return new DomNodeFactoryCollection(this);
        }

        protected override string GetNameForItem(IDomNodeFactory item) {
            // IObjectWithName ion = item as IObjectWithName;
            // if (ion == null) {
            //     var qn = App.GetProviderName(typeof(IDomNodeFactory), item);
            //     return qn.LocalName;
            // }
            return base.GetNameForItem(item);
        }

        public DomAttribute CreateAttribute(string name) {
            return Items.FirstNonNull(t => t.CreateAttribute(name));
        }

        public DomAttribute CreateAttribute(string name, string value) {
            var attr = CreateAttribute(name);
            if (attr == null) {
                return null;
            }
            attr.Value = value;
            return attr;
        }

        public DomAttribute CreateAttribute(string name, IDomValue value) {
            var attr = CreateAttribute(name);
            if (attr == null) {
                return null;
            }

            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }
            return attr.SetValue(value);
        }

        public DomCDataSection CreateCDataSection() {
            return Items.FirstNonNull(t => t.CreateCDataSection());
        }

        public DomComment CreateComment() {
            return Items.FirstNonNull(t => t.CreateComment());
        }

        public DomDocumentType CreateDocumentType(string name) {
            return Items.FirstNonNull(t => t.CreateDocumentType(name));
        }

        public DomElement CreateElement(string name) {
            return Items.FirstNonNull(t => t.CreateElement(name));
        }

        public DomEntityReference CreateEntityReference(string data) {
            return Items.FirstNonNull(t => t.CreateEntityReference(data));
        }

        public DomProcessingInstruction CreateProcessingInstruction(string target) {
            return Items.FirstNonNull(t => t.CreateProcessingInstruction(target));
        }

        public DomText CreateText() {
            return Items.FirstNonNull(t => t.CreateText());
        }

        public DomDocumentFragment CreateDocumentFragment() {
            return Items.FirstNonNull(t => t.CreateDocumentFragment());
        }

        public DomEntity CreateEntity(string name) {
            return Items.FirstNonNull(t => t.CreateEntity(name));
        }

        public DomNotation CreateNotation(string name) {
            return Items.FirstNonNull(t => t.CreateNotation(name));
        }

        public Type GetAttributeNodeType(string name) {
            return Items.FirstNonNull(t => t.GetAttributeNodeType(name));
        }

        public Type GetElementNodeType(string name) {
            return Items.FirstNonNull(t => t.GetElementNodeType(name));
        }

        public Type GetProcessingInstructionNodeType(string name) {
            return Items.FirstNonNull(t => t.GetProcessingInstructionNodeType(name));
        }

        public DomCDataSection CreateCDataSection(string data) {
            var result = CreateCDataSection(data);
            if (result == null) {
                return null;
            }
            result.SetData(data);
            return result;
        }

        public DomComment CreateComment(string data) {
            var result = CreateComment(data);
            if (result == null) {
                return null;
            }
            result.SetData(data);
            return result;
        }

        public DomDocumentType CreateDocumentType(string name, string publicId, string systemId) {
            var result = CreateDocumentType(name, publicId, systemId);
            if (result == null) {
                return null;
            }
            result.PublicId = publicId;
            result.SystemId = systemId;
            return result;
        }

        public DomProcessingInstruction CreateProcessingInstruction(string target, string data) {
            var result = CreateProcessingInstruction(data);
            if (result == null) {
                return null;
            }
            result.Data = data;
            return result;
        }

        public DomText CreateText(string data) {
            var result = CreateText(data);
            if (result == null) {
                return null;
            }
            result.Data = data;
            return result;
        }
    }
}
