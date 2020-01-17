//
// - HxlNodeFactoryTests.cs -
//
// Copyright 2014 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Hxl.Controls;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl {

    public class HxlNodeFactoryTests {

        static readonly IDictionary<string, Type> ATTRIBUTES = new Dictionary<string, Type> {
            { "runat", typeof(HxlRunAtAttribute) },
            { "text", typeof(HxlTextAttribute) },
            { "if", typeof(HxlIfAttribute) },
            { "unless", typeof(HxlUnlessAttribute) },
            { "for", typeof(HxlForAttribute) },
            { "case", typeof(HxlCaseAttribute) },
            { "when", typeof(HxlWhenAttribute) },
            { "otherwise", typeof(HxlOtherwiseAttribute) },
            { "render", typeof(HxlRenderAttribute) },
            { "t", typeof(HxlTAttribute) },
        };

        static readonly IDictionary<string, Type> HTML_ATTRIBUTES = new Dictionary<string, Type> {
            { "charset", typeof(HxlCharSetAttribute) },
            { "class", typeof(HxlClassAttribute) },
        };

        static readonly IDictionary<string, Type> ELEMENTS = new Dictionary<string, Type> {
            { "text", typeof(HxlTextElement) },
            { "value", typeof(HxlValueElement) },
            { "if", typeof(HxlIfElement) },
            { "unless", typeof(HxlUnlessElement) },
            { "foreach", typeof(HxlForEachElement) },
            { "for", typeof(HxlForElement) },
            { "comment", typeof(HxlCommentElement) },
        };

        static readonly IDictionary<string, Type> DIRECTIVES = new Dictionary<string, Type> {
            { "model", typeof(HxlModelDirective) },
            { "template", typeof(HxlTemplateDirective) },
            { "using", typeof(HxlUsingDirective) },
        };

        [Fact]
        public void test_required_factory_implementations() {
            var fac = new HxlNodeFactory();

            foreach (var kvp in ELEMENTS) {
                HxlQualifiedName name = new HxlQualifiedName("c", new[] {
                                                                 kvp.Key
                                                             }, null, Xmlns.HxlLangUri);
                var element = fac.CreateElement(name);
                Assert.IsInstanceOf(kvp.Value, element);
            }
        }

        [Fact]
        public void test_required_factory_implementations_attributes() {
            var fac = new HxlNodeFactory();

            foreach (var kvp in ATTRIBUTES) {
                HxlQualifiedName name = new HxlQualifiedName("c", new[] {
                                                                 kvp.Key
                                                             }, null, Xmlns.HxlLangUri);
                var element = fac.CreateAttribute(name);
                Assert.IsInstanceOf(kvp.Value, element);
            }
        }

        [Fact]
        public void test_required_factory_implementations_directives() {
            var fac = new HxlNodeFactory();

            foreach (var kvp in DIRECTIVES) {
                string name = kvp.Key;
                var element = fac.CreateProcessingInstruction(name);
                Assert.IsInstanceOf(kvp.Value, element);
            }
        }

    }

}


