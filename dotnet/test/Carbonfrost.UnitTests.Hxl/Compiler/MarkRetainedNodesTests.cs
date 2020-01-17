//
// - MarkRetainedNodesTests.cs -
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
using Carbonfrost.Commons.Html;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class MarkRetainedNodesTests {

        // TODO Should check retaining nodes - probably without parsing

        internal static DomElement Parse(string text) {
            var doc = new DomConverter().Convert(
                HtmlDocument.ParseXml(text, null),
                new HxlDocument(),
                (Type t) => {}
            );

            MarkRetainedNodes.Instance.Preprocess(doc, null);
            return doc.DocumentElement.FirstChild;
        }

        [Fact]
        public void IsRetained_should_not_retain_elements_by_default() {
            string expr = "<article></article>";
            var m = Parse(expr);
            Assert.False(m.IsRetained());
        }

        [Fact]
        public void IsRetained_should_always_retain_head() {
            string expr = "<head></head>";
            var m = Parse(expr);
            Assert.True(m.IsRetained());
        }

        [Fact]
        public void IsRetained_should_retain_server_attribute_fragments() {
            string expr = "<wut c:if=\"true\"></wut>";
            var m = Parse(expr);
            Assert.True(m.IsRetained());
        }
    }
}

