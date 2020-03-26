//
// Copyright 2014, 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    [Providers]
    public abstract class HxlDomNodeFactory : DomNodeFactory, IHxlDomNodeFactory {

        private IHxlNamespaceResolver _resolver;

        internal static readonly HxlDomNodeFactory Compiler = new HxlCompilerNodeFactory();

        public abstract DomElement CreateElement(HxlQualifiedName name);
        public abstract DomAttribute CreateAttribute(HxlQualifiedName name);

        public sealed override DomAttribute CreateAttribute(string name) {
            if (name == null)
                throw new ArgumentNullException("name");

            if (string.IsNullOrEmpty(name))
                throw Failure.EmptyString("name");

            if (!name.Contains(":"))
                return base.CreateAttribute(name);

            HxlQualifiedNameHelper helper;
            var resolved = ResolveName(name, out helper);
            var node = CreateAttribute(resolved);

            if (node != null) {
                SetVariableProperty(node, helper.Variable);
            }

            return node;
        }

        private static void SetVariableProperty(DomAttribute component, string value) {
            // TODO Validate whether variable is needed
            if (string.IsNullOrEmpty(value))
                return;

            var pd = HxlAttributeFragmentDefinition.ForComponent(component).VariableProperty;
            if (pd == null)
                return;

            if (!string.IsNullOrEmpty(value) && !CodeUtility.IsValidIdentifier(value))
                throw HxlFailure.NotValidVariableName(value);

            // TODO Could have non-string variable prop (uncommon)
            if (pd.PropertyType == typeof(string))
                pd.SetValue(component, value);
            else
                throw new NotImplementedException();
        }

        public override DomDocumentFragment CreateDocumentFragment() {
            return new HxlDocumentFragment();
        }

        public sealed override DomElement CreateElement(string name) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(name))
                throw Failure.EmptyString("name");

            if (!name.Contains(":"))
                return base.CreateElement(name);

            var resolved = ResolveName(name);
            return CreateElement(resolved);
        }

        public virtual Type GetAttributeNodeType(HxlQualifiedName name) {
            var attr = CreateAttribute(name);
            if (attr == null)
                return null;
            else
                return attr.GetType();
        }

        public virtual Type GetElementNodeType(HxlQualifiedName name) {
            var e = CreateElement(name);
            if (e == null)
                return null;
            return e.GetType();
        }

        private HxlQualifiedName ResolveName(string name) {
            HxlQualifiedNameHelper dummy;
            return ResolveName(name, out dummy);
        }

        private HxlQualifiedName ResolveName(string name, out HxlQualifiedNameHelper helper) {
            helper = HxlQualifiedNameHelper.Parse(name);
            Uri ns = null;
            if (!string.IsNullOrEmpty(helper.Prefix)) {
                ns = LookupNamespace(helper.Prefix);
            }
            return helper.ToName(ns);
        }

        protected virtual Uri LookupNamespace(string prefix) {
            return (_resolver ?? HxlNamespaceResolver.Default).LookupNamespace(prefix);
        }

        void IHxlDomNodeFactory.SetResolver(IHxlNamespaceResolver resolver) {
            // TODO This unexpected behavior with the resolver may make it
            // unlikely/impossible that this class could be extended outside of this assembly (design)
            _resolver = resolver;
        }

    }
}
