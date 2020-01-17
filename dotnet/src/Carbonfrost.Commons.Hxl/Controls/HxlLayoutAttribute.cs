//
// - HxlLayoutAttribute.cs -
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
using System.Collections.Generic;
using System.Linq;
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.Commons.Hxl.Controls {

    [AttributeFragmentUsage]
    public class HxlLayoutAttribute : AttributeFragment {

        // TODO Policy in attribute should prevent html>head from being inlined

        [Value]
        public string Layout {
            get; set;
        }

        public HxlLayoutAttribute() : base("hxl:layout") {
        }

        protected override IElementTemplate OnElementRendering() {
            if (SinglePage)
                return new LayoutSinglePageTemplate(Layout);
            else
                return new LayoutTemplate(Layout, false);
        }

        private bool SinglePage {
            get {
                var layouts = TemplateContext.SinglePageLayoutInfo;
                return layouts != null;
            }
        }

        internal bool IsDisabled(string placeholder, string location) {
            PageLayoutInfo layouts = TemplateContext.SinglePageLayoutInfo;
            if (layouts == null)
                return false;

            string current;
            if (layouts.Locations.TryGetValue(placeholder, out current)) {
                // Disabled if the location is the same
                return current == location;
            }

            return false;
        }
    }

}

