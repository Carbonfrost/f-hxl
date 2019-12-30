//
// - TextUtility.cs -
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
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    static class TextUtility {

        public static void OuterText(TextWriter _sb, DomProcessingInstruction instruction) {
            _sb.Write("<?");
            _sb.Write(instruction.Target);
            _sb.Write(" ");
            _sb.Write(instruction.Data);
            _sb.Write("-->");
        }

        public static void OuterText(TextWriter _sb, DomCDataSection section) {
            _sb.Write("<![CDATA[");
            _sb.Write(section.Data);
            _sb.Write("]]>");
        }

        public static void OuterText(TextWriter w, DomDocumentType documentType) {
            DomDocumentType node = documentType;
            w.Write("<!DOCTYPE ");
            w.Write(node.Name);

            if (!string.IsNullOrWhiteSpace(node.PublicId)) {
                w.Write(" PUBLIC \"");
                w.Write(node.PublicId);
                w.Write("\"");
            }
            if (!string.IsNullOrWhiteSpace(node.SystemId)) {
                w.Write(" \"");
                w.Write(node.SystemId);
                w.Write("\"");
            }
            w.Write('>');
        }

    }

}
