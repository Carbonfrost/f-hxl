//
// - Prototypes.cs -
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
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl;

[assembly: XmlnsAttribute("http://example.com/", Prefix = "test")]

namespace Carbonfrost.UnitTests.Hxl {

    [HxlElementUsage]
    public class ExampleElement : HxlElement {

        [Value]
        public string A { get; set; }

        [Variable]
        public Double B { get; set; }

        public override void Render() {
            this.Output.Write("example: {0} {1}", A, B);
            RenderBody();
        }

        public override string ToString() {
            return $"Example A={A} B={B}";
        }
    }

    [HxlAttributeUsage]
    public class ExampleAttribute : HxlAttribute {

        public string A { get; set; }

        public Double B { get; set; }

        [Value]
        public char C { get; set; }

        protected override IHxlElementTemplate OnElementRendering() {
            var txt = this.OwnerDocument.CreateText(string.Format("example: {0} {1} {2}", A, B, C));
            this.OwnerElement.ChildNodes.Insert(0, txt);

            return HxlElementTemplate.Default;
        }
    }

    [HxlAttributeUsage]
    public class MyAttributeFragment : HxlAttribute {

        protected override IHxlElementTemplate OnElementRendering() {
            this.OwnerElement.AppendText(Value.ToUpper());
            return null;
        }
    }

    [HxlAttributeUsage]
    public class MyDerivedAttributeFragment : MyAttributeFragment {

        [Value]
        public TimeZoneInfo A { get; set; }

        protected override IHxlElementTemplate OnElementRendering() {
            this.OwnerElement.AppendText(Convert.ToString(A));
            return null;
        }
    }

}
