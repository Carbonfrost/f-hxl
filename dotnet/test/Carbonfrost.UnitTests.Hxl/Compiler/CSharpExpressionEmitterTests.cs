//
// - CSharpExpressionEmitterTests.cs -
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
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class CSharpExpressionEmitterTests {

        [Fact]
        public void Print_should_handle_arrays() {
            var exp = Expression.Serialize(new byte[] { 4, 5, 8 });
            Assert.Equal("new System.Byte[] { 4, 5, 8, }", ConvertToString(exp));
        }

        private string ConvertToString(Expression e) {
            var sw = new StringWriter();
            var emitter = new CSharpExpressionEmitter(sw);
            emitter.Visit(e);
            return sw.ToString();
        }
    }
}
