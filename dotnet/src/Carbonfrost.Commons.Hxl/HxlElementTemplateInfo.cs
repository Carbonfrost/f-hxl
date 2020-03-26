//
// Copyright 2013, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class HxlElementTemplateInfo : IEquatable<HxlElementTemplateInfo> {

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

        public HxlElementTemplateInfo(string element)
            : this(element, null)
        {
        }

        public HxlElementTemplateInfo(string element, DomStringTokenList classList) {
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

        public bool Equals(HxlElementTemplateInfo other) {
            if (other == null)
                return false;

            return this.ClassName.Equals(other.ClassName)
                && this.Element == other.Element;
        }

        public override bool Equals(object obj)  {
            HxlElementTemplateInfo other = obj as HxlElementTemplateInfo;
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

        public static bool operator ==(HxlElementTemplateInfo lhs, HxlElementTemplateInfo rhs) {
            if (ReferenceEquals(lhs, rhs))
                return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(HxlElementTemplateInfo lhs, HxlElementTemplateInfo rhs) {
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
