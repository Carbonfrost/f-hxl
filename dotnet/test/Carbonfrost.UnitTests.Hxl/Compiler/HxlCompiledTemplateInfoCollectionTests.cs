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
using System.IO;
using System.Reflection;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    // What a generated class using f-hxl-aspnetcore will look like

    [global::System.CodeDom.Compiler.GeneratedCode("HxlCompiler", "0.2.0.0")]
    [global::Carbonfrost.Commons.Hxl.HxlTemplate(Name = "Views/Home/_ApiStats.hxl")]
    public partial class views_home__apistats_hxl : HxlTemplate {
        public override void Transform(TextWriter outputWriter, HxlTemplateContext context) {
            throw new NotImplementedException();
        }
    }

    public class HxlCompiledTemplateInfoCollectionTests {

        public Assembly Assembly {
            get {
                return GetType().Assembly;
            }
        }

        [Fact]
        public void Indexer_can_provide_by_name_case_nominal() {
            var col = new HxlCompiledTemplateInfoCollection(Assembly);
            var actual = col["Views/Home/_ApiStats.hxl"];
            Assert.Equal(typeof(views_home__apistats_hxl), actual.CompiledType);
        }

        [Fact]
        public void Indexer_can_provider_by_name_case_insensitive() {
            var col = new HxlCompiledTemplateInfoCollection(Assembly);
            var actual = col["views/home/_apistats.hxl"];
            Assert.Equal(typeof(views_home__apistats_hxl), actual.CompiledType);
        }
    }
}
