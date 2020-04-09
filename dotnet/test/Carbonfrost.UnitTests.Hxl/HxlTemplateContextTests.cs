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
using System.Dynamic;
using System.Linq;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl {

    public class HxlTemplateContextTests {

        [Fact]
        public void GetDynamicMemberNames_should_contain_properties() {
            var context = new HxlTemplateContext(new { a = "", b = "" });
            Assert.SetEqual(new[] { "a", "b", "TemplateFactory", "Data", "DataProviders", "Parent" },
                            context.GetDynamicMemberNames().ToArray());
        }

        [Fact]
        public void TryGetMember_should_obtain_owner_property() {
            var context = new HxlTemplateContext(new { a = "hello", });
            var binder = new FakeGetMemberBinder("a");
            object result;
            Assert.True(context.TryGetMember(binder, out result));
            Assert.Equal("hello", result);
        }

        [Fact]
        public void Dynamic_dispatch_should_evaluate_member_access_from_Data() {
            var tc = new HxlTemplateContext(null);
            tc.Data.Add("model", new { a = "hello" });

            string actual = ((dynamic) tc).model.a;
            Assert.Equal("hello", actual);
        }

        [Fact]
        public void Dynamic_dispatch_should_evaluate_member_access_from_a_data_provider() {
            var tc = new HxlTemplateContext(null);
            tc.DataProviders.AddNew("provider", new { b = "world" });

            string actual = ((dynamic) tc).B;
            Assert.Equal("world", actual);
        }

        class FakeGetMemberBinder : GetMemberBinder {
            public FakeGetMemberBinder(string name)
                : base(name, true) {}

            public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion) {
                throw new NotImplementedException();
            }

        }
    }
}
