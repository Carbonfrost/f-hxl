//
// - CompilerDomOptimization.cs -
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
using System.Linq;
using System.Text;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    abstract class CompilerDomOptimization : ICompilerDomOptimization {

        internal static HxlRenderWorkElement GetConsolidation(IEnumerable<DomNode> nodes,
                                                          IList<string> front,
                                                          IList<string> back,
                                                          HxlRenderWorkElement sentinel) {

            front = front ?? Empty<string>.List;
            back = back ?? Empty<string>.List;

            // <render>
            //  <text /> <text /> <render /> <text/>
            // </render>
            // ==>
            // inline text with Write instructions and push up
            // nested render nodes
            List<string> pre = new List<string>(front);
            List<string> post = new List<string>();
            List<string> current = pre;

            foreach (var m in nodes) {
                if (m.NodeType == DomNodeType.Text) {
                    string sb = CodeUtility.AppendDomText(m.TextContent);
                    current.Add(sb);
                }
                else {
                    var hxlTextElement = m as HxlTextElement;
                    if (hxlTextElement != null) {
                        string sb = CodeUtility.AppendDomText(hxlTextElement.Data);
                        current.Add(sb);

                    } else if (m.NodeType == DomNodeType.Element) {

                        HxlRenderWorkElement mr = (HxlRenderWorkElement) m;
                        current.AddRange(mr.PreLines);

                        // If the element has children, then there can be only
                        // one
                        if (m.ChildNodes.Count > 0)
                            current = post;

                        current.AddRange(mr.PostLines);
                    }
                }
            }

            post.AddRange(back);
            var result = new HxlRenderWorkElement(pre, post);
            if (sentinel != null) {
                sentinel.RemoveSelf();

                foreach (var m in sentinel.ChildNodes.ToArray())
                    result.Append(m);
            }
            return result;
        }

        public abstract bool Optimize(DomNode node);

        internal static bool IsConsolidatable(DomNodeCollection childNodes, out HxlRenderWorkElement singleton) {
            singleton = null;
            if (childNodes.Count == 0)
                return false;

            foreach (var node in childNodes) {
                if (node.NodeType == DomNodeType.Text)
                    continue;

                if (node is HxlTextElement)
                    continue;

                HxlRenderWorkElement render = node as HxlRenderWorkElement;
                if (render != null) {
                    if (render.ChildNodes.Count == 0)
                        continue;

                    if (singleton == null) {
                        singleton = render;
                        continue;
                    }

                    return false;
                }

                return false;
            }

            return true;
        }
    }
}


