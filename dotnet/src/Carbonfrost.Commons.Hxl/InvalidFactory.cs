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
using System.Linq;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    class InvalidFactory : HxlDomNodeFactory {

        public override DomElement CreateElement(HxlQualifiedName name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            throw HxlFailure.NoMatchingServerElement(name.FullName);
        }

        public override DomAttribute CreateAttribute(HxlQualifiedName name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            throw HxlFailure.NoMatchingServerAttribute(name.FullName);
        }

        public override Type GetAttributeNodeType(HxlQualifiedName name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            throw HxlFailure.NoMatchingServerAttribute(name.FullName);
        }

        public override Type GetElementNodeType(HxlQualifiedName name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            throw HxlFailure.NoMatchingServerElement(name.FullName);
        }

        protected override Uri LookupNamespace(string prefix) {
            var result = base.LookupNamespace(prefix);
            if (result == null) {
                throw HxlFailure.PrefixDoesNotMapToNamespace(prefix);
            }
            return result;
        }
    }
}
