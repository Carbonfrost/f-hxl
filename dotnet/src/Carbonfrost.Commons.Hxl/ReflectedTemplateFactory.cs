//
// - ReflectedTemplateFactory.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Hxl {

    sealed class ReflectedTemplateFactory : IHxlTemplateFactory, IEnumerable<HxlCompiledTemplateInfo> {

        private readonly Assembly assembly;
        private HxlCompiledTemplateInfoCollection _items;

        public ReflectedTemplateFactory(Assembly assembly) {
            this.assembly = assembly;
        }

        public int TemplateCount {
            get {
                return this.Count();
            }
        }

        private HxlCompiledTemplateInfoCollection RequireMap() {
            if (_items == null)
                _items = new HxlCompiledTemplateInfoCollection(assembly);
            return _items;
        }

        public HxlTemplate CreateTemplate(string templateName, string templateType, IServiceProvider serviceProvider) {
            if (templateName == null)
                throw new ArgumentNullException("templateName");
            if (string.IsNullOrEmpty(templateName))
                throw Failure.EmptyString("templateName");

            var info = RequireMap().FindTemplate(templateName, templateType);
            HxlTemplate result = null;
            Type type = null;

            if (info != null) {
                type = info.CompiledType;

                try {

                    result = (HxlTemplate) Activator.CreateInstance(type);

                } catch (Exception ex) {
                    Traceables.ReflectedTemplateFactoryCreateTemplateError(
                        assembly,
                        type,
                        templateName,
                        ex);

                    throw;
                }
            }

            Traceables.ReflectedTemplateFactoryCreateTemplate(
                assembly,
                type,
                templateName,
                result != null);
            return result;
        }

        public IEnumerator<HxlCompiledTemplateInfo> GetEnumerator() {
            return RequireMap().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
