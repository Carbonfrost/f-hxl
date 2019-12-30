//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Hxl.Controls;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl {

    public class HxlProviderFactoryTests {

        [Theory]
        [InlineData(typeof(AttributeFragment))]
        [InlineData(typeof(ElementFragment))]
        [InlineData(typeof(ProcessingInstructionFragment))]
        public void IsProviderObject_should_match_known_types(Type type) {
            Assert.True(new HxlProviderFactory().IsProviderObject(type));
        }
    }
}