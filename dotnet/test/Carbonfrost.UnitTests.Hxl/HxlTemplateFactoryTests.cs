//
// - HxlTemplateFactoryTests.cs -
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
using System.Reflection;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Spec;
using Carbonfrost.UnitTests.Hxl.Compiler;

namespace Carbonfrost.UnitTests.Hxl {

    public class HxlTemplateFactoryTests : ParsedTemplateTestBase {

        [Fact]
        public void create_instance_from_assembly_factory() {
            Load("trivial-content-decorator");
            var f = (ReflectedTemplateFactory)
                HxlTemplateFactory.FromAssembly(this.Assembly);

            Assert.Equal(2, f.TemplateCount);
        }

        [Fact]
        public void system_assembly_implies_null_factory() {
            var mscorlib = HxlTemplateFactory.FromAssembly(typeof(object).GetTypeInfo().Assembly);
            Assert.Same(HxlTemplateFactory.Null, mscorlib);
        }

        [Fact]
        public void CreateTemplate_should_be_case_insensitive() {
            Load("trivial-content-decorator");
            var f = (ReflectedTemplateFactory) HxlTemplateFactory.FromAssembly(Assembly);
            var template = f.CreateTemplate("trivial_content_decorator_fixture_master_hxl", null, null);
            var template2 = f.CreateTemplate("Trivial_Content_decorator_fixture_master_hxl", null, null);
            Assert.NotNull(template);
            Assert.NotNull(template2);
            Assert.Equal(template.GetType(), template2.GetType());
        }
    }
}
