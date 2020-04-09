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

using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl {

    public class HxlPlaceholderContentProviderTests {

        [Fact]
        public void MergeAttributes_should_copy_attributes_from_operand_nominal() {
            var doc = new HxlDocument();
            var from = doc.CreateElement("from").Attribute("h", "a");
            var to = doc.CreateElement("to");

            HxlPlaceholderContentProvider.MergeAttributes(from, to);
            Assert.Equal("<to h=\"a\"/>", to.OuterXml);
        }

        [Fact]
        public void MergeAttributes_should_replace_attributes_from_operand() {
            var doc = new HxlDocument();
            var from = doc.CreateElement("from").Attribute("h", "a");
            var to = doc.CreateElement("to").Attribute("h", "existing");

            HxlPlaceholderContentProvider.MergeAttributes(from, to);
            Assert.Equal("<to h=\"a\"/>", to.OuterXml);
        }

        [Fact]
        public void MergeAttributes_should_append_to_class_attribute() {
            var doc = new HxlDocument();
            var from = doc.CreateElement("from").Attribute("class", "a");
            var to = doc.CreateElement("to").Attribute("class", "existing");

            HxlPlaceholderContentProvider.MergeAttributes(from, to);
            Assert.Equal("<to class=\"existing a\"/>", to.OuterXml);
        }
    }

}
