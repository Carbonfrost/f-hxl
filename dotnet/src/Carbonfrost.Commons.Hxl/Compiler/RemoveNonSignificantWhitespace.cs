//
// - RemoveNonSignificantWhitespace.cs -
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
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    using WhitespaceHandler = Action<DomElement>;

    class RemoveNonSignificantWhitespace : HxlCompilerProcessor {

        internal static readonly RemoveNonSignificantWhitespace Instance
            = new RemoveNonSignificantWhitespace();

        // Technically, this is an optimization, but it is easier to apply
        // to the DomDocument than the HXL

        private static readonly Regex COMPRESS = new Regex(@"\s+");

        private static readonly string[] WS_REMOVE = {
            "html",
            "head",

            "table",
            "tbody",
            "thead",
            "tfoot",
            "tr",

            "dl",
            "ul",
            "ol",
        };

        static readonly string[] WS_TRIM = {
            "a",
        };

        static readonly IDictionary<string, WhitespaceHandler>
            WS_HANDLING = new Dictionary<string, WhitespaceHandler>(StringComparer.OrdinalIgnoreCase);

        static RemoveNonSignificantWhitespace() {
            foreach (var s in WS_REMOVE)
                WS_HANDLING.Add(s, RemoveText);

            foreach (var s in WS_TRIM)
                WS_HANDLING.Add(s, TrimText);
        }

        public override void Preprocess(DomDocument document, IServiceProvider serviceProvider) {
            foreach (var m in document.ChildNodes.OfType<DomElement>())
                ProcessElement(m);
        }

        static void ProcessElement(DomElement e) {
            foreach (var child in e.Elements)
                ProcessElement(child);

            WhitespaceHandler action = FindAction(e);
            action(e);
        }

        static WhitespaceHandler FindAction(DomElement e) {
            WhitespaceHandler action;
            if (WS_HANDLING.TryGetValue(e.NodeName, out action)) {
                return action;
            }

            return Empty<DomElement>.Action;
        }

        private static void RemoveText(DomElement self) {
            if (!self.ChildNodes.Any(t => t.IsText))
                return;

            // TODO Copying to an array is wasteful (perf)
            foreach (var node in self.ChildNodes.ToArray()) {
                if (node.IsText && string.IsNullOrWhiteSpace(node.TextContent)) {
                    node.RemoveSelf();
                }
            }
        }

        private static void TrimText(DomElement self) {
            if (self.ChildNodes.Count == 0)
                return;

            if (self.FirstChildNode.IsText) {
                var text = (DomText) self.FirstChildNode;
                text.Data = text.Data.TrimStart();
            }

            if (self.LastChildNode.IsText) {
                var text = (DomText) self.LastChildNode;
                text.Data = text.Data.TrimEnd();
            }

            CompressWS(self);
        }

        private static void CompressWS(DomElement self) {
            foreach (var node in self.ChildNodes) {
                if (node.IsText) {
                    var text = (DomText) node;
                    // text.Data = COMPRESS.Replace(text.Data, " ");
                }
            }
        }

    }
}


