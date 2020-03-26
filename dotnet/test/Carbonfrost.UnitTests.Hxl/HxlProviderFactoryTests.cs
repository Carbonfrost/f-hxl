//
// Copyright 2016, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Hxl.Controls;
using Carbonfrost.Commons.Spec;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.UnitTests.Hxl {

    public class HxlProviderFactoryTests {

        private IEnumerable<object> ExampleProviderObjects {
            get {
                yield return HxlElement.Create((d, e) => {});
                yield return HxlAttribute.Create("ok", (d, e) => "");
            }
        }

        [Theory]
        [InlineData(typeof(HxlAttribute))]
        [InlineData(typeof(HxlElement))]
        [InlineData(typeof(HxlProcessingInstruction))]
        [InlineData(typeof(HxlDocument))]
        [InlineData(typeof(HxlDocumentFragment))]
        public void IsProviderObject_should_match_known_types(Type type) {
            Assert.True(new HxlProviderFactory().IsProviderObject(type));
        }

        [Theory]
        [InlineData(typeof(HxlFeatureAttribute))]
        [InlineData(typeof(ExampleElement))]
        public void IsProviderObject_should_match_known_derived_types(Type type) {
            Assert.True(new HxlProviderFactory().IsProviderObject(type));
        }

        [Theory]
        [PropertyData(nameof(ExampleProviderObjects))]
        public void IsProviderObject_should_match_known_derived_types(object provideObj) {
            Assert.True(new HxlProviderFactory().IsProviderObject(provideObj));
        }

        [Fact]
        public void CreateWriter_should_be_HxlWriter() {
            Assert.IsInstanceOf<HxlWriter>(
                new HxlProviderFactory().CreateWriter(TextWriter.Null)
            );
        }

        [Fact]
        public void GetProviders_should_supply_our_provider_type() {
            Assert.Contains(typeof(HxlProviderFactory),
                            App.GetProviders<DomProviderFactory>().Select(t => t.GetType()));
        }

        [Fact]
        public void ForProviderObject_should_supply_correct_type() {
            var element = new ExampleElement();
            var pro = DomProviderFactory.ForProviderObject(element);

            Assert.NotNull(pro);
            Assert.IsInstanceOf<IHxlDomNodeFactory>(pro.CreateNodeFactory(null));
        }
    }
}
