//
// - HxlAttributeFragmentDefinition.cs -
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

namespace Carbonfrost.Commons.Hxl {

    class HxlAttributeFragmentDefinition {

        private static readonly IDictionary<Type, HxlAttributeFragmentDefinition> _cache
            = new Dictionary<Type, HxlAttributeFragmentDefinition>();

        private readonly Type _type;
        private PropertyInfo _valueProperty;
        private bool _init;
        private PropertyInfo[] _elementDataProperties;
        private PropertyInfo _variableProperty;

        public PropertyInfo ValueProperty {
            get {
                EnsureInit();
                return _valueProperty;
            }
        }

        public IEnumerable<PropertyInfo> ElementDataProperties {
            get {
                EnsureInit();
                return _elementDataProperties;
            }
        }

        public PropertyInfo VariableProperty {
            get {
                EnsureInit();
                return _variableProperty;
            }
        }

        public HxlAttributeFragmentDefinition(Type type) {
            _type = type;
        }

        public static HxlAttributeFragmentDefinition ForType(Type type) {
            return _cache.GetValueOrCache(type, t => new HxlAttributeFragmentDefinition(t));
        }

        public static HxlAttributeFragmentDefinition ForComponent(object component) {
            if (component == null)
                throw new ArgumentNullException("component");

            return ForType(component.GetType());
        }

        // TODO Could have multiple properties that match (uncommon)
        private void EnsureInit() {
            if (_init)
                return;

            _init = true;
            var props = Utility.ReflectGetProperties(_type);

            // Performance - don't use Attributes[] because it will create the
            // default Attribute
            var ed = new List<PropertyInfo>();
            foreach (PropertyInfo prop in Utility.ReflectGetProperties(_type)) {
                if (prop.IsDefined(typeof(ValueAttribute)))
                    _valueProperty = prop;

                if (prop.IsDefined(typeof(ElementDataAttribute)))
                    ed.Add(prop);

                if (prop.IsDefined(typeof(VariableAttribute)))
                    _variableProperty = prop;
            }
            _elementDataProperties = ed.ToArray();
        }

    }
}


