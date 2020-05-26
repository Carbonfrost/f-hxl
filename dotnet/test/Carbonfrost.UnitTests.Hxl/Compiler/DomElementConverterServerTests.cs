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
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class DomElementConverterServerTests {

        private readonly IHxlCompilerReferencePath ReferencePath = new HxlCompilerReferencePath();

        internal DomElementConverter Subject {
            get {
                return DomElementConverter.Server;
            }
        }

        private IDomNodeFactory NodeFactory {
            get {
                return new DomNodeFactory(new FakeDomNodeTypeProvider());
            }
        }

        private DomConverter Parent {
            get {
                return new DomConverter(Services);
            }
        }

        private HxlServices Services {
            get {
                var sc = new ServiceContainer();
                sc.AddService(typeof(IDomNodeFactory), NodeFactory);
                sc.AddService(typeof(IHxlCompilerReferencePath), ReferencePath);
                return new HxlServices(sc);
            }
        }

        public class PServerElement : HxlElement {
            public string A { get; set; }
            public int B { get; set; }
            public string[] C { get; set; }

            public PServerElement() : base("e:c") {}
            public PServerElement(string name) : base(name) {}
            public override void Render() {
            }
        }

        class FakeDomNodeTypeProvider : DomNodeTypeProvider {
            public override Type GetElementNodeType(string name) {
                if (name == "e:c") {
                    return typeof(PServerElement);
                }
                return base.GetElementNodeType(name);
            }
        }

        // FIXME Move to TypeBinding

        // [Fact]
        // public void Convert_assigns_string_properties_to_server_element() {
        //     var element = new DomDocument().AppendElement("e:c");
        //     element.Attribute("A", "hello");

        //     var result = (PServerElement) Subject.Convert(Parent, element, Services);
        //     Assert.Equal("hello", result.A);
        // }

        // [Fact]
        // public void Convert_assigns_simple_properties_to_server_element() {
        //     var element = new DomDocument().AppendElement("e:c");
        //     element.Attribute("B", "500");
        //     var result = (PServerElement) Subject.Convert(Parent, element, Services);
        //     Assert.Equal(500, result.B);
        // }

        // [Fact]
        // public void Convert_implicitly_depends_on_server_element_type() {
        //     var element = new DomDocument().AppendElement("e:c");
        //     Subject.Convert(Parent, element, Services);
        //     Assert.Contains(typeof(PServerElement), ReferencePath.ImplicitTypeUses);
        //     Assert.Contains(typeof(PServerElement).Assembly, ReferencePath.ImplicitAssemblyReferences);
        // }

        // [Fact]
        // public void Convert_implicitly_depends_on_server_element_nested() {
        //     var element = new DomDocument().AppendElement("div");
        //     element.AppendElement("e:c");

        //     Subject.Convert(Parent, element, Services);
        //     Assert.Contains(typeof(PServerElement), ReferencePath.ImplicitTypeUses);
        //     Assert.Contains(typeof(PServerElement).Assembly, ReferencePath.ImplicitAssemblyReferences);
        // }
    }
}
