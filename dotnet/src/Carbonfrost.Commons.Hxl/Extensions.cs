//
// - Extensions.cs -
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

namespace Carbonfrost.Commons.Hxl {

    public static class Extensions {

        public static IElementTemplate GetElementTemplate(this DomElement element) {
            if (element == null)
                throw new ArgumentNullException("element");

            var result = element.Annotation<ElementTemplateAnnotation>();
            return result == null ? null : result.Template;
        }

        public static void SetElementTemplate(this DomElement element, IElementTemplate template) {
            if (element == null)
                throw new ArgumentNullException("element");

            if (template == null) {
                element.RemoveAnnotations<ElementTemplateAnnotation>();
                return;
            }

            var result = element.Annotation<ElementTemplateAnnotation>();
            if (result == null) {
                result = new ElementTemplateAnnotation();
                element.AddAnnotation(result);
            }
            result.Template = template;
        }

        private class ElementTemplateAnnotation {
            public IElementTemplate Template;
        }

    }
}
