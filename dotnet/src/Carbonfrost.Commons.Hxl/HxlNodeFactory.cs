//
// - HxlNodeFactory.cs -
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
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Hxl.Controls;

namespace Carbonfrost.Commons.Hxl {

    sealed class HxlNodeFactory : HxlDomNodeFactory {

        public override DomElement CreateElement(HxlQualifiedName name) {
            if (name.NamespaceUri == Xmlns.HxlLangUri)
                return LangElement(name);

            if (name.NamespaceUri == Xmlns.HxlUri)
                return ControlsElement(name);

            return null;
        }

        public override DomAttribute CreateAttribute(HxlQualifiedName name) {
            if (name.NamespaceUri == Xmlns.HxlLangUri)
                return Lang(name);

            if (name.NamespaceUri == Xmlns.HxlUri)
                return Controls(name);

            return null;
        }

        public override DomProcessingInstruction CreateProcessingInstruction(string target, string data) {
            var props = HxlDirective.Parse(data);
            var result = CreatePICore(target);

            if (result != null)
                Activation.Initialize(result, props);

            return result;
        }

        private static ProcessingInstructionFragment CreatePICore(string target) {
            switch (target) {
                case "model":
                    return new HxlModelDirective();

                case "template":
                    return new HxlTemplateDirective();

                case "using":
                    return new HxlUsingDirective();
            }

            return null;
        }

        static DomElement LangElement(HxlQualifiedName name) {
            switch (name.Name) {
                case "comment":
                    return new HxlCommentElement();

                case "if":
                    return new HxlIfElement();

                case "unless":
                    return new HxlUnlessElement();

                case "foreach":
                    return new HxlForEachElement();

                case "for":
                    return new HxlForElement();

                case "text":
                    return new HxlTextElement();

                case "value":
                    return new HxlValueElement();

                case "model":
                    return new HxlModelElement();

                case "when":
                    return new HxlWhenElement();

                case "case":
                    return new HxlCaseElement();

                case "otherwise":
                    return new HxlOtherwiseElement();

                case "root":
                    return new HxlRootElement();
            }

            // TODO c:while, c:until
            return null;
        }

        static DomElement ControlsElement(HxlQualifiedName name) {
            switch (name.Name) {
                case "body":
                    return new HxlBodyElement();
            }

            return null;
        }

        // TODO Implement an override version of GetAttributeNodeType (performance)

        static AttributeFragment Controls(HxlQualifiedName name) {
            switch (name.Name) {
                case "layout":
                    return new HxlLayoutAttribute();

                case "placeholder":
                    return new HxlPlaceholderAttribute();

                case "placeholdertarget":
                    return new HxlPlaceholderTargetAttribute();

                case "feature":
                    return new HxlFeatureAttribute();

                case "features":
                    return new HxlFeaturesAttribute();

                case "body":
                    return new HxlBodyAttribute();

                default:
                    return HtmlControls(name);
            }
        }

        static AttributeFragment HtmlControls(HxlQualifiedName name) {
            switch (name.Name) {
                case "class":
                    return new HxlClassAttribute();

                case "charset":
                    return new HxlCharSetAttribute();
            }
            return null;
        }

        static AttributeFragment Lang(HxlQualifiedName name) {
            switch (name.Name) {
                case "t":
                    return new HxlTAttribute();

                case "if":
                    return new HxlIfAttribute();

                case "render":
                    return new HxlRenderAttribute();

                case "unless":
                    return new HxlUnlessAttribute();

                case "foreach":
                    return new HxlForEachAttribute();

                case "for":
                    return new HxlForAttribute();

                case "runat":
                    return new HxlRunAtAttribute();

                case "text":
                    return new HxlTextAttribute();

                case "model":
                    return new HxlModelAttribute();

                case "when":
                    return new HxlWhenAttribute();

                case "case":
                    return new HxlCaseAttribute();

                case "otherwise":
                    return new HxlOtherwiseAttribute();

                case "space":
                    return new HxlSpaceAttribute();
            }

            // TODO c:while, c:until, c:case/c:when
            return null;
        }
    }

}

