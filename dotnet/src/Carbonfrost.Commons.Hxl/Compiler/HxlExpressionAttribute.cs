//
// - HxlExpressionAttribute.cs -
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
using System.IO;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class HxlExpressionAttribute : HxlAttribute {

        readonly string attrName;
        readonly string code;

        internal HxlExpressionAttribute(string attrName, string code) : base(RandomName()) {
            this.code = code;
            this.attrName = attrName;
        }

        internal void GetInitCode(string variable, IHxlTemplateEmitter context, TextWriter tw) {
            // TODO Possibly better to use other name in this attribute render closure
            // HACK __self__ is a hack
            tw.Write("{3} = global::{0}.Create(\"{2}\", (__closure, __self__) => {1});" + Environment.NewLine, typeof(HxlAttribute).FullName, code, attrName, variable);
        }
    }
}
