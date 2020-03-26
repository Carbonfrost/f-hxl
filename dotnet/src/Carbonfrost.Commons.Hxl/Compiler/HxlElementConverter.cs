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
using System.Text;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    abstract class HxlElementConverter : HxlCompilerConverter {

        static readonly HxlElementConverter Server = new ServerElementConverter();
        static readonly HxlElementConverter RewriteIslands = new RewriteIslandsConverter();
        static readonly HxlElementConverter Default = new DefaultElementConverter();

        public static HxlCompilerConverter GetElementConverter(DomElement element) {
            if (element is HxlLangElement)
                return RewriteIslands;

            if (element is HxlRenderWorkElement)
                return Noop;

            if (element is HxlElement)
                return Server;

            // If element contains any server attributes (AttributeFragment), then emit
            // a retained node
            if (element.Attributes.Any(t => t is HxlAttribute))
                return Default;

            return Default;
        }

        public sealed override DomObject Convert(DomObject node, IScriptGenerator gen) {
            return Convert(node.OwnerDocument, (DomElement) node, gen);
        }

        protected abstract DomNode Convert(DomDocument document, DomElement element, IScriptGenerator gen);

        static DomElement ProcessChildren(DomElement result, DomElement input, IScriptGenerator gen) {
            // TODO ToArray() is wasteful (performance)
            foreach (var m in input.ChildNodes.ToArray()) {
                var conv = HxlCompilerConverter.ChooseConverter(m);
                conv.ConvertAndAppend(result, m, gen);
            }

            foreach (var m in input.Attributes.ToArray()) {
                var conv = HxlCompilerConverter.ChooseConverter(m);
                conv.ConvertAndAppend(result, m, gen);
            }
            return result;
        }

        private class DefaultElementConverter : HxlElementConverter {

            // Just process children
            protected override DomNode Convert(DomDocument document, DomElement element, IScriptGenerator gen) {
                var e = document.CreateElement(element.Name);
                // Copy annotations to the new element
                e.AddAnnotations(element.Annotations<object>().Except(HxlAnnotations.Retained));
                return ProcessChildren(e,
                                       element,
                                       gen);
            }
        }

        private class RewriteIslandsConverter : HxlElementConverter {

            protected override DomNode Convert(DomDocument document, DomElement element, IScriptGenerator gen) {
                var lang = ((HxlLangElement)element);
                var result = lang.ToIsland(gen);
                ProcessChildren(result, element, gen);
                lang.RewriteIslandChildren(result);
                return result;
            }

        }

        private class ServerElementConverter : HxlElementConverter {

            // - Element is an ElementFragment
            protected override DomNode Convert(DomDocument document, DomElement element, IScriptGenerator gen) {

                // TODO This will not work if expressions are in non-string types
                var myValues = element.Attributes.Select(t => new KeyValuePair<string, object>(t.Name, t.Value));

                // Locate the property handling inner text
                // TODO Using inner text (but it could contain markup, which would technically require special handling logic)
                // TODO Memoize this lookup (performance)
                foreach (PropertyInfo p in Utility.ReflectGetProperties(element.GetType())) {
                    if (p.IsDefined(typeof(ValueAttribute))) {
                        var kvp = new KeyValuePair<string, object>(p.Name, element.InnerText);
                        myValues = Utility.Cons(kvp, myValues);
                    }
                }

                Activation.Initialize(element, myValues);

                return element;
            }

            public void OnConversionException(string property, object value, Exception exception) {
                // If the value looks like an expression, assume that expression parsing will handle it
                if (System.Convert.ToString(value).Contains("$"))
                    return;
                else
                    throw HxlFailure.FailedToReadServerElement(exception);
            }
        }
    }
}
