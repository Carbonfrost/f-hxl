//
// - ProviderDomNodeFactoryTests.cs -
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
using System.Linq;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class ProviderDomNodeFactoryTests {

        [Fact]
        public void CreateAttribute_should_lookup_provider() {
            var infos = App.DescribeProviders().GetProviderInfos(typeof(HxlAttribute))
                .ToArray();

            var p = new ProviderDomNodeFactory();
            var collection = new HxlNamespaceCollection();
            collection.AddNew("test", new Uri("http://example.com/"));
            ((IHxlDomNodeFactory) p).SetResolver(collection);

            // test: prefix gets picked up within dom creation
            var attr = p.CreateAttribute("test:My");
            Assert.IsInstanceOf<MyAttributeFragment>(attr);
            Assert.Equal("test:my", attr.Name);
        }

    }

}
