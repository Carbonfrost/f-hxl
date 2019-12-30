//
// - HxlWriter.cs -
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Html;

namespace Carbonfrost.Commons.Hxl {

    public class HxlWriter : DomWriter, ITextOutput {

        readonly TextWriter _writer;
        private readonly HtmlWriterSettings settings = new HtmlWriterSettings();
        private readonly HxlTemplateContext _templateContext;
        private int depth = 1;

        private string IndentBuffer {
            get {
                // TODO Memoize this computation (performance)
                return new string(' ', depth * settings.Indent);
            }
        }

        public HxlTemplateContext TemplateContext {
            get {
                return _templateContext;
            }
        }

        public TextWriter BaseWriter {
            get {
                return _writer;
            }
        }

        public HxlWriter(TextWriter writer, HxlTemplateContext templateContext) {
            if (templateContext == null)
                throw new ArgumentNullException("templateContext");

            _templateContext = templateContext;
            _writer = writer;
        }

        public override void WriteStartElement(string name, string namespaceUri) {
            throw new NotImplementedException();
        }

        public override void WriteStartAttribute(string name, string namespaceUri) {
            throw new NotImplementedException();
        }

        public override void WriteEndAttribute() {
            throw new NotImplementedException();
        }

        public override void WriteValue(string value) {
            throw new NotImplementedException();
        }

        public override void WriteEndDocument() {
            throw new NotImplementedException();
        }

        public override void WriteDocumentType(string name, string publicId, string systemId) {
            throw new NotImplementedException();
        }

        public override void WriteEntityReference(string name) {
            throw new NotImplementedException();
        }

        public override void WriteProcessingInstruction(string target, string data) {
            if (settings.PrettyPrint)
                Indent();

            _writer.Write("<?");
            _writer.Write(target);
            _writer.Write(" ");
            _writer.Write(data);
            _writer.Write("?>");
        }

        public override void WriteNotation() {
            throw new NotImplementedException();
        }

        public override void WriteComment(string data) {
            throw new NotImplementedException();
        }

        public override void WriteCDataSection(string data) {
            _writer.Write("<!CDATA[");
            _writer.Write(data);
            _writer.Write("]]>");
        }

        public override void WriteText(string data) {
            // TODO Missing HTMl ws normalzation and other rules like pretty print
            _writer.Write(data);
        }

        public override void WriteStartDocument() {
        }

        public override void WriteEndElement() {
            throw new NotImplementedException();
        }

        public override void WriteDocument(DomDocument document) {
            if (document == null)
                return;
            Write(document.ChildNodes);
        }

        public override void WriteEntityReference(DomEntityReference reference) {
            if (reference == null)
                return;
            _writer.Write("&");
            _writer.Write(reference.NodeName);
        }

        public override void WriteCDataSection(DomCDataSection section) {
            if (section == null)
                return;

            WriteCDataSection(section.Data);
        }

        public override void WriteDocumentType(DomDocumentType documentType) {
            // Drop document type if we're running a master
            if (this._templateContext.MasterInfo == null)
                TextUtility.OuterText(_writer, documentType);
        }

        public override void WriteElement(DomElement element) {
            var frag = element as ElementFragment;
            if (frag != null) {
                WriteElementFragment(frag);
                return;
            }

            // TODO Missing HTML settings and pretty print semantics
            IElementTemplate template = ElementTemplate.ProcessAttributesForTemplate(element, _templateContext);
            template = template ?? element.GetElementTemplate() ?? ElementTemplate.Default;

            template.Render(element, this);
        }

        public override void WriteProcessingInstruction(DomProcessingInstruction instruction) {
            if (instruction == null)
                return;
            WriteProcessingInstruction(instruction.Target, instruction.Data);
        }

        public override void WriteComment(DomComment comment) {
            if (comment == null)
                return;
            if (settings.PrettyPrint)
                Indent();

            _writer.Write("<!--");
            _writer.Write(comment.Text);
            _writer.Write("-->");
        }

        public override void WriteText(DomText text) {
            if (text == null)
                return;
            WriteText(text.Data);
        }

        // `ITextOutput' implementation
        TextWriter ITextOutput.Output {
            get {
                return BaseWriter;
            }
        }

        public void Write(object value) {
            if (ReferenceEquals(null, value))
                return;

            var str = value as string;
            if (str != null) {
                _writer.Write(str);
                return;
            }

            var d = value as DomObject;
            if (d != null) {
                base.Write(d);
                return;
            }

            if (value is IEnumerable) {

                var items = value as IEnumerable<DomObject>;
                if (items != null) {
                    base.Write(items);
                    return;
                }

                if (value is Array || value.GetType().DeclaringType == typeof(Enumerable)) {
                    foreach (object e in (System.Collections.IEnumerable) value)
                        Write(e);
                    return;

                }
            }

            _writer.Write(value);
        }

        public void WriteLine(object value) {
            Write(value);
            _writer.WriteLine();
        }

        public void Write(string value) {
            _writer.Write(value);
        }

        public void WriteLine(string value) {
            _writer.WriteLine(value);
        }

        public void Write(string format, params object[] args) {
            _writer.Write(format, args);
        }

        public void WriteLine(string format, params object[] args) {
            _writer.WriteLine(format, args);
        }

        // TODO Probably obtain from tempContext

        public HxlWriter StartBufferContent(string name) {
            throw new NotImplementedException();
        }

        public void EndBufferContent(string name) {
            throw new NotImplementedException();
        }

        private void WriteElementFragment(ElementFragment fragment) {
            fragment.Render_(this._templateContext, this);
        }

        private void Indent() {
            _writer.Write("\n");
            _writer.Write(IndentBuffer);
        }

        private string PreferredQuote() {
            return "\"";
        }

    }
}
