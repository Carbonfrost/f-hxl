//
// - CompositeTemplateFactory.cs -
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
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Hxl {

    sealed class CompositeTemplateFactory : IHxlTemplateFactory {

        readonly IDictionary<TemplateKey, IHxlTemplateFactory> previousCache = new Dictionary<TemplateKey, IHxlTemplateFactory>();
        readonly IHxlTemplateFactory[] values;

        public CompositeTemplateFactory(IHxlTemplateFactory[] values) {
            this.values = values;
        }

        public HxlTemplate CreateTemplate(string templateName, string templateType, IServiceProvider serviceProvider) {
            if (templateName == null)
                throw new ArgumentNullException("templateName");
            if (string.IsNullOrEmpty(templateName))
                throw Failure.EmptyString("templateName");

            // Optimize by reusing previous factory by name
            IHxlTemplateFactory previous;
            var key = new TemplateKey(templateName, templateType);
            if (previousCache.TryGetValue(key, out previous)) {
                var result = previous.CreateTemplate(templateName, templateType, serviceProvider);
                if (result != null)
                    return result;
            }

            foreach (var f in values) {
                var template = f.CreateTemplate(templateName, templateType, serviceProvider);
                if (template != null) {
                    previousCache[key] = f;
                    return template;
                }
            }

            return null;
        }
    }
}
