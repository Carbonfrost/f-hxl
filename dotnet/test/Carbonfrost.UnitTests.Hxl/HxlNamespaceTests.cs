//
// - HxlNamespaceTests.cs -
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
using System.Reflection;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl {

	public class HxlNamespaceTests {

		[Fact]
		public void GetDefaultPrefix_should_lookup_default_prefix() {
			var dt = typeof(HxlNamespaceTests);
			var prefix = HxlNamespace.GetDefaultPrefix(dt.GetQualifiedName().Namespace, dt.GetTypeInfo().Assembly);
			Assert.Equal("test", prefix);
		}

		[Fact]
		public void ToString_should_format_namespace_names() {
			var n = new HxlNamespace("t", new Uri("http://example.com"));
			Assert.Equal("t => http://example.com/", n.ToString());
		}
	}
}

