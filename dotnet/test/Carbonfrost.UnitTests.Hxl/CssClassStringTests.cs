//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

        [Theory]
        [InlineData("p", "tcp")]
        [InlineData("pp", "tcp-socket-option-level")]
        [InlineData("ppp", "tcp-socket-option-level tcp")]
        [InlineData("PP", "socket-option-level-tcp")]
        [InlineData("C", "socket-option-level")]
        [InlineData("CPP", "socket-option-level socket-option-level-tcp")]
        [InlineData("G", "tcp socket-option-level")]
        [InlineData("  G", "tcp socket-option-level", Name = "general ignore ws")]
        [InlineData("PP C", "socket-option-level-tcp socket-option-level", Name = "ignore ws")]
        public void ToString_should_convert_enum_using_format(string format, string expected) {
            Assert.Equal(
                expected,
                new CssClassString(SocketOptionLevel.Tcp).ToString(format)
            );
        }

        [Theory]
        [InlineData("Gp", Name = "can't combine general format")]
        [InlineData("Z")]
        public void ToString_should_throw_on_invalid_format_string(string format) {
            Assert.Throws<FormatException>(
                () => new CssClassString(SocketOptionLevel.Tcp).ToString(format)
            );
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


