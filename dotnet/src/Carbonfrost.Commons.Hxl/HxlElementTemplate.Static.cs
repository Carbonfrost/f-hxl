//
// - ElementTemplate.Static.cs -
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
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    partial class HxlElementTemplate {

        public static readonly IHxlElementTemplate InnerHtml = new InnerHtmlElementTemplate();
        public static readonly IHxlElementTemplate Default = new DefaultElementTemplate();
        public static readonly IHxlElementTemplate Skip = new EmptyElementTemplate();

        public static IHxlElementTemplate FromRenderMode(RenderMode mode) {
            return mode.ToTemplate();
        }

        public static IHxlElementTemplate FromLayout(string layout) {
            return new LayoutTemplate(layout, false);
        }

        public static IHxlElementTemplate Async(HxlElementTemplate template) {
            if (template == null)
                throw new ArgumentNullException("template");

            return new AsyncTemplate(template);
        }

        public static IHxlElementTemplate RenderAction() {
            throw new NotImplementedException();
        }

        public static IHxlElementTemplate RenderPartial() {
            throw new NotImplementedException();
        }

        public static IHxlElementTemplate RenderUrl() {
            throw new NotImplementedException();
        }

        public static IHxlElementTemplate FromTemplate(string name) {
            return new RenderTemplateImpl(name, null);
        }

        public static IHxlElementTemplate FromTemplate(string type, string name) {
            return new RenderTemplateImpl(name, type);
        }

        class RenderTemplateImpl : IHxlElementTemplate {

            private readonly string _name;
            private readonly string _type;

            public RenderTemplateImpl(string name, string type) {
                _name = name;
                _type = type;
            }

            public void Render(DomElement element, HxlWriter output) {
                var eti = new HxlElementTemplateInfo(element.Name, element.Attribute("class"));
                var services = ServiceProvider.Compose(
                    ServiceProvider.Current,
                    ServiceProvider.FromValue(eti));

               var template = output.TemplateContext.TemplateFactory.CreateTemplate(_name, _type, services);
                if (template == null) {
                    // TODO Missing line number
                    throw HxlFailure.CannotFindMatchingTemplate(_type, _name, -1, -1);
                }

                var context = output.TemplateContext.CreateChildContext(template);
                context.Data.Add("element", element);

                template.Transform(output.BaseWriter, context);
            }

        }
    }
}


