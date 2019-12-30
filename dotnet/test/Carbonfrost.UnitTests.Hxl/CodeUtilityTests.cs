//
// - CodeUtilityTests.cs -
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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl {

    public class CodeUtilityTests {

        [Fact]
        public void Slug_should_escape_leading_digits() {
            Assert.Equal("_404_hxl", CodeUtility.Slug("404.hxl"));
        }

        [Fact]
        public void EmitInstantiation_should_generate() {
            var manager = new ExpressionSerializationManager();
            var output = new StringWriter();
            string name;
            using (manager.CreateSession()) {
                name = CodeUtility.EmitInstantiation(manager, output, new A());
            }

            // TODO Empty statement and extra ws are generated
            Assert.Equal("a1", name);
            Assert.Equal(
@"Carbonfrost.UnitTests.Hxl.A a1;
Carbonfrost.UnitTests.Hxl.B b1;
a1 = new Carbonfrost.UnitTests.Hxl.A();
a1.B = b1 = new Carbonfrost.UnitTests.Hxl.B();
b1.C = 420;
b1.D = true;
;

", output.ToString());
        }
    }

    public class A {
        public B B { get; set; }

        public A() {
            this.B = new B();
        }
    }

    public class B {

        public int C { get; set; }
        public bool D { get; set; }

        public B() {
            C = 420;
            D = true;
        }
    }
}
