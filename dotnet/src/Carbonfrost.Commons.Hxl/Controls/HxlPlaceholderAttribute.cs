//
// - HxlPlaceholderAttribute.cs -
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
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.Commons.Hxl.Controls {

    [AttributeFragmentUsage(Priority = AttributeFragmentPriority.Layout)]
    public class HxlPlaceholderAttribute : AttributeFragment {

        private string _placeholder;

        [Value]
        public string Placeholder {
            get {
                return HxlPlaceholderContentProvider.GetImpliedPlaceholderName(OwnerElement, _placeholder);
            }
            set {
                _placeholder = value;
            }
        }

        public HxlPlaceholderAttribute() : base("hxl:placeholder") {
        }

        protected override IElementTemplate OnElementRendering() {
            return new PlaceholderTemplate(this.Placeholder);
        }

        class PlaceholderTemplate : ElementTemplate {

            private readonly string name;

            public PlaceholderTemplate(string name) {
                this.name = name;
            }

            protected override void Render() {
                var master = TemplateContext.FindMasterInfo();
                HxlWriter output = HxlOutput;
                bool spa = master.InSpa;

                if (spa) {
                    // Could be recursive
                    WriteLine("<{0} class=\"app-placeholder\" data-placeholder=\"{1}\"></{0}>",
                              Element.NodeName,
                              name);
                    output = TemplateContext.StartBufferContent("spaFragments");

                    var writer = ((HxlTemplateContext.SpaFragmentWriter) output.BaseWriter);
                    writer.Name = name;
                }

                HxlPlaceholderContentProvider.Render(
                    this.Element,
                    name,
                    master.LayoutName,
                    output);
            }

        }
    }

}
