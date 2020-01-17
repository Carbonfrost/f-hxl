//
// - AttributeFragmentTests.cs -
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
using System.Linq;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class AttributeFragmentTests {

        class B {
            public string C { get { return null;  } }
        }

        [Fact]
        public void ImplicitName_should_remove_affixes() {
            Assert.Equal("my", AttributeFragment.GetImplicitName(typeof(MyAttributeFragment)));
        }

        // TODO Technically, web-dom expects that attributes are never instantiated
        // directly (the dom node factory or the document should be responsible).
        // However, the code generator uses direct construction; it shouldn't (design)

       [Fact]
        public void Constructor_should_guess_default_name() {
            var attr = new MyAttributeFragment();
            // test: prefix is the default, which may not actually be
            // correct
            Assert.Equal("test:my", attr.Name);
        }

        [Fact]
        public void Constructor_should_guess_default_name_derived() {
            var attr = new MyDerivedAttributeFragment();
            // test: prefix is the default, which may not actually be
            // correct
            Assert.Equal("test:myderived", attr.Name);
        }
    }
}
