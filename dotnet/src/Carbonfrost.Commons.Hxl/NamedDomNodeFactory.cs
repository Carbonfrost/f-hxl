//
// - NamedDomNodeFactory.cs -
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

using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    class NamedDomNodeFactory : IDomNodeFactory, IEnumerable<IDomNodeFactory> {

        readonly string name;
        readonly IDomNodeFactory factory;

        public NamedDomNodeFactory(string name, IDomNodeFactory factory) {
            this.name = name;
            this.factory = factory;
        }

        public string Name {
            get {
                return this.name;
            }
        }

        public IEnumerator<IDomNodeFactory> GetEnumerator() {
            yield return factory;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public DomAttribute CreateAttribute(string name) {
            return factory.CreateAttribute(name);
        }

        public DomAttribute CreateAttribute(string name, IDomValue value) {
            return factory.CreateAttribute(name, value);
        }

        public DomAttribute CreateAttribute(string name, string value) {
            return factory.CreateAttribute(name, value);
        }
        public DomCDataSection CreateCDataSection() {
            return factory.CreateCDataSection(); }

        public DomCDataSection CreateCDataSection(string data) {
            return factory.CreateCDataSection(data);
        }

        public DomComment CreateComment() {
            return factory.CreateComment();
        }

        public DomComment CreateComment(string data) {
            return factory.CreateComment(data);
        }

        public DomDocumentType CreateDocumentType(string name, string publicId, string systemId) {
            return factory.CreateDocumentType(name, publicId, systemId);
        }

        public DomElement CreateElement(string name) {
            return factory.CreateElement(name);
        }

        public DomEntityReference CreateEntityReference(string data) {
            return factory.CreateEntityReference(data);
        }

        public DomProcessingInstruction CreateProcessingInstruction(string target) {
            return factory.CreateProcessingInstruction(target);
        }

        public DomProcessingInstruction CreateProcessingInstruction(string target, string data) {
            return factory.CreateProcessingInstruction(target, data);
        }

        public DomText CreateText() {
            return factory.CreateText();
        }

        public DomText CreateText(string data) {
            return factory.CreateText();
        }

        public Type GetAttributeNodeType(string name) {
            return factory.GetAttributeNodeType(name);
        }

        public Type GetCommentNodeType(string name) {
            return factory.GetCommentNodeType(name);
        }

        public Type GetElementNodeType(string name) {
            return factory.GetElementNodeType(name);
        }

        public Type GetEntityReferenceNodeType(string name) {
            return factory.GetEntityReferenceNodeType(name);
        }

        public Type GetEntityNodeType(string name) {
            return factory.GetEntityNodeType(name);
        }

        public Type GetNotationNodeType(string name) {
            return factory.GetNotationNodeType(name);
        }

        public Type GetProcessingInstructionNodeType(string name) {
            return factory.GetProcessingInstructionNodeType(name);
        }

        public Type GetTextNodeType(string name) {
            return factory.GetTextNodeType(name);
        }
    }
}
