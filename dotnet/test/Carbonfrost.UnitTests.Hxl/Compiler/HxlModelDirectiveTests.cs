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

using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class HxlModelDirectiveTests {

        [XFact(Reason = "no automatic type conversion")]
        public void TextContent_will_initialize_model_type() {
            var d = new HxlModelDirective {
                TextContent = "type=\"System.String\""
            };
            Assert.Equal(TypeReference.Parse("System.String"), d.Type);
        }

        [Fact]
        public void NodeName_is_expected() {
            var d = new HxlModelDirective();
            Assert.Equal("model", d.NodeName);
            Assert.Equal("model", d.Target);
        }
    }
}
