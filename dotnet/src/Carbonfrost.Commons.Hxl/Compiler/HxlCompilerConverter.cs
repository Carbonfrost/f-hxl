//
// - HxlCompilerConverter.cs -
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
using System.Text;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    abstract class HxlCompilerConverter {
        // Rewrites the DOM tree to contain working compiler nodes and simplifications.

        public static readonly HxlCompilerConverter Noop = new NoopImpl();
        public static readonly HxlCompilerConverter Inline = new InlineNodeConverter();

        public abstract DomObject Convert(DomObject node, IScriptGenerator gen);

        public void ConvertAndAppend(DomNode parent, DomObject m, IScriptGenerator gen) {
            var child = Convert(m, gen);

            if (child == null)
                return;

            if (object.ReferenceEquals(child, m)) {
                parent.Append(child);

            } else {
                parent.Append(child);
            }
        }

        internal static HxlCompilerConverter ChooseConverter(DomObject node) {
            switch (node.NodeType) {
                case DomNodeType.Element:
                    return HxlElementConverter.GetElementConverter((DomElement) node);

                case DomNodeType.Attribute:
                    return HxlAttributeConverter.GetAttributeConverter((DomAttribute) node);

                case DomNodeType.ProcessingInstruction:
                    return HxlProcessingInstructionConverter.GetProcessingInstructionConverter((DomProcessingInstruction) node);

                case DomNodeType.Text:
                    // TODO Could be noop if entity ref nodes were processed (design, perf)
                    // Right now, inlining is the cleanest way to handle entities correctly
                    return Inline;

                case DomNodeType.DocumentType:
                default:
                    return Noop;
            }
        }

        internal static DomNode InlineOuterText(DomDocument document, DomObject element) {
            var result = (HxlTextElement) document.CreateElement("c:text");
            result.Data = new OuterTextVisitor().ConvertToString(element);

            // And make into a render work fragment
            // TODO Use the correct script generator

            // Single spaces are common - don't cause a render island to
            // be created for them
            if (result.Data == " ")
                return result;
            else
                return result.ToIsland(CSharpScriptGenerator.Instance);
        }

        private class InlineNodeConverter : HxlCompilerConverter {
            // - Element is NOT an ElementFragment
            // - Element contains no AttributeFragments
            // - Element has no children OR has descendents which are all marked as inlining
            public override DomObject Convert(DomObject node, IScriptGenerator gen) {
                return InlineOuterText(node.OwnerDocument, node);
            }
        }

        private class OuterTextVisitor : DomNodeVisitor {

            private readonly StringWriter _sb = new StringWriter();

            public string ConvertToString(DomObject node) {
                Visit(node);
                return _sb.ToString();
            }

            private void Visit(IEnumerable<DomNode> nodes, string between) {
                bool comma = false;
                foreach (DomNode node in nodes) {
                    if (comma)
                        _sb.Write(between);

                    Visit(node);
                    comma = true;
                }
            }

            protected override void VisitAttribute(DomAttribute attribute) {
                _sb.Write(attribute.Name);
                _sb.Write("=");
                _sb.Write("\"");
                _sb.Write(attribute.Value);
                _sb.Write("\"");
            }

            protected override void VisitCDataSection(DomCDataSection section) {
                TextUtility.OuterText(_sb, section);
            }

            protected override void VisitComment(DomComment comment) {
            }

            protected override void VisitDocumentType(DomDocumentType documentType) {
                TextUtility.OuterText(_sb, documentType);
            }

            protected override void VisitElement(DomElement element) {
                ElementTemplate.RenderElementStart(element, _sb);
                this.VisitAll(element.ChildNodes);
                ElementTemplate.RenderElementEnd(element, _sb);
            }

            protected override void VisitNotation(DomNotation notation) {
                this.DefaultVisit(notation);
            }

            protected override void VisitProcessingInstruction(DomProcessingInstruction instruction) {
                TextUtility.OuterText(_sb, instruction);
            }

            protected override void VisitText(DomText text) {
                string data;
                if (Utility.IsData(text)) {
                    data = text.Data;

                } else {
                    data = Utility.EscapeHtml(text.Data);
                }

                _sb.Write(data);

            }
        }

        class NoopImpl : HxlCompilerConverter {

            public override DomObject Convert(DomObject node, IScriptGenerator gen) {
                return node;
            }
        }

    }

}
