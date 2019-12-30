//
// Copyright 2015, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl {

    public class HxlProviderTests {

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
            Assert.IsInstanceOf<IHxlDomNodeFactory>(pro.NodeFactory);
        }

        [Fact]
        public void IsProviderObject_should_supply_correct_type() {
            var ins = new HxlProviderFactory();
            Assert.True(ins.IsProviderObject(typeof(ExampleElement)));
            Assert.True(ins.IsProviderObject(typeof(ElementFragment)));
        }
    }

}
