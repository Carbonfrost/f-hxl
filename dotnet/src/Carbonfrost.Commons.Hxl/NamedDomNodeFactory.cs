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

using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    class NamedDomNodeFactory : IDomNodeFactory, IEnumerable<IDomNodeFactory> {

        readonly string _name;
        readonly IDomNodeFactory _factory;

        public NamedDomNodeFactory(string name, IDomNodeFactory factory) {
            _name = name;
            _factory = factory;
        }

        public string Name {
            get {
                return _name;
            }
        }

        public IEnumerator<IDomNodeFactory> GetEnumerator() {
            yield return _factory;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public DomAttribute CreateAttribute(string name) {
            return _factory.CreateAttribute(name);
        }

        public DomCDataSection CreateCDataSection() {
            return _factory.CreateCDataSection();
        }

        public DomComment CreateComment() {
            return _factory.CreateComment();
        }

        public DomDocumentFragment CreateDocumentFragment() {
            return _factory.CreateDocumentFragment();
        }

        public DomEntity CreateEntity(string name) {
            return _factory.CreateEntity(name);
        }

        public DomNotation CreateNotation(string name) {
            return _factory.CreateNotation(name);
        }

        public DomDocumentType CreateDocumentType(string name) {
            return _factory.CreateDocumentType(name);
        }

        public DomElement CreateElement(string name) {
            return _factory.CreateElement(name);
        }

        public DomEntityReference CreateEntityReference(string data) {
            return _factory.CreateEntityReference(data);
        }

        public DomProcessingInstruction CreateProcessingInstruction(string target) {
            return _factory.CreateProcessingInstruction(target);
        }

        public DomText CreateText() {
            return _factory.CreateText();
        }

        public DomText CreateText(string data) {
            return _factory.CreateText();
        }

        public Type GetAttributeNodeType(string name) {
            return _factory.GetAttributeNodeType(name);
        }

        public Type GetElementNodeType(string name) {
            return _factory.GetElementNodeType(name);
        }

        public Type GetProcessingInstructionNodeType(string name) {
            return _factory.GetProcessingInstructionNodeType(name);
        }
    }
}
