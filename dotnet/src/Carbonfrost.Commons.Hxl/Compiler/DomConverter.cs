//
// - DomConverter.cs -
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
using System.ComponentModel;
using System.Linq;

using System.Reflection;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Html;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class DomConverter : HtmlNodeVisitor {

        private HxlDocument _result;
        private DomContainer _current;
        private Action<Type> _typeUse;

        public HxlDocument Convert(HtmlDocument html,
                                   HxlDocument result,
                                   Action<Type> typeUse) {
            _result = result;
            _current = _result;
            _typeUse = typeUse;
            Visit(html);
            return _result;
        }

        public override void VisitProcessingInstruction(HtmlProcessingInstruction node) {
            var macro = _result.CreateProcessingInstruction(node.Target, node.Data);

            // TODO Missing line numbers and positions
            // TODO Enforce directives only at document level
            int line = -1;
            int pos = -1;

            if (macro != null
                && (macro.Target == "xml" || (macro is ProcessingInstructionFragment))) {
                _current.Append(macro);
                AddImplicitType(macro.GetType());

            } else
                throw HxlFailure.DirectiveNotDefined(node.Target, line, pos);
        }

        public override void VisitDocument(HtmlDocument node) {
            // Wrap us in c:root
            var child = _result.CreateElement("c:root");
            AppendChild(child);
            VisitRange(node.ChildNodes);
        }

        public override void VisitDocumentType(HtmlDocumentType node) {
            var docType = _result.CreateDocumentType(node.Name, node.PublicId, node.SystemId);
            _current.Append(docType);
        }

        public override void VisitText(HtmlText node) {
            // Text isn't allowed at the document level
            if (_current.NodeType == DomNodeType.Document) {
                var macro = new HxlTextElement {
                    Data = Utility.EscapeHtml(node.Text)
                };
                _current.Append(macro);
                return;
            }

            _current.Append(_result.CreateText(node.Text));
            base.VisitText(node);
        }

        public override void VisitElement(HtmlElement node) {
            var child = _result.CreateElement(node.NodeName);
            if (child == null) {
                throw HxlFailure.ServerElementCannotBeCreated(node.NodeName, -1, -1);
            }

            var oldCurrent = _current;
            AppendChild(child);

            // Consolidate server attributes
            var serverAttributes
                = new Dictionary<string, DomAttributeWithInitializers>();

            AddImplicitType(child.GetType());

            foreach (var m in node.Attributes) {
                if (m.Name.Contains(":")) {
                    var hxl = ResolveName(m.Name);
                    string id = hxl.Name;

                    DomAttributeWithInitializers withInit;

                    // Coalesce attribute extension syntax
                    if (!serverAttributes.TryGetValue(id, out withInit)) {
                        withInit = CreateServerDomAttribute(m, hxl.Property);
                        serverAttributes.Add(id, withInit);
                    }

                    if (hxl.Property == null) {
                        InitProperty(null, m.Value, withInit);
                    } else {
                        InitProperty(hxl.Property, m.Value, withInit);
                    }

                } else {
                    CreateDomAttribute(m);
                }
            }

            foreach (var m in serverAttributes.Values)
                Activation.Initialize(m.Attribute, m.Initializers);

            VisitRange(node.ChildNodes);

            _current = oldCurrent;
        }

        void VisitRange(IEnumerable<HtmlNode> childNodes) {
            foreach (var m in childNodes) {
                Visit(m);
            }
        }

        private DomAttribute CreateDomAttribute(HtmlAttribute m) {
            DomAttribute attr = _result.CreateAttribute(m.Name, m.Value);
            if (attr == null)
                throw new NotImplementedException();

            AddImplicitType(attr.GetType());
            _current.Attributes.Add(attr);
            return attr;
        }

        private DomAttributeWithInitializers CreateServerDomAttribute(HtmlAttribute m, string property) {
            var result = new DomAttributeWithInitializers();

            try {
                result.Attribute = _result.CreateAttribute(m.Name);

            } catch (Exception ex) {
                if (Failure.IsCriticalException(ex))
                    throw;
                else
                    throw HxlFailure.CannotCreateAttributeOnConversion(m.Name, ex);
            }

            if (result.Attribute == null)
                throw HxlFailure.ServerAttributeCannotBeCreated(m.Name, -1, -1);

            _current.Attributes.Add(result.Attribute);
            AddImplicitType(result.Attribute.GetType());

            return result;
        }

        private void InitProperty(string property, string value, DomAttributeWithInitializers withInit) {
            PropertyInfo prop;
            DomAttribute attr = withInit.Attribute;

            if (property == null) {
                var valueProp = HxlAttributeFragmentDefinition.ForComponent(attr).ValueProperty;
                if (valueProp == null) {
                    valueProp = Utility.ReflectGetProperty(attr.GetType(), "Value");
                }
                // TODO Might not have a value property (should implement an expression around the Value property)
                prop = valueProp;

            } else {
                // TODO Obtain line numbers
                prop = Utility.ReflectGetProperty(attr.GetType(), property);

                if (prop == null)
                    throw HxlFailure.ServerAttributePropertyNotFound(attr.GetType(), property, -1, -1);
            }

            if (!HxlAttributeConverter.IsExpr(value)) {
                if (property == null) {
                    withInit.Attribute.Value = value;
                }

                withInit.Initializers.Add(prop.Name, value);
                return;
            }

            var buffer = new ExpressionBuffer();
            RewriteExpressionSyntax.MatchVariablesAndEmit(buffer, value);

            // TODO Could allow multiple exprs
            if (buffer.Parts.Count == 1) {
            } else
                throw new NotImplementedException("ex");

            attr.AddAnnotation(new ExpressionInitializers(prop, buffer.Parts[0]));
        }

        private HxlQualifiedNameHelper ResolveName(string name) {
            return HxlQualifiedNameHelper.Parse(name);
        }

        private void AddImplicitType(Type type) {
            _typeUse(type);
        }

        private void AppendChild(DomContainer child) {
            _current.Append(child);
            _current = child;
        }

        class DomAttributeWithInitializers {

            public DomAttribute Attribute;
            public readonly IDictionary<string, object> Initializers = new Dictionary<string, object>();
        }
    }
}
