//
// - HxlTemplateFactoryCollection.cs -
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
using System.Reflection;

using Carbonfrost.Commons.PropertyTrees;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl  {

    public class HxlTemplateFactoryCollection : NamedObjectCollection<IHxlTemplateFactory>, IHxlTemplateFactory {

        [Add]
        public IHxlTemplateFactory AddNew(string name, TypeReference type) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(name))
                throw Failure.EmptyString("name");
            if (type == null)
                throw new ArgumentNullException("type");

            var result = HxlTemplateFactory.LateBound(type);
            Add(name, result);
            return result;
        }

        [Add]
        public IHxlTemplateFactory AddAssembly(Assembly assembly) {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            var result = HxlTemplateFactory.FromAssembly(assembly);
            Add(assembly.GetName().Name, result);
            return result;
        }

        public HxlTemplate CreateTemplate(string templateName, string templateType, IServiceProvider serviceProvider) {
            return this.Items.FirstNonNull(t => t.CreateTemplate(templateName, templateType, serviceProvider));
        }
    }
}
