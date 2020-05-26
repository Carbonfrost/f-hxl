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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public struct CssClassString : IEquatable<CssClassString>, IFormattable {

        static readonly Regex SPLITTER = new Regex(
            @"(?=[A-Z])",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled
        );

        private readonly string[] _propertyClasses;
        private readonly string _typeClass;

        public static readonly CssClassString Empty = "";

        public IReadOnlyList<string> Values {
            get {
                var result = new string[_propertyClasses.Length + 1];
                Array.Copy(_propertyClasses, result, _propertyClasses.Length);
                result[result.Length - 1] = _typeClass;
                return result;
            }
        }

        private IEnumerable<string> PropertyClassesWithPrefixes {
            get {
                if (string.IsNullOrEmpty(_typeClass)) {
                    return _propertyClasses;
                }

                var tc = _typeClass;
                return _propertyClasses.Select(p => ($"{tc}-{p}"));
            }
        }

        private IEnumerable<string> PropertyClassesWithSuffixes {
            get {
                if (string.IsNullOrEmpty(_typeClass)) {
                    return _propertyClasses;
                }

                var tc = _typeClass;
                return _propertyClasses.Select(p => ($"{p}-{tc}"));
            }
        }

        public CssClassString(string value) {
            if (value is null) {
                _propertyClasses = Array.Empty<string>();
            } else {
                _propertyClasses = DomStringTokenList.Parse(value).ToArray();
            }
            _typeClass = "";
        }

        public CssClassString(object value) {
            if (value == null) {
                _propertyClasses = Array.Empty<string>();
                _typeClass = "";
                return;
            }

            _propertyClasses = ConvertToPropertyClasses(value).ToArray();
            _typeClass = Hyphenate(value.GetType().Name);
        }

        public static implicit operator CssClassString(string value) {
            return new CssClassString(value);
        }

        public static CssClassString Parse(string text) {
            CssClassString result;
            if (TryParse(text, out result)) {
                return result;
            }
            throw Failure.NotParsable(nameof(text), typeof(CssClassString));
        }

        public static bool TryParse(string text, out CssClassString result) {
            result = new CssClassString(text);
            return true;
        }

        static IEnumerable<string> ConvertToPropertyClasses(object value) {
            var result = new List<string>();

            if (value is Enum) {
                result.AddRange(ConvertToPropertyClass((Enum) value));
            }

            foreach (var pd in Utility.ReflectGetProperties(value.GetType())) {
                try {
                    if (typeof(bool) == pd.PropertyType) {
                        result.AddRange(
                            ConvertToPropertyClass((bool) pd.GetValue(value), pd.Name)
                        );

                    } else if (typeof(Enum).IsAssignableFrom(pd.PropertyType))
                        result.AddRange(ConvertToPropertyClass((Enum) pd.GetValue(value)));

                } catch (Exception ex) {
                    Traceables.HandleComponentModelReflection(pd, ex);
                }
            }
            return result;
        }

        static IEnumerable<string> ConvertToPropertyClass(Enum value) {
            var v = value.ToString();

            return v.Split(new [] { ", " }, StringSplitOptions.None).Select(Hyphenate);
        }

        static IEnumerable<string> ConvertToPropertyClass(bool value, string property) {
            if (property.StartsWith("Is", StringComparison.Ordinal)) {
                property = property.Substring(2);
            }

            return value
                ? new [] { Hyphenate(property) }
                : Array.Empty<string>();
        }

        static string Hyphenate(string t) {
            var elements = SPLITTER.Split(t).Skip(1);
            return string.Join("-", elements).ToLowerInvariant();
        }

        public override string ToString() {
            return string.Join(" ", Values);
        }

        public bool Equals(CssClassString other) {
            return new HashSet<string>(Values).SetEquals(other.Values);
        }

        public override bool Equals(object obj) {
            return obj is CssClassString c && Equals(c);
        }

        public override int GetHashCode() {
            return -1939223833 + ToString().GetHashCode();
        }

        public string ToString(string format) {
            return ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            if (string.IsNullOrEmpty(format)) {
                format = "G";
            }
            switch (format.Trim()) {
                case "G":
                case "g":
                    return ToString();
            }
            var parts = new List<string>();
            foreach (var expr in Regex.Split(format.Trim(), @"(PP|pp|P|p|C|\s+)")) {
                var ee = ToStringExpr(expr);
                if (ee.Length > 0) {
                    parts.Add(ee);
                }
            }
            return string.Join(" ", parts);
        }

        private string ToStringExpr(string expr) {
            if (string.IsNullOrWhiteSpace(expr)) {
                return "";
            }
            switch (expr) {
                case "p":
                    return string.Join(" ", _propertyClasses);
                case "pp":
                    return string.Join(" ", PropertyClassesWithSuffixes);
                case "PP":
                    return string.Join(" ", PropertyClassesWithPrefixes);
                case "C":
                    return _typeClass;
                default:
                    throw new FormatException();
            }
        }
    }
}
