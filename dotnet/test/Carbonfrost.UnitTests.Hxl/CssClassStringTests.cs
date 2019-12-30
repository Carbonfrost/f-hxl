//
// - CssClassStringTests.cs -
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
using System.Net.Sockets;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl {

    public class CssClassStringTests {

        [Fact]
        public void ToString_should_convert_no_properties_trivial() {
            Assert.Equal("object", new CssClassString(new object()).ToString());
        }

        [Fact]
        public void ToString_should_convert_enum() {
            Assert.Equal("tcp socket-option-level", new CssClassString(SocketOptionLevel.Tcp).ToString());
        }

        [Fact]
        public void ToString_should_convert_enum_flags() {
            Assert.Equal("control-data-truncated broadcast socket-flags",
                         new CssClassString(SocketFlags.Broadcast
                                           | SocketFlags.ControlDataTruncated).ToString());
        }

        [Fact]
        public void ToString_should_convert_composite_object() {
            Assert.Equal("open control-data-truncated broadcast a", new CssClassString(new A()).ToString());
        }

        class A {

            public bool IsOpen {
                get {
                    return true;
                }
            }

            public bool IsClosed {
                get {
                    return false;
                }
            }

            public SocketFlags D {
                get {
                    return SocketFlags.Broadcast
                        | SocketFlags.ControlDataTruncated;
                }
            }
        }

    }
}


