//
// - LoopStatus.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Hxl {

    public class LoopStatus {

        public int Count { get; set; }

        public dynamic Current { get; set; }

        public int Index { get; set; }

        public bool IsEven {
            get {
                return (0 == (this.Index % 2));
            }
        }

        public bool IsFirst {
            get {
                return (this.Index == 0);
            }
        }

        public bool IsLast {
            get {
                return (this.Index == (this.Count - 1));
            }
        }

        public bool IsOdd {
            get {
                return !this.IsEven;
            }
        }

        public int Position {
            get { return (this.Index + 1); } }
    }

}
