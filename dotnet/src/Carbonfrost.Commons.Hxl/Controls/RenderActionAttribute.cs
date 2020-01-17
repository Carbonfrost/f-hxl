//
// - RenderActionAttribute.cs -
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
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.Commons.Hxl.Controls {

    public abstract class RenderActionAttribute : AttributeFragment {

        public virtual string ActionName {
            get {
                return Utility.GetImplicitActionName(GetType());
            }
        }

        public virtual string ControllerName {
            get {
                return Utility.GetImplicitControllerName(GetType());
            }
        }

        protected RenderActionAttribute() : base() {
        }

        protected RenderActionAttribute(string name) : base(name) {
        }

        protected virtual void FilterRouteValues(IDictionary<string, object> routeValues) {
        }

        private IDictionary<string, object> GetRouteValues() {
            var routeValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (PropertyInfo pd in Utility.ReflectGetProperties(GetType())) {
                try {
                    routeValues.Add(pd.Name, pd.GetValue(this));
                }
                catch (Exception ex) {
                    Traceables.HandleComponentModelReflection(pd, ex);
                }
            }

            routeValues.Remove("ActionName");
            routeValues.Remove("ControllerName");
            routeValues.Add("action", ActionName);
            routeValues.Add("controller", ControllerName);

            var ep = TemplateContext.DataProviders["elementData"] as IPropertyStore;
            if (ep != null) {
                foreach (var kvp in ep) {
                    routeValues.Add(kvp.Key, kvp.Value);
                }
            }

            // TODO Work element data
            // routeValues.Add("elementData", new PropertiesObject(ep));

            // TODO Consider allowing other route values
            // routeValues.Add("body", this.OwnerElement.Body);

            // routeValues.Add("element", this.Element);
            // routeValues.Add("attributes", this.Element.Attributes);
            // routeValues.Add("id", this.Element.Attributes);
            // routeValues.Add("class", this.Element.Attributes);

            FilterRouteValues(routeValues);
            return routeValues;
        }

        protected override IElementTemplate OnElementRendering() {
            var rvd = GetRouteValues();
            var e = OwnerElement;

            return new RenderActionTemplate {
                ActionName = this.ActionName,
                ControllerName = this.ControllerName,
                TemplateInfo = new ElementTemplateInfo(e.Name, e.Attribute("class")),
                RouteValues = rvd,
            };
        }

    }
}

