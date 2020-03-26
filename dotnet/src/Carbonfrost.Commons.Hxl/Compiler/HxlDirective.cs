//
// Copyright 2013, 2016, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Net;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public abstract class HxlDirective : HxlProcessingInstruction, IPropertiesContainer {

        // TODO Support values outside of quotes, support single quotes

        public static readonly HxlDirective Null = new NullDirective();

        private static readonly Regex KVP = new Regex(
            @"\s* (?<Key> [:_a-z]+) \s* ( = \s* (?<Value> "".*?"") ) ? \s* ", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        private readonly IProperties _properties;

        IProperties IPropertiesContainer.Properties {
            get {
                return _properties;
            }
        }

        public override string TextContent {
            get {
                return base.TextContent;
            }
            set {
                base.TextContent = value;

                // TODO Should reset other properties
                var props = _properties ?? Properties.Null;
                foreach (var k in _Parse(value)) {
                    props.SetProperty(k.Key, k.Value);
                }
            }
        }

        protected HxlDirective(string name) : base(name) {
            // TODO Should use filtered properties so that inherited properties cannot
            // be set
            _properties = Properties.FromValue(this);
        }

        internal sealed override void AcceptVisitor(IHxlVisitor visitor) {
            IHxlLanguageVisitor v = visitor as IHxlLanguageVisitor;
            if (v == null)
                base.AcceptVisitor(visitor);
            else {
                v.Visit(this);
            }
        }

        internal static IEnumerable<KeyValuePair<string, object>> Parse(string text) {
            text = (text ?? string.Empty).Trim();

            // Legacy XSP comment format
            // <? /* comment */ ?>
            if (text.Length == 0
                || text.StartsWith("//", StringComparison.Ordinal)
                || text.StartsWith("/*", StringComparison.Ordinal))
                return null;

            else
                return _Parse(text);
        }

        static IEnumerable<KeyValuePair<string, object>> _Parse(string text) {
            if (string.IsNullOrEmpty(text)) {
                yield break;
            }
            MatchCollection matches = KVP.Matches(text);
            int expectedIndex = 0;

            foreach (Match match in matches) {
                if (match.Index > expectedIndex) {
                    throw HxlFailure.InvalidDirective();
                }

                Group valueGroup = match.Groups["Value"];
                string value = null;
                value = (valueGroup.Success ? WebUtility.HtmlDecode(valueGroup.Value.Trim('"')).Trim() : null);

                string key = (match.Groups["Key"].Value ?? string.Empty).Trim();

                yield return new KeyValuePair<string, object>(key, value);

                expectedIndex = match.Index + match.Length;
            }
        }

        sealed class NullDirective : HxlDirective {
            public NullDirective() : base("hxl:null") {}
        }

    }
}
