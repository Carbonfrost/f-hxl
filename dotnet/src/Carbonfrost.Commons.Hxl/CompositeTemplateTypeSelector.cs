//
// - CompositeTemplateTypeSelector.cs -
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
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl {

    class CompositeTemplateTypeSelector : IHxlTemplateTypeSelector {

        private readonly IHxlTemplateTypeSelector[] _items;

        public CompositeTemplateTypeSelector(IHxlTemplateTypeSelector[] items) {
            _items = items;
        }

        TypeReference IHxlTemplateTypeSelector.GetTemplateBaseClass(TypeReference defaultBaseClass, TypeReference modelType) {
            return FirstNonNull(t => t.GetTemplateBaseClass(defaultBaseClass, modelType));
        }

        TResult FirstNonNull<TResult>(Func<IHxlTemplateTypeSelector, TResult> action)
            where TResult : class
        {

            foreach (var m in _items) {
                var result = action(m);
                if (result != null)
                    return result;
            }

            return null;
        }

    }
}
