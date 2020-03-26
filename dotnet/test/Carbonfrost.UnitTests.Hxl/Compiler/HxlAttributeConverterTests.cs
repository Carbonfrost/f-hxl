//
// - HxlAttributeConverterTests.cs -
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

using System.IO;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class HxlAttributeConverterTests : HxlNodeConverterTestBase {

        [Fact]
        public void transform_simple_attribute_noop() {
            var doc = new HxlDocument();
            var attr = doc.CreateAttribute("class");
            attr.Value = "no expressions";
            var node = ConvertNode(attr);
            Assert.Same(attr, node);
        }

        [Fact]
        public void transform_simple_attribute_expression_syntax() {
            var doc = new HxlDocument();
            var attr = doc.CreateAttribute("class");
            attr.Value = "no $myExpressions";

            var expected = "myvar = global::Carbonfrost.Commons.Hxl.HxlAttribute.Create(\"class\", (__closure, __self__) => string.Concat((object) \"no \", (__closure.MyExpressions)));" + Environment.NewLine;
            var tw = new StringWriter();
            ((HxlExpressionAttribute) ConvertNode(attr)).GetInitCode("myvar", null, tw); // new IndentedTextWriter(tw, "    "));
            string text = tw.ToString();
            Assert.Equal(expected, text);
        }

        [Theory]
        [InlineData("Hello, $planet", true)]
        [InlineData("Nominal", false)]
        [InlineData("", false)]
        [InlineData("$$", false)]
        [InlineData("$", false)]
        [InlineData("Another $$ expression$value", true)]
        [InlineData("Total $$$grandTotal", true)]
        public void IsExpr_should_match_expression_syntax(string text, bool expected) {
            Assert.Equal(expected, HxlAttributeConverter.IsExpr(text));
        }

    }
}
