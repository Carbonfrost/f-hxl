//
// - HxlTemplateTypeSelector.cs -
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
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl {

    public abstract class HxlTemplateTypeSelector : IHxlTemplateTypeSelector {

        public static readonly IHxlTemplateTypeSelector Default = new DefaultHxlTemplateTypeSelector();
        public static readonly IHxlTemplateTypeSelector Null = new NullHxlTemplateTypeSelector();

        public virtual TypeReference GetTemplateBaseClass(TypeReference defaultBaseType, TypeReference modelType) {
            return null;
        }

        public static IHxlTemplateTypeSelector Compose(params IHxlTemplateTypeSelector[] items) {
            if (items == null || items.Length == 0)
                return Null;
            if (items.Length == 1)
                return items[0];

            return new CompositeTemplateTypeSelector(items);
        }

        public static IHxlTemplateTypeSelector Compose(IEnumerable<IHxlTemplateTypeSelector> items) {
            if (items == null || !items.Any())
                return Null;

            return Compose(items.ToArray());
        }

        class NullHxlTemplateTypeSelector : IHxlTemplateTypeSelector {
            TypeReference IHxlTemplateTypeSelector.GetTemplateBaseClass(TypeReference defaultBaseClass, TypeReference modelType) {
                return null;
            }

        }

    }

}


