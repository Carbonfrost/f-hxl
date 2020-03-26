//
// - LayoutSinglePageTemplate.cs -
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
using System.Linq;
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.Commons.Hxl {

    sealed class LayoutSinglePageTemplate : HxlElementTemplate {

        private readonly string _layout;

        public LayoutSinglePageTemplate(string _layout) {
            this._layout = _layout;
        }

        protected override void Render() {
            // Only render the content placeholders
            var placeholderContent = HxlPlaceholderContentProvider.FromElement(Element);

            var masterInfo = new HxlMasterInfo(placeholderContent, null, Element, _layout);
            masterInfo.InSpa = true;

            var master = LayoutTemplate.LoadMaster(TemplateContext.TemplateFactory, false, _layout);
            var tc = TemplateContext.CreateChildContext(master);
            tc.SetMasterInfo(masterInfo);

            master.Transform(TextWriter.Null, tc);

            // TODO Could have multiple items scheduled
            // TODO Skip over single-page elements that were disabled
            // TODO Output correct spa locations

            // If Output is null writer, then we're nested
            if (ReferenceEquals(Output, TextWriter.Null))
                return;

            Output.WriteLine("{ ");
            object versionString = "1";
            Output.WriteLine(string.Format("\"version\": \"{0}\", ", versionString));
            Output.WriteLine("\"fragments\": [");

            bool comma = false;
            foreach (StringWriter b in TemplateContext.EndBufferContent("spaFragments")) {
                if (comma)
                    Output.Write(",");

                Output.WriteLine(b);
                comma = true;
            }
            Output.WriteLine("] }");
        }
    }
}
