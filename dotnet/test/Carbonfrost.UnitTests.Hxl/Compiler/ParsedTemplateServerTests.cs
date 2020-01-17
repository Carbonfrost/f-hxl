//
// - ParsedTemplateServerTests.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

    public class ParsedTemplateServerTests : ParsedTemplateTestBase {

        // TODO Test element with inner text value

        [Fact]
        public void server_element_with_expressions() {
            LoadDefault();
            GenerateAndAssert();
        }

        [Fact]
        public void server_element_with_expressions_body() {
            LoadDefault();
            GenerateAndAssert();
        }

        [Fact]
        public void server_attribute_with_expressions() {
            LoadDefault();
            GenerateAndAssert();
        }

        [Fact]
        public void server_attribute_latebound() {
            LoadDefault();
            GenerateAndAssert();
        }

        [Fact]
        public void server_attribute_expression_value() {
            LoadDefault();

            Data.SetProperty("c", TimeZoneInfo.Utc);
            GenerateAndAssert();
        }
    }

}
