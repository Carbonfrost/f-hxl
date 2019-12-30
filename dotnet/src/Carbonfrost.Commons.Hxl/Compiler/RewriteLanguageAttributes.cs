//
// - RewriteLanguageAttributes.cs -
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

    sealed class RewriteLanguageAttributes : HxlCompilerProcessor {

        internal static readonly RewriteLanguageAttributes Instance
            = new RewriteLanguageAttributes();

        public override void Preprocess(DomDocument document, IServiceProvider serviceProvider) {
            Rewrite(document);
        }

        static void Rewrite(DomDocument document) {
            ProcessElement(document);
        }

        static void ProcessElement(DomContainer e) {
            if (e == null)
                return;

            // TODO Copying to an array is wasteful (performance)
            foreach (var c in e.ChildNodes.ToArray()) {
                ProcessElement(c as DomElement);
            }

            // Language attributes can wrap the element --
            // <span c:if /> ==>
            //   <c:if> <span /> </c:if>

            if (e.Attributes == null) {
                return;
            }

            // TODO Correct order in these attributes (AttributeFragmentPriority)
            // TODO copying to array is wasteful (performance)
            var hxl = e.Attributes.OfType<HxlLangAttribute>().ToArray();
            DomElement newElement = null;

            foreach (var m in hxl) {
                newElement = m.ApplyToElement(newElement);
                m.RemoveSelf();
            }

            if (newElement != null) {
                e.ReplaceWith(newElement);
                newElement.Append(e);
            }

        }

    }
}
