//
// - HxlTemplateExtension.cs -
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Hxl.Controls;

namespace Carbonfrost.Commons.Hxl {

    public abstract class HxlTemplateExtension : HxlTemplate, ITextOutput {

        private bool _init;
        private readonly Stack<HxlTemplateContext> _contexts = new Stack<HxlTemplateContext>();
        private HxlTemplateLogHelper _log;

        public bool IsInitialized {
            get {
                return this._init;
            }
        }

        public bool IsMasterTemplate {
            get {
                return TemplateContext.MasterInfo != null;
            }
        }

        public dynamic Data {
            get {
                return this.TemplateContext;
            }
        }

        public HxlTemplateContext TemplateContext {
            get {
                return _contexts.PeekOrDefault();
            }
        }

        protected HxlTemplateLogHelper Console {
            get {
                if (_log == null)
                    _log = new HxlTemplateLogHelper(this);

                return _log;
            }
        }

        protected TextWriter Output {
            get;
            private set;
        }

        public override void Transform(TextWriter outputWriter, HxlTemplateContext context) {
            if (outputWriter == null)
                throw new ArgumentNullException("outputWriter");
            if (context == null)
                context = new HxlTemplateContext(this);

            PushTemplateContext(context);
            try {
                this.Output = outputWriter;

                EnsureInit();
                ApplyMaster();

                TransformTextCore();

            } finally {
                this.Output = null;
                PopTemplateContext();
            }
        }

        public void PushTemplateContext(HxlTemplateContext templateContext) {
            if (templateContext == null)
                throw new ArgumentNullException("templateContext");

            this._contexts.Push(templateContext);
        }

        public void PopTemplateContext() {
            this._contexts.Pop();
        }

        protected abstract void TransformTextCore();

        public void Initialize() {
            if (_init)
                throw Failure.AlreadyInitialized();

            EnsureInit();
        }

        protected virtual void InitializeCore() {
        }

        // TODO May want to make a version of this virtual

        private void ApplyMaster() {
            if (!IsMasterTemplate) {
                return;
            }

            var masterInfo = this.TemplateContext.MasterInfo;
            var placeholderContent = masterInfo.PlaceholderContent;
            var mergeAttributes = masterInfo.LayoutElement;
            var master = this;

            // TODO If template isnt IHxlDocumentFragmentAccessor, then this is an error
            // TODO It is also possible there is a document being used for a master with multiple root elements
            var access = ((IHxlDocumentFragmentAccessor) master);
            var de = access.DocumentFragment.FirstChild;
            HxlPlaceholderContentProvider.MergeAttributes(mergeAttributes, de);

            de.Attribute("data-layout", masterInfo.LayoutName);

            // If master has no doctype, then copy doctype from child
            if (access.DocumentFragment.ChildNodes.All(t => t.NodeType != DomNodeType.DocumentType)) {
                var dt = mergeAttributes.ChildNodes.OfType<DomDocumentType>().FirstOrDefault();
                if (dt != null)
                    access.DocumentFragment.ChildNodes.Insert(0, dt);
            }

            // Merge the head element if present
            var myHead = de.Element("head");

            if (myHead != null && masterInfo.HeadElement != null) {
                // TODO Array copy is wasteful (performance)
                foreach (var m in masterInfo.HeadElement.ChildNodes.ToArray()) {

                    // TODO XMLNS might not be correct (rare)
                    if (m.HasAttributes && (m.HasAttribute("hxl:placeholder")
                                            || m.HasAttribute("hxl:placeholdertarget")))
                        continue;

                    myHead.Append(m);
                }
            }
        }

        private void EnsureInit() {
            if (!_init)
                InitializeCore();

            _init = true;
        }

        public void Write(object value) {
            ToStringHelper.PrintString(this.Output, value);
        }

        public void Write(string text) {
            Output.Write(text);
        }

        public void Write(string format, params object[] args) {
            Output.Write(format, args);
        }

        public void WriteLine(object value) {
            ToStringHelper.PrintString(this.Output, value);
            this.Output.WriteLine();
        }

        public void WriteLine(string text) {
            Output.WriteLine(text);
        }

        public void WriteLine(string format, params object[] args) {
            Output.WriteLine(format, args);
        }

        public HxlWriter StartBufferContent(string name) {
            throw new NotImplementedException();
        }

        public void EndBufferContent(string name) {
            throw new NotImplementedException();
        }

        TextWriter ITextOutput.Output {
            get {
                return Output;
            }
        }

    }
}
