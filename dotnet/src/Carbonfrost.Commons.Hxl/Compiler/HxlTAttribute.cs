//
// - HxlTAttribute.cs -
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
using System.IO;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    [AttributeFragmentUsage(Priority = AttributeFragmentPriority.Layout)]
    public sealed class HxlTAttribute : HxlLangAttribute {

        [Value]
        public string Template { get; set; }

        internal HxlTAttribute() : base("c:t") {
        }

        internal override void AcceptVisitor(IHxlLanguageVisitor visitor) {
             visitor.Visit(this);
        }

        internal override DomElement ApplyToElement(DomElement newElement) {
            // Change the element's render mode
            OwnerElement.AddAnnotation(new RenderAnnotation(Template));
            OwnerElement.Retain();
            return newElement;
        }

        class RenderAnnotation : IRenderWorkElementCodeBuilder {

            private readonly string _name;

            public RenderAnnotation(string _name) {
                this._name = _name;
            }

            public void EmitCode(TextWriter output, string varName, DomElement workElement) {
                output.Write("((global::{3}) {0}).SetElementTemplate(global::{2}.FromTemplate(\"{1}\"));",
                             varName, _name, typeof(ElementTemplate).FullName, typeof(DomElement).FullName);
            }

        }
    }
}

