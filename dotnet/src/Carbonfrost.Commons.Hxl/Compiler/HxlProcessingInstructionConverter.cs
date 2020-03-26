//
// - HxlProcessingInstructionConverter.cs -
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
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    abstract class HxlProcessingInstructionConverter : HxlCompilerConverter {

        static readonly HxlProcessingInstructionConverter Server = new ServerPIConverter();

        public sealed override DomObject Convert(DomObject node, IScriptGenerator gen) {
            return Convert(node.OwnerDocument, (DomProcessingInstruction) node);
        }

        public static HxlCompilerConverter GetProcessingInstructionConverter(DomProcessingInstruction instruction) {
            if (instruction is HxlProcessingInstruction)
                return Server;
            else
                return Noop;
        }

        protected abstract DomNode Convert(DomDocument document, DomProcessingInstruction instruction);

        class ServerPIConverter : HxlProcessingInstructionConverter {

            protected override DomNode Convert(DomDocument document, DomProcessingInstruction instruction) {
                HxlProcessingInstruction pf = (HxlProcessingInstruction) instruction;
                if (pf.NeedsEmit)
                    return pf;
                else
                    return null;
            }
        }
    }
}
