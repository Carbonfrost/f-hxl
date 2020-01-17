//
// - RemoveWhitespaceOptimization.cs -
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
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class RemoveWhitespaceOptimization : CompilerDomOptimization {

        static readonly Regex WS = new Regex(@"[ \n\r\t]+");

        public static readonly RemoveWhitespaceOptimization Instance
            = new RemoveWhitespaceOptimization();

        private RemoveWhitespaceOptimization() {}

        public override bool Optimize(DomNode node) {
            if (node.IsElement) {
                ProcessElement((DomElement) node);
            }

            return true;
        }

        void ProcessElement(DomElement e) {
            if (IsWSImportant(e))
                return;

            // TODO Keep scripts and styles as is until compressor can be used
            if (e.NodeName == "style" || e.NodeName =="script")
                return;

            foreach (var child in e.ChildNodes) {
                if (child.IsElement) {
                    ProcessElement((DomElement) child);

                } else if (child.IsText) {
                    var text = (DomText) child;
                    text.Data = CompressWS(text.Data);

                } else {
                    var te = child as HxlTextElement;
                    if (te != null) {
                        te.Data = CompressWS(te.Data);
                    }
                }
            }
        }

        static string CompressWS(string data) {
            return WS.Replace(data, " ").Trim();
        }

        // TODO Enable disabling using c:space=preserve

        static bool IsWSImportant(DomElement e) {
            switch (e.NodeName) {
                case "pre":
                    return true;

                default:
                    return false;
            }
        }

    }
}
