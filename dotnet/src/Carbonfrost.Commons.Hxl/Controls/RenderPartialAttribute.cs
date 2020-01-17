//
// - RenderPartialAttribute.cs -
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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.Commons.Hxl.Controls {

    public abstract class RenderPartialAttribute : AttributeFragment {

        public virtual string ViewName {
            get {
                return Utility.GetImplicitName(GetType());
            }
        }

        public object Model {
            get;
            set;
        }

        protected RenderPartialAttribute() : base() {
        }

        protected RenderPartialAttribute(string name) : base(name) {
        }

        protected override IElementTemplate OnElementRendering() {
            var element = this.OwnerElement;
            var data = GetViewData();

            return new RenderPartialTemplate {
                ViewData = data,
                Model = data,
                PartialViewName = this.ViewName,
                TemplateInfo = new ElementTemplateInfo(element.Name, element.Attribute("class"))
            };
        }

        protected virtual void FilterViewData(IDictionary<string, object> routeValues) {
        }

        private IDictionary<string, object> GetViewData() {
            var data = new Dictionary<string, object>();

            foreach (PropertyInfo pd in Utility.ReflectGetProperties(GetType())) {
                try {
                    data.Add(pd.Name, pd.GetValue(this));
                } catch (Exception ex) {
                    Traceables.HandleComponentModelReflection(pd, ex);
                }
            }

            FilterViewData(data);
            return data;
        }
    }
}



