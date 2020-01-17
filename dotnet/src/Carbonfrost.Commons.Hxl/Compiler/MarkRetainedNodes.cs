//
// - MarkRetainedNodes.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class MarkRetainedNodes : HxlCompilerProcessor {

        internal static readonly MarkRetainedNodes Instance
            = new MarkRetainedNodes();

        public override void Preprocess(DomDocument document, IServiceProvider serviceProvider) {
            Rewrite(document);
        }

        private static void Rewrite(DomDocument document) {
            ProcessElement(document);
        }

        static bool ProcessElement(DomNode item) {
            // Always retain html > head and doctype
            // (Unfortunately, we must since we can't tell whether the
            // document is being used as a master.)
            if (item.NodeType == DomNodeType.DocumentType) {
                item.Retain();
                return true;
            }
            if (item.NodeName =="head") {
                item.Retain();

                // TODO Head can be retained in such a way that its
                // children don't need retaining (performance)
                foreach (var m in ((DomElement) item).Elements) {
                    m.Retain();
                }
                return true;
            }
            if (ConsiderChildren(item)
                || (item.IsElement && item is ElementFragment)) {

                item.Retain();
                return true;

            } else {
                return false;
            }
        }

        static bool ProcessAttribute(DomAttribute item) {
            // TODO Could be escaped expression (performance)

            if (item is AttributeFragment
                || (item.Value ?? string.Empty).Contains("$")) {

                item.Retain();
                return true;

            } else {
                return false;
            }
        }

        static bool ConsiderChildren(DomNode item) {
            bool result = false;

            if (item.Attributes != null) {
                foreach (var m in item.Attributes)
                    result |= ProcessAttribute(m);
            }

            foreach (var m in item.ChildNodes)
                result |= ProcessElement(m);

            return result;
        }
    }
}
