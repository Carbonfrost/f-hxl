//
// - ReduceAnyFragmentOptimization.cs -
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class ReduceAnyFragmentOptimization : CompilerDomOptimization {

        public static readonly ReduceAnyFragmentOptimization Instance
            = new ReduceAnyFragmentOptimization();

        private ReduceAnyFragmentOptimization() {}

        public override bool Optimize(DomNode node) {
            TryConsolidate(node);
            return true;
        }

        static void TryConsolidate(DomNode any) {
            HxlRenderWorkElement sentinel;

            if (IsConsolidatable(any.ChildNodes, out sentinel)) {
                var consolidated = GetConsolidation(any.ChildNodes, null, null, sentinel);
                any.Empty();
                any.Append(consolidated);
            }

            if (IsInlinableElement(any)) {
                DomElement e = (DomElement) any;
                InlineElement(e);
            }
        }

        static void InlineElement(DomElement e) {
            StringWriter fore = new StringWriter();
            StringWriter aft = new StringWriter();

            HxlElementTemplate.RenderElementStart(e, fore);
            HxlElementTemplate.RenderElementEnd(e, aft);
            HxlRenderWorkElement frag = (HxlRenderWorkElement) e.ChildNodes[0];
            List<string> pre = new List<string>();
            List<string> post = new List<string>();

            pre.Add(CodeUtility.AppendDomText(fore.ToString()));
            pre.AddRange(frag.PreLines);
            post.AddRange(frag.PostLines);
            post.Add(CodeUtility.AppendDomText(aft.ToString()));
            var consolidated = new HxlRenderWorkElement(pre, post);
            frag.RemoveSelf();

            foreach (var m in frag.ChildNodes.ToArray())
                consolidated.Append(m);

            e.ReplaceWith(consolidated);
        }

        static bool IsInlinableElement(DomNode node) {
            // TODO It is possible that this element is being retained for some other
            // reason (like an attribute fragment)
            // Need to update MarkRetainedNodes to correspond to retaining depending only on work
            // for compiler constructs.

            // TODO There might be annotation types that specifically prevent inlining - this is too broad (performance)
            return node.IsElement
                && !(node is HxlElement) // server element
                && node.ChildNodes.Count == 1
                && !node.Attributes.Any(t => t is HxlAttribute)
                && node.ChildNodes[0] is HxlRenderWorkElement
                && !node.Annotations<object>().Any();
        }
    }
}
