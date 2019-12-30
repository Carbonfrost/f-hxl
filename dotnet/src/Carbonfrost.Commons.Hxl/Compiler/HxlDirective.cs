//
// Copyright 2013, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Net;
using System.Text.RegularExpressions;

using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl.Compiler;

namespace Carbonfrost.Commons.Hxl.Compiler {

    interface IFileLocationConsumer {
        void SetFileLocation(int lineNumber, int linePosition);
    }

    public abstract class HxlDirective : ProcessingInstructionFragment, IFileLocationConsumer {

        // TODO Support values outside of quotes, support single quotes

        public static readonly HxlDirective Null = new NullDirective();

        private static readonly Regex KVP = new Regex(
            @"(?<Key> [:_a-z]+) \s* ( = \s* (?<Value> "".*?"") ) ? \s* ", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        private int lineNumber;
        private int linePosition;

        protected HxlDirective(string name) : base(name) {}

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
            MatchCollection matches = KVP.Matches(text);
            int expectedIndex = 0;

            foreach (Match match in matches) {
                if (match.Index > expectedIndex)
                    throw HxlFailure.InvalidDirective();

                Group valueGroup = match.Groups["Value"];
                string value = null;
                value = (valueGroup.Success ? WebUtility.HtmlDecode(valueGroup.Value.Trim('"')).Trim() : null);

                string key = (match.Groups["Key"].Value ?? string.Empty).Trim();

                yield return new KeyValuePair<string, object>(key, value);

                expectedIndex = match.Index + match.Length;
            }
        }

        void IFileLocationConsumer.SetFileLocation(int lineNumber, int linePosition) {
            this.lineNumber = lineNumber;
            this.linePosition = linePosition;
        }

        sealed class NullDirective : HxlDirective {
            public NullDirective() : base("hxl:null") {}
        }

    }
}
