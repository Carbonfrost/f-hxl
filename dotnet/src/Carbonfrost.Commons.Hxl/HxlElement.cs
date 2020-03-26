//
// Copyright 2013, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.IO;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Hxl {

    public abstract class HxlElement : DomElement<HxlElement>, IHxlNode, ITextOutput {

        static readonly Regex ELEMENT = new Regex("(^Hxl)|(Element$)|(ElementFragment$)", RegexOptions.IgnoreCase);

        private ITextOutput _outputBuffer = InvalidTextOutput.Instance;
        private DomStringTokenList classList;

        [ExpressionSerializationMode(ExpressionSerializationMode.Hidden)]
        public Action<HxlTemplateContext, HxlElement> Rendering { get; set; }

        public HxlTemplateContext TemplateContext {
            get;
            private set;
        }

        public TextWriter Output {
            get {
                return _outputBuffer.Output;
            }
        }

        [ExpressionSerializationMode(ExpressionSerializationMode.Hidden)]
        public string ClassName {
            get {
                return this.Attribute("class");
            }
            set {
                this.Attribute("class", value);
            }
        }

        [ExpressionSerializationMode(ExpressionSerializationMode.Hidden)]
        public DomStringTokenList ClassList {
            get {
                if (classList == null) {
                    classList = new DomStringTokenList { Value = ClassName };
                    this.Attribute("class", classList);
                }
                return classList;
            }
        }

        [ExpressionSerializationMode(ExpressionSerializationMode.Hidden)]
        public string Class {
            get { return ClassName; }
            set { ClassName = value; }
        }

        [ExpressionSerializationMode(ExpressionSerializationMode.Hidden)]
        public override string OuterXml {
            get { return base.OuterXml; }
            set {
                // TODO This setter is not available because of f-core-runtime-expressions
                // not properly filtering it out (metadata is set up correctly)
            }
        }

        protected HxlElement()
            : base() {}

        protected HxlElement(string name) : base(name) {
        }

        public void RenderBody() {
            var hw = _outputBuffer as HxlWriter;
            if (hw != null)
                hw.Write(ChildNodes);
        }

        internal void Render_(HxlTemplateContext templateContext,
                              HxlWriter hxlWriter)
        {
            try {
                this.TemplateContext = templateContext;
                _outputBuffer = hxlWriter;

                if (Rendering != null) {
                    Rendering(templateContext, this);
                }

                Render();

            } finally {
                this.TemplateContext = null;
                _outputBuffer = InvalidTextOutput.Instance;
            }
        }

        public abstract void Render();

        void IHxlNode.AcceptVisitor(IHxlVisitor visitor) {
            AcceptVisitor(visitor);
        }

        internal virtual void AcceptVisitor(IHxlVisitor visitor) {
            visitor.Visit(this);
        }

        public static HxlElement FromText(string text) {
            return new LiteralFragment(text);
        }

        internal static string GetImplicitName(Type type) {
            string result = ELEMENT.Replace(type.Name, string.Empty);
            if (result.Length == 0)
                return type.Name.ToLowerInvariant();
            else
                return result.ToLowerInvariant();
        }

        // `ITextOutput' glue
        public HxlWriter StartBufferContent(string name) {
            return _outputBuffer.StartBufferContent(name);
        }

        public void EndBufferContent(string name) {
            _outputBuffer.EndBufferContent(name);
        }

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

        // TODO Technically, any markup can be written by element fragments, which makes them more like
        // macros or PIs (design)

        public static HxlElement Create(Action<dynamic, HxlElement> action) {
            if (action == null)
                return new NullFragment();
            else
                return new ActionFragment(action);
        }

        sealed class NullFragment : HxlElement {
            public override void Render() {}
        }

        sealed class ActionFragment : HxlElement {

            private readonly Action<dynamic, HxlElement> _action;

            public ActionFragment(Action<dynamic, HxlElement> action) {
                _action = action;
            }

            public override void Render() {
                _action(this.TemplateContext, this);
            }
        }

        sealed class LiteralFragment : HxlElement {

            private readonly string literal;

            public LiteralFragment(string literal) {
                this.literal = literal;
            }

            public override void Render() {
                this.Write(literal);
            }
        }
    }
}
