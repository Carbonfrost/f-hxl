//
// - HxlModelAttribute.cs -
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
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public sealed class HxlModelAttribute : HxlLangAttribute {

        [Value]
        public TypeReference Model {
            get;
            set;
        }

        internal HxlModelAttribute() : base("c:model")
        {
        }

        internal override void AcceptVisitor(IHxlLanguageVisitor visitor) {
            visitor.Visit(this);
        }

        internal override HxlLangElement ConvertToElement() {
            var e = (HxlModelElement) this.OwnerDocument.CreateElement("c:model");
            e.Model = this.Model;
            return e;
        }
    }
}
