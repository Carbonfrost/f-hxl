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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    abstract partial class DomElementConverter {

        // Coalesces server attributes into a single server attribute
        // <div c:a:a="" c:a:b="" c:a:c="" />
        // -->
        // <div c:a={new CAAttribute { A = "", B = "", C = "" } } />
        public static readonly DomElementConverter ServerAttributes = new ServerAttributesDomElementConverter();

        class DomAttributeWithInitializers {
            public DomAttribute Attribute;
            public readonly IDictionary<string, object> Initializers = new Dictionary<string, object>();
        }

        private HxlQualifiedNameHelper ResolveName(string name) {
            return HxlQualifiedNameHelper.Parse(name);
        }

        class ServerAttributesDomElementConverter : DomElementConverter {

            public override DomElement Convert(DomConverter parent, DomElement element, HxlServices services) {
                if (element.Attributes.Count == 0) {
                    return parent.KeepButConvertChildren(element);
                }
                if (!element.Attributes.Any(a => a.Name.Contains(":"))) {
                    return parent.KeepButConvertChildren(element);
                }
                var factory = services.NodeFactory;
                // var result = factory.CreateElement(element.NodeName);
                var result = parent.KeepButConvertChildren(element);

                // if (result == null) {
                //     throw HxlFailure.ServerElementCannotBeCreated(element.NodeName, -1, -1);
                // }
                // result.Append(
                //     parent.ConvertAll(element.ChildNodes)
                // );

                var serverAttributes = new Dictionary<string, DomAttributeWithInitializers>();
                services.ReferencePath.AddImplicitTypeUse(result.GetType());

                var attrs = new List<DomAttribute>(element.Attributes);
                element.RemoveAttributes();
                foreach (var m in attrs) {
                    if (m.Name.Contains(":")) {
                        // attrs.Add(m);
                        var hxl = ResolveName(m.Name);
                        string id = hxl.Name;

                        DomAttributeWithInitializers withInit;

                        // Coalesce attribute extension syntax
                        if (!serverAttributes.TryGetValue(id, out withInit)) {
                            withInit = CreateServerDomAttribute(factory, m, hxl.Property);
                            serverAttributes.Add(id, withInit);
                        }

                        if (hxl.Property == null) {
                            InitProperty(null, m.Value, withInit);
                        } else {
                            InitProperty(hxl.Property, m.Value, withInit);
                        }

                    } else {
                        result.Attributes.Add(m);
                        // result.Attributes.Add(
                        //     CreateDomAttribute(factory, m)
                        // );
                    }
                }

                foreach (var m in attrs) {
                    m.RemoveSelf();
                }

                foreach (var m in serverAttributes.Values) {
                    Activation.Initialize(m.Attribute, m.Initializers);
                    result.Attributes.Add(m.Attribute);
                    services.ReferencePath.AddImplicitTypeUse(m.Attribute.GetType());
                }
                return result;
            }
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

                if (prop == null) {
                    throw HxlFailure.ServerAttributePropertyNotFound(attr.GetType(), property, -1, -1);
                }
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

        private DomAttribute CreateDomAttribute(IDomNodeFactory factory, DomAttribute m) {
            DomAttribute attr = factory.CreateAttribute(m.Name);
            if (attr == null) {
                throw new NotImplementedException();
            }
            attr.Value = m.Value;
            return attr;
        }

        private DomAttributeWithInitializers CreateServerDomAttribute(IDomNodeFactory factory, DomAttribute m, string property) {
            var result = new DomAttributeWithInitializers();

            try {
                result.Attribute = factory.CreateAttribute(m.Name);

            } catch (Exception ex) {
                if (Failure.IsCriticalException(ex)) {
                    throw;
                } else {
                    throw HxlFailure.CannotCreateAttributeOnConversion(m.Name, ex);
                }
            }

            if (result.Attribute == null) {
                throw HxlFailure.ServerAttributeCannotBeCreated(m.Name, -1, -1);
            }
            return result;
        }
    }
}
