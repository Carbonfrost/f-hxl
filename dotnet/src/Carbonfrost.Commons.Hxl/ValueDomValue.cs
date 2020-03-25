//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Reflection;
using Carbonfrost.Commons.Core.Runtime;

using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    class ValueDomValue : IDomValue {

        private readonly AttributeFragment _attr;
        private readonly PropertyInfo _pd;
        private object valueCache;
        private string textCache;

        public object TypedValue {
            get {
                return valueCache;
            }
            set {
                this.valueCache = value;
                ConvertBack(value);
                _pd.SetValue(_attr, this.valueCache);
            }
        }

        public ValueDomValue(AttributeFragment attribute, PropertyInfo property) {
            this._attr = attribute;
            this._pd = property;
        }

        public bool IsReadOnly {
            get {
                return false;
            }
        }

        public string Value {
            get {
                if (this.textCache == null) {
                    ConvertBack(this.TypedValue);
                }
                return this.textCache;
            }
            set {
                this.textCache = value;
                Convert(value);
                _pd.SetValue(_attr, this.valueCache);
            }
        }

        public void Initialize(DomAttribute attribute) {
            // TODO This will be null on exiting the attribute, which shouldn't happen (design)
        }

        // TODO Error handling could be more robust

        private void ConvertBack(object value) {
            try {
                if (value == null) {
                    textCache = null;
                    return;
                }

                textCache = value.ToString();
            } catch (Exception ex) {
                Traceables.HandleComponentModelReflection(_pd, ex);
            }
        }

        private void Convert(string text) {
            try {
                valueCache = Activation.FromText(_pd.PropertyType, text);
            } catch (Exception ex) {
                Traceables.HandleComponentModelReflection(_pd, ex);
            }
        }

        public void AppendValue(object value) {
            if (value is string s) {
                Value += s;
            }
            TypedValue = value;
        }

        public IDomValue Clone() {
            return this;
        }
    }
}

