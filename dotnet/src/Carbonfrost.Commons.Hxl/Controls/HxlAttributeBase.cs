//
// - HxlAttributeBase.cs -
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
using System.Linq;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.Commons.Hxl.Controls {

    public abstract class HxlAttributeBase : AttributeFragment {

        // TODO Implement HxlAttributeBase, consider renaming
        // TODO html:charset, etc. for working with HTML
        // c:class:text, c:class:feature, c:class:media, c:class:append=yes
        // ConvertToValue() - strong typing

        public DomStringTokenList Features { get; set; }

        readonly string htmlName;

        internal HxlAttributeBase(string name) : base("hxl:" + name) {
            this.htmlName = name;
        }

        protected sealed override IElementTemplate OnElementRendering() {
            this.OwnerElement.Attribute(this.htmlName, this.Value);
            return base.OnElementRendering();
        }

    }
}
