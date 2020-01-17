//
// - RewriteExpressionSyntaxTests.cs -
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
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class RewriteExpressionSyntaxTests {

        [Fact]
        public void test_match_nominal() {
            string expr = "Hello, ${planet}";
            var m = RewriteExpressionSyntax.MatchVariables(expr);
            Assert.Equal(1, m.Count);
            Assert.Equal("${planet}", m[0].Value);
        }

        [Fact]
        public void test_match_ignores_double_dollar() {
            string expr = "Dollar $$ bills ya'll";
            var m = RewriteExpressionSyntax.MatchVariables(expr);
            Assert.Equal(0, m.Count);
        }

        // TODO Is it an error to use just $ ?
        // TODO Test invalid formats $234, for example

    }
}
