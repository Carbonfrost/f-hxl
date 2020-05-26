//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

using System.Linq;
using System.Collections.Generic;

using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class DomConverter {
        // Used by the PreparedDocumentBuilder to do most of the work of converting
        // HTML semantics to HXL semantcs

        private readonly HxlServices _services;
        private readonly DomElementConverter Element = DomElementConverter.Compose(
            DomElementConverter.TypeBinding,
            DomElementConverter.ServerAttributes,
            DomElementConverter.Server
        );

        private HxlServices Services {
            get {
                return _services;
            }
        }

        private IDomNodeFactory NodeFactory {
            get {
                return Services.NodeFactory;
            }
        }

        public DomConverter(HxlServices services) {
            _services = services;
        }

        private HxlDocumentFragment NewDocument() {
            var factory = new HxlProviderFactory(NodeFactory);
            return (HxlDocumentFragment) new HxlDocument(factory).CreateDocumentFragment();
        }

        public HxlDocumentFragment Convert(DomContainer html) {
            var result = NewDocument();
            result.Append(ConvertAll(html.ChildNodes));
            return result;
        }

        public DomNode Convert(DomNode node) {
            switch (node.NodeType) {
                case DomNodeType.Element:
                    return ConvertElement((DomElement) node);

                case DomNodeType.ProcessingInstruction:
                    return ConvertProcessingInstruction((DomProcessingInstruction) node);

                case DomNodeType.Text:
                    // TODO Could be noop if entity ref nodes were processed (design, perf)
                    // Right now, inlining is the cleanest way to handle entities correctly
                    return ConvertText((DomText) node);

                case DomNodeType.Comment:
                case DomNodeType.CDataSection:
                case DomNodeType.DocumentType:
                default:
                    return node;
            }
        }

        internal IReadOnlyList<DomNode> ConvertAll(IEnumerable<DomNode> nn) {
            var nodes = nn.ToList();
            var result = new DomNode[nodes.Count];
            int index = 0;
            foreach (var node in nodes) {
                result[index] = Convert(node);
                index++;
            }
            return result;
        }

    // FIXME Rename
        internal DomElement KeepButConvertChildren(DomElement element) {
            // var result = NodeFactory.CreateElement(element.NodeName);
            // Activation.Initialize(
            //     result,
            //     Properties.FromValue(element)
            // );

            // // Attributes can be moved over to save from copying then
            // foreach (var attr in element.Attributes.ToArray()) {
            //     result.Append(attr);
            // }

            // element.Append(
            //     ConvertAll(element.ChildNodes)
            // );
            // return result;
            var children = ConvertAll(element.ChildNodes);
            element.RemoveChildNodes();
            // while (element.ChildNodes.Count > 0) {
            //     if (element.FirstChildNode.ParentNode != null) {
            //         element.FirstChildNode.RemoveSelf();
            //     }
            // }
            element.Append(children);
            return element;
        }

        private DomProcessingInstruction ConvertProcessingInstruction(DomProcessingInstruction node) {
            var macro = NodeFactory.CreateProcessingInstruction(node.Target);
            macro.Data = node.Data;

            // TODO Missing line numbers and positions
            // TODO Enforce directives only at document level
            int line = -1;
            int pos = -1;

            if (macro != null
                && (macro.Target == "xml" || (macro is HxlProcessingInstruction))) {
                Services.ReferencePath.AddImplicitTypeUse(macro.GetType());

            } else {
                throw HxlFailure.DirectiveNotDefined(node.Target, line, pos);
            }
            return macro;
        }

        private DomText ConvertText(DomText node) {
            var result = NodeFactory.CreateText();
            result.Data = node.Data;
            return result;
        }

        private DomElement ConvertElement(DomElement node) {
            var conv = Element;

            var child = conv.Convert(this, node, Services);
            return child;
        }
    }
}
