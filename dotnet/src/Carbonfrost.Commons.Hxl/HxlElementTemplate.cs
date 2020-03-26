//
// Copyright 2013, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using System.IO;
using System.Linq;

using Carbonfrost.Commons.Html;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public abstract partial class HxlElementTemplate : ITextOutput, IHxlElementTemplate {

        static readonly HtmlWriterSettings settings = new HtmlWriterSettings {
            // TODO Using XHTML for output because parser inside tests requires it.
            // This is a dependency on how HXL is currently using the XML tree builder.
            IsXhtml = true,
        };

        private DomElement _element;
        private HxlWriter _writer;
        private ITextOutput _outputBuffer;

        private ITextOutput OutputHelper {
            get {
                return _outputBuffer;
            }
        }

        public DomElement Element {
            get {
                return _element;
            }
        }

        public TextWriter Output {
            get {
                return this._writer.BaseWriter;
            }
        }

        public HxlTemplateContext TemplateContext {
            get {
                return this._writer.TemplateContext;
            }
        }

        // TODO Should this be API? (design)
        internal HxlWriter HxlOutput {
            get {
                return _writer;
            }
        }

        void IHxlElementTemplate.Render(DomElement element, HxlWriter output) {
            _element = element;
            try {
                _writer = output;
                _outputBuffer = output;
                Render();

            } finally {
                _writer = null;
                _outputBuffer = null;
            }
        }

        protected abstract void Render();

        protected void RenderBody() {
            _writer.Write(Element.ChildNodes);
        }

        internal static void RenderElementEnd(DomElement element, TextWriter w) {
            if (element.ChildNodes.Count > 0 || !Utility.IsSelfClosing(element)) {
                w.Write("</");
                w.Write(element.Name);
                w.Write(">");
            }
        }

        internal static IHxlElementTemplate ProcessAttributesForTemplate(DomElement element, HxlTemplateContext templateContext) {
            // TODO Cloning array is wasteful (performance)

            // TODO If attributes are added, they aren't considered
            // so if they are server attributes they don't get OnElementRendering_ (rare)

            IHxlElementTemplate template = null;
            // TODO Sort attributes by priority
            // TODO Capture any attribute text output (uncommon)
            foreach (var af in element.Attributes.OfType<HxlAttribute>().ToArray()) {
                if (af != null) {
                    // TODO Probably need to push context
                    template = af.OnElementRendering_(templateContext) ?? template;
                }
            }

            return template;
        }

        internal static void RenderElementStart(DomElement element, TextWriter w) {
            w.Write("<");
            w.Write(element.Name);

            foreach (var entry in element.Attributes) {
                if (!HxlAttribute.IsClientAttribute(entry))
                    continue;

                w.Write(" ");
                w.Write(entry.Name);

                if (entry.Value != null) {
                    w.Write("=\"");
                    w.Write(entry.Value); // TODO Conditional - HtmlEncoder.Escape(entry.Value, settings.Charset.GetEncoder(), settings.EscapeMode));
                    w.Write("\"");
                }
            }

            if (settings.IsXhtml && (element.ChildNodes.Count == 0 && Utility.IsSelfClosing(element))) {
                w.Write(" />");

            } else {
                w.Write(">");
            }
        }

        // `ITextOutput' glue
        public void Write(object value) {
            _outputBuffer.Write(value);
        }

        public void WriteLine(object value) {
            _outputBuffer.WriteLine(value);
        }

        public void Write(string value) {
            _outputBuffer.Write(value);
        }

        public void WriteLine(string value) {
            _outputBuffer.WriteLine(value);
        }

        public void Write(string format, params object[] args) {
            _outputBuffer.Write(format, args);
        }

        public void WriteLine(string format, params object[] args) {
            _outputBuffer.WriteLine(format, args);
        }

        public HxlWriter StartBufferContent(string name) {
            return _outputBuffer.StartBufferContent(name);
        }

        public void EndBufferContent(string name) {
            _outputBuffer.EndBufferContent(name);
        }

        sealed class EmptyElementTemplate : IHxlElementTemplate {
            void IHxlElementTemplate.Render(DomElement element, HxlWriter output) {}
        }

        class DefaultElementTemplate : IHxlElementTemplate {

            void IHxlElementTemplate.Render(DomElement element, HxlWriter output) {
                RenderElementStart(element, output.BaseWriter);
                output.Write(element.ChildNodes);
                RenderElementEnd(element, output.BaseWriter);
            }
        }

    }

}
