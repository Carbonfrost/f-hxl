//
// - CssClassString.cs -
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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Carbonfrost.Commons.Hxl {

    public class CssClassString {

        static readonly Regex SPLITTER = new Regex(
            @"(?=[A-Z])",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        private readonly object _value;

        public CssClassString(object _value) {
            this._value = _value;
        }

        static string ConvertToClass(Enum value) {
            var v = value.ToString();

            return string.Join(" ", v.Split(new [] { ", " }, StringSplitOptions.None).Select(Hyphenate));
        }

        static string ConvertToClass(bool value, string property) {
            if (property.StartsWith("Is", StringComparison.Ordinal))
                property = property.Substring(2);

            return value ? Hyphenate(property) : string.Empty;
        }

        static string Hyphenate(string t) {
            var elements = SPLITTER.Split(t).Skip(1);
            return string.Join("-", elements).ToLowerInvariant();
        }

        public override string ToString() {
            var sb = new StringBuilder();

            if (_value is Enum)
                sb.Append(ConvertToClass((Enum) _value));

            foreach (PropertyInfo pd in Utility.ReflectGetProperties(_value.GetType())) {
                try {
                    if (typeof(bool) == pd.PropertyType) {
                        var boolStr = ConvertToClass((bool)pd.GetValue(_value), pd.Name);

                        if (boolStr.Length > 0)
                            sb.AppendSeparator(" ").Append(boolStr);

                    } else if (typeof(Enum).IsAssignableFrom(pd.PropertyType))
                        sb.AppendSeparator(" ").Append(ConvertToClass((Enum)pd.GetValue(_value)));
                } catch (Exception ex) {
                    Traceables.HandleComponentModelReflection(pd, ex);
                }
            }

            sb.AppendSeparator(" ");
            sb.Append(Hyphenate(_value.GetType().Name));

            return sb.ToString();
        }

    }
}
