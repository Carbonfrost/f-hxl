//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl {

    public class HxlAssemblyCollectionTests {

        [Fact]
        public void LookupNamespace_should_resolve_assembly_ns_bindings() {
            var c = new HxlAssemblyCollection();
            c.AddNew(typeof(Glob).GetTypeInfo().Assembly.GetName());

            Assert.Equal("https://ns.carbonfrost.com/commons/core", Convert.ToString(c.LookupNamespace("runtime")));
        }

        [Fact]
        public void LookupNamespace_should_resolve_implied_default_bindings() {
            var c = new HxlAssemblyCollection();

            // Because of Core
            Assert.Equal("https://ns.carbonfrost.com/commons/core", Convert.ToString(c.LookupNamespace("runtime")));
        }

        [Fact]
        public void LookupNamespace_should_disable_implied_default_bindings() {
            var c = new HxlAssemblyCollection {
                DisableAutomaticProbing = true,
            };

            // Because of Core
            Assert.Null(c.LookupNamespace("runtime"));
            Assert.Null(c.LookupNamespace("shareable"));
        }

    }

}
