//
// Copyright 2014, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using Carbonfrost.Commons.Hxl.Compiler;

namespace Carbonfrost.Commons.Hxl {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class HxlTemplateAttribute : Attribute  {

        public string Name { get; set; }
        public string Type { get; set; }

        internal static void NameOrDefault(HxlTemplate template, out string name, out string type) {
            var builder = template as IHxlTemplateBuilder;
            NameOrDefault(template.GetType(), out name, out type);

            if (builder != null && !string.IsNullOrEmpty(builder.TemplateName)) {
                name = builder.TemplateName;
            }
        }

        internal static void NameOrDefault(Type templateType, out string name, out string type) {
            var attr = (HxlTemplateAttribute) templateType.GetTypeInfo().GetCustomAttribute(typeof(HxlTemplateAttribute));

            if (attr == null) {
                name = templateType.Name;
                type = "*";
            } else {
                // TODO Better algorithm for generating names
                name = string.IsNullOrEmpty(attr.Name) ? templateType.Name : attr.Name;
                type = string.IsNullOrEmpty(attr.Type) ? "*" : attr.Type;
            }
        }

    }
}
