//
// - LayoutTemplate.cs -
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
using System.Linq;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.Commons.Hxl {

    // Renders a given layout in place of the element
    sealed class LayoutTemplate : ElementTemplate {

        readonly string _layoutName;
        readonly bool _ignoreErrors;

        public LayoutTemplate(string layoutName, bool ignoreErrors) {
            this._ignoreErrors = ignoreErrors;
            this._layoutName = layoutName;
        }

        protected override void Render() {
            // Capture placeholder content
            var placeholderContent = HxlPlaceholderContentProvider.FromElement(Element);

            HxlMasterInfo masterInfo = new HxlMasterInfo(placeholderContent, null, this.Element, _layoutName);
            var output = this.Output;

            HxlTemplateContext context = this.TemplateContext;
            var master = LoadMaster(context.TemplateFactory, _ignoreErrors, _layoutName);

            var childContext = context.CreateChildContext(master);
            childContext.SetMasterInfo(masterInfo);

            master.Transform(output, childContext);
        }

        internal static HxlTemplate LoadMaster(
            IHxlTemplateFactory templateFactory,
            bool ignoreErrors,
            string layoutName)
        {
            if (templateFactory == HxlTemplateFactory.Null) {
                if (ignoreErrors)
                    return null;
                throw HxlFailure.CannotLoadAnyMasterTemplates();
            }
            var master = templateFactory.CreateTemplate(layoutName, "layout", null);
            if (master == null) {
                if (ignoreErrors)
                    return null;
                throw HxlFailure.CannotLoadMasterTemplate(layoutName);
            }

            return master;
        }
    }
}
