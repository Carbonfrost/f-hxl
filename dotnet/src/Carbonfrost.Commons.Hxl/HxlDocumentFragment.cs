//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.IO;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class HxlDocumentFragment : DomDocumentFragment, IDomNodeFactoryApiConventions, IHxlDocumentWriter {

        // TODO These delegates and IDomNodeFactoryApiConventions can be removed with upgrade to fwebdom

        public DomAttribute CreateAttribute(string name, IDomValue value) {
            return OwnerDocument.CreateAttribute(name, value);
        }

        public DomAttribute CreateAttribute(string name, string value) {
            return OwnerDocument.CreateAttribute(name, value);
        }

        public DomAttribute CreateAttribute(string name) {
            return OwnerDocument.CreateAttribute(name);
        }

        public DomCDataSection CreateCDataSection(string data) {
            return OwnerDocument.CreateCDataSection(data);
        }

        public DomCDataSection CreateCDataSection() {
            return OwnerDocument.CreateCDataSection();
        }

        public DomComment CreateComment(string data) {
            return OwnerDocument.CreateComment(data);
        }

        public DomComment CreateComment() {
            return OwnerDocument.CreateComment();
        }

        public DomDocumentFragment CreateDocumentFragment() {
            return OwnerDocument.CreateDocumentFragment();
        }

        public DomDocumentType CreateDocumentType(string name, string publicId, string systemId) {
            return OwnerDocument.CreateDocumentType(name, publicId, systemId);
        }

        public DomDocumentType CreateDocumentType(string name) {
            return OwnerDocument.CreateDocumentType(name);
        }

        public DomElement CreateElement(string name) {
            return OwnerDocument.CreateElement(name);
        }

        public DomEntity CreateEntity(string name) {
            return OwnerDocument.CreateEntity(name);
        }

        public DomEntityReference CreateEntityReference(string name) {
            return OwnerDocument.CreateEntityReference(name);
        }

        public DomNotation CreateNotation(string name) {
            return OwnerDocument.CreateNotation(name);
        }

        public DomProcessingInstruction CreateProcessingInstruction(string target, string data) {
            return OwnerDocument.CreateProcessingInstruction(target, data);
        }

        public DomProcessingInstruction CreateProcessingInstruction(string target) {
            return OwnerDocument.CreateProcessingInstruction(target);
        }

        public DomText CreateText(string data) {
            return OwnerDocument.CreateText(data);
        }

        public DomText CreateText() {
            return OwnerDocument.CreateText();
        }

        public Type GetAttributeNodeType(string name) {
            return OwnerDocument.GetAttributeNodeType(name);
        }

        public Type GetElementNodeType(string name) {
            return OwnerDocument.GetElementNodeType(name);
        }

        public Type GetProcessingInstructionNodeType(string target) {
            return OwnerDocument.GetProcessingInstructionNodeType(target);
        }

        public void WriteTo(TextWriter writer, HxlTemplateContext templateContext) {
            WriteTo(writer, new HxlWriterSettings {
                TemplateContext = templateContext
            });
        }

        public void WriteTo(TextWriter writer, HxlWriterSettings settings) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            new HxlWriter(writer, settings).Write(this);
        }
    }
}
