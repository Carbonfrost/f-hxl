//
// - HxlIterationElementBase.cs -
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
using System.Linq;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public abstract class HxlIterationElementBase : HxlLangElement {

        internal HxlIterationElementBase(string name) : base(name) {}

        // TODO Probably need validation on names of vars

        [Variable]
        public string Var {
            get {
                return Attribute("var");
            }
            set {
                Attribute("var", value);
            }
        }

        public string VarLoopStatus {
            get {
                return Attribute("varLoopStatus");
            }
            set {
                Attribute("varLoopStatus", value);
            }
        }

    }

    interface IHxlIterationElement {
        string Var { get; set; }
        string VarLoopStatus { get; set; }
    }
}
