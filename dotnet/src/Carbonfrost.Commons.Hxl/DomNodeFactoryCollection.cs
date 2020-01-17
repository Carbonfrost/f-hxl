//
// - DomNodeFactoryCollection.cs -
//
// Copyright 2014 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.PropertyTrees;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class DomNodeFactoryCollection : NamedObjectCollection<IDomNodeFactory>, IDomNodeFactory {

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

        public DomAttribute CreateAttribute(string name, IDomValue value) {
            return Items.FirstNonNull(t => t.CreateAttribute(name, value));
        }

        public DomAttribute CreateAttribute(string name, string value) {
            return Items.FirstNonNull(t => t.CreateAttribute(name, value));
        }

        public DomCDataSection CreateCDataSection() {
            return Items.FirstNonNull(t => t.CreateCDataSection());
        }

        public DomCDataSection CreateCDataSection(string data) {
            return Items.FirstNonNull(t => t.CreateCDataSection(data));
        }

        public DomComment CreateComment() {
            return Items.FirstNonNull(t => t.CreateComment());
        }

        public DomComment CreateComment(string data) {
            return Items.FirstNonNull(t => t.CreateComment(data));
        }

        public DomDocumentType CreateDocumentType(string name, string publicId, string systemId) {
            return Items.FirstNonNull(t => t.CreateDocumentType(name, publicId, systemId));
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

        public DomProcessingInstruction CreateProcessingInstruction(string target, string data) {
            return Items.FirstNonNull(t => t.CreateProcessingInstruction(target, data));
        }

        public DomText CreateText() {
            return Items.FirstNonNull(t => t.CreateText());
        }

        public DomText CreateText(string data) {
            return Items.FirstNonNull(t => t.CreateText(data));
        }

        public Type GetAttributeNodeType(string name) {
            return Items.FirstNonNull(t => t.GetAttributeNodeType(name));
        }

        public Type GetCommentNodeType(string name) {
            return Items.FirstNonNull(t => t.GetCommentNodeType(name));
        }

        public Type GetElementNodeType(string name) {
            return Items.FirstNonNull(t => t.GetElementNodeType(name));
        }

        public Type GetEntityReferenceNodeType(string name) {
            return Items.FirstNonNull(t => t.GetEntityReferenceNodeType(name));
        }

        public Type GetEntityNodeType(string name) {
            return Items.FirstNonNull(t => t.GetEntityNodeType(name));
        }

        public Type GetNotationNodeType(string name) {
            return Items.FirstNonNull(t => t.GetNotationNodeType(name));
        }

        public Type GetProcessingInstructionNodeType(string name) {
            return Items.FirstNonNull(t => t.GetProcessingInstructionNodeType(name));
        }

        public Type GetTextNodeType(string name) {
            return Items.FirstNonNull(t => t.GetTextNodeType(name));
        }

    }
}
