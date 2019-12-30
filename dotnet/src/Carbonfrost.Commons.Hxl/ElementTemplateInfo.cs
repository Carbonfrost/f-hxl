//
// - ElementTemplateInfo.cs -
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
using System.Linq;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class ElementTemplateInfo : IEquatable<ElementTemplateInfo> {

        private readonly DomStringTokenList _classList;
        private readonly string _element;

        public string ClassName {
            get {
                return ClassList.Value;
            }
        }

        public DomStringTokenList ClassList {
            get {
                return _classList;
            }
        }

        public string Element {
            get {
                return _element;
            }
        }

        public ElementTemplateInfo(string element)
            : this(element, null)
        {
        }

        public ElementTemplateInfo(string element, DomStringTokenList classList) {
            if (element == null)
                throw new ArgumentNullException("element");
            if (string.IsNullOrEmpty(element))
                throw Failure.EmptyString("element");

            if (classList == null)
                _classList = new DomStringTokenList();
            else
                _classList = classList.IsReadOnly ? classList : classList.Clone();

            // UNDONE _classList.MakeReadOnly();
            _element = element;
        }

        public bool Equals(ElementTemplateInfo other) {
            if (other == null)
                return false;

            return this.ClassName.Equals(other.ClassName)
                && this.Element == other.Element;
        }

        public override bool Equals(object obj)  {
            ElementTemplateInfo other = obj as ElementTemplateInfo;
            return Equals(other);
        }

        public override int GetHashCode() {
            int hashCode = 0;
            unchecked {
                hashCode += 1000000007 * _classList.GetHashCode();
                hashCode += 1000000009 * _element.GetHashCode();
            }
            return hashCode;
        }

        public static bool operator ==(ElementTemplateInfo lhs, ElementTemplateInfo rhs) {
            if (ReferenceEquals(lhs, rhs))
                return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ElementTemplateInfo lhs, ElementTemplateInfo rhs) {
            return !(lhs == rhs);
        }

        public override string ToString() {
            if (this.ClassList.Count == 0)
                return Element;
            else
                return string.Concat(Element, ".", string.Join(".", (IEnumerable<string>) ClassList));
        }

    }
}
