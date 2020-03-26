//
// - HxlLangAttribute.cs -
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

namespace Carbonfrost.Commons.Hxl.Compiler {

    public abstract class HxlLangAttribute : HxlAttribute {

        internal HxlLangAttribute(string name) : base(name) {
        }

        protected override IHxlElementTemplate OnElementRendering() {
            throw FutureFeatures.InterpretedLanguageElements();
        }

        internal sealed override void AcceptVisitor(IHxlVisitor visitor) {
            IHxlLanguageVisitor v = visitor as IHxlLanguageVisitor;

            if (v == null)
                base.AcceptVisitor(visitor);
            else
                AcceptVisitor(v);
        }

        internal abstract void AcceptVisitor(IHxlLanguageVisitor visitor);

        internal virtual HxlLangElement ConvertToElement() {
            return null;
        }

        internal virtual DomElement ApplyToElement(DomElement newElement) {
            var newEle = ConvertToElement();

            if (newEle != null) {
                if (newElement == null)
                    newElement = newEle;
                else {
                    newElement.Append(newEle);
                }
            }

            return newElement;
        }
    }
}
