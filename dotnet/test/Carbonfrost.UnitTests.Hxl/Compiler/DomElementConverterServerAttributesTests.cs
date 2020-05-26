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

using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Hxl.Compiler;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class DomElementConverterServerAttributesTests {

        private readonly IDomNodeFactory NodeFactory = new FakeDomNodeFactory();
        private readonly IHxlCompilerReferencePath ReferencePath = new HxlCompilerReferencePath();

        internal DomElementConverter Subject {
            get {
                return DomElementConverter.ServerAttributes;
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

        public class PServerAttribute : HxlAttribute {
            public string A { get; set; }
            public int B { get; set; }
            public string[] C { get; set; }

            public PServerAttribute() : base("e:c") {}
        }

        class FakeDomNodeFactory : DomNodeFactory {
            public override DomAttribute CreateAttribute(string name) {
                if (name.Equals("e:c")) {
                    return new PServerAttribute();
                }
                if (name.StartsWith("e:c")) {
                    return new PServerAttribute();
                }
                return base.CreateAttribute(name);
            }
        }

        [Theory]
        [InlineData("e:c")]
        [InlineData("e:c:a")]
        public void Convert_provides_server_attribute_type(string attr) {
            var element = new DomDocument().AppendElement("div");
            element.Attribute(attr, "hello");

            var actual = Subject.Convert(Parent, element, Services);
            Assert.HasCount(1, actual.Attributes);
            Assert.IsInstanceOf<PServerAttribute>(actual.Attributes[0]);
        }

        [Fact]
        public void Convert_assigns_string_properties_to_server_attribute() {
            var element = new DomDocument().AppendElement("div");
            element.Attribute("e:c:a", "hello");

            var result = (PServerAttribute) Subject.Convert(Parent, element, Services).Attributes[0];
            Assert.Equal("hello", result.A);
        }

        [Fact]
        public void Convert_assigns_simple_properties_to_server_attribute() {
            var element = new DomDocument().AppendElement("div");
            element.Attribute("e:c:b", "500");

            var result = (PServerAttribute) Subject.Convert(Parent, element, Services).Attributes[0];
            Assert.Equal(500, result.B);
        }

        [Fact]
        public void Convert_implicitly_depends_on_attribute_type() {
            var element = new DomDocument().AppendElement("div");
            element.Attribute("e:c:b", "500");

            Subject.Convert(Parent, element, Services);
            Assert.Contains(typeof(PServerAttribute), ReferencePath.ImplicitTypeUses);
            Assert.Contains(typeof(PServerAttribute).Assembly, ReferencePath.ImplicitAssemblyReferences);
        }
    }
}
