//
// - RenderActionTemplate.cs -
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

namespace Carbonfrost.Commons.Hxl {

    public class RenderActionTemplate : ElementTemplate {

        public string ActionName {
            get;
            set;
        }

        public string ControllerName {
            get;
            set;
        }

        public IDictionary<string, object> RouteValues {
            get;
            set;
        }

        public ElementTemplateInfo TemplateInfo {
            get;
            set;
        }

        internal ElementFragment Body {
            get {
                return ElementFragment.Create((__closure__, __self) => RenderBody());
            }
        }

        protected override void Render() {
            IHxlRenderingProvider pro = TemplateContext.RenderingProvider;
            pro.RenderAction(Output, ActionName, ControllerName, TemplateInfo, Body, RouteValues);
        }
    }

}