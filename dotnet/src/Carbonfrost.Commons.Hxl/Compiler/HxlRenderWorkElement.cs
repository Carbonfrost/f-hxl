//
// - HxlRenderWorkElement.cs -
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

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class HxlRenderWorkElement : ElementFragment {

        private readonly string[] pre, post;

        public string[] PreLines {
            get {
                return pre;
            }
        }

        public string[] PostLines {
            get {
                return post;
            }
        }

        public HxlRenderWorkElement(IEnumerable<string> pre, IEnumerable<string> post)
            : base("c:render")
        {
            this.pre = pre.ToArray();
            this.post = post.ToArray();
        }

        public void WritePreLines(TextWriter output) {
            foreach (var m in pre)
                output.WriteLine(m);
        }

        public void WritePostLines(TextWriter output) {
            foreach (var m in post)
                output.WriteLine(m);
        }

        public override void Render() {}

    }
}
