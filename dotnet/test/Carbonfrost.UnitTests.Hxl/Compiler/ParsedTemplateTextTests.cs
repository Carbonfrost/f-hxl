//
// Copyright 2013, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class ParsedTemplateTextTests : ParsedTemplateTestBase {

        [XFact(Reason = "escape handling")]
        public void trivial_text() {
            LoadDefault();
            GenerateAndAssert();
            AssertNoWarnings();
        }

        [Fact]
        public void trivial_html() {
            LoadDefault();
            GenerateAndAssert();
            AssertNoWarnings();
        }

        [Fact]
        public void trivial_element() {
            LoadDefault();
            GenerateAndAssert();
            AssertNoWarnings();
        }

        [Fact]
        public void trivial_ignore_comments() {
            LoadDefault();
            GenerateAndAssert();
            AssertNoWarnings();
        }

        [Fact]
        public void trivial_compiler_comment() {
            LoadDefault();
            GenerateAndAssert();
            AssertNoWarnings();
        }

        [Fact]
        public void expression_simple() {
            LoadDefault();
            GenerateAndAssert();
            AssertNoWarnings();
        }

        [Fact]
        public void expression_namespace_syntax() {
            LoadDefault();
            GenerateAndAssert();
            AssertNoWarnings();
        }

        [Fact]
        public void expression_closure_capture() {
            LoadDefault();
            GenerateAndAssert();
            AssertNoWarnings();
        }

        [Fact]
        public void expression_using_extension_methods() {
            LoadDefault();
            GenerateAndAssert();
            AssertNoWarnings();
        }

        [Fact]
        public void retain_scripts_and_styles() {
            LoadDefault();
            GenerateAndAssert();
            AssertNoWarnings();
        }

        // TODO Support correct encoding of hex entity escapes

        [Fact, Skip]
        public void entity_hex_encoding() {
            LoadDefault();
            GenerateAndAssert();
        }
    }

}
