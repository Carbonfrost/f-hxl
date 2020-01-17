//
// - ParsedTemplateLanguageTests.cs -
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

    public class ParsedTemplateLanguageTests : ParsedTemplateTestBase {

        [Fact]
        public void language_element_if() {
            LoadDefault();
            GenerateAndAssert();
        }

        [Fact]
        public void language_nested_if_construct() {
            LoadDefault();
            GenerateAndAssert();
        }

        [Fact]
        public void language_case_construct() {
            LoadDefault();
            GenerateAndAssert();
        }

        [Fact]
        public void language_comment_construct() {
            LoadDefault();
            GenerateAndAssert();
        }

        [Fact]
        public void expression_simple_attribute() {
            LoadDefault();
            GenerateAndAssert();
        }

        [Fact, Skip]
        public void language_model_attribute() {
            LoadDefault();
            GenerateAndAssert();
        }

        [Fact] 
        public void language_t_construct() {
            LoadDefault();
            GenerateAndAssert();
        }

    }

}
