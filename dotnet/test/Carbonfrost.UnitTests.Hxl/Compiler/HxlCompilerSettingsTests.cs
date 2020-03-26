//
// Copyright 2014, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using System.Xml;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;
using Microsoft.CSharp.RuntimeBinder;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public class HxlCompilerSettingsTests {

        // TODO This is not comprehensive and is limited by forwarders
        // Uri is needed but won't work in netcore

        [Theory]
        [InlineData(typeof(Activation))]
        [InlineData(typeof(DomElement))]
        [InlineData(typeof(HxlElement))]
        [InlineData(typeof(RuntimeBinderException))]
        [XInlineData(typeof(Enumerable))] // TODO These are forwarded in netcore
        [XInlineData(typeof(XmlWriter))]
        public void Default_should_have_known_HXL_types_referenced_implicitly(Type type) {
            var settings = HxlCompilerSettings.Default;
            var list = settings.Assemblies.Where(t => t.Name.Name == type.GetTypeInfo().Assembly.GetName().Name).ToList();
            Assert.Equal(1, list.Count);
        }
    }
}

