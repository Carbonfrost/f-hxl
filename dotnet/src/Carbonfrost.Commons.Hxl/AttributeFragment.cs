//
// - AttributeFragment.cs -
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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public abstract class AttributeFragment : DomAttribute<AttributeFragment>, IHxlNode, ITextOutput {

        private static volatile int globalIndex;
        private HxlTemplateContext _templateContext;

        private ITextOutput _outputBuffer = InvalidTextOutput.Instance;

        static readonly Regex ATTRIBUTE = new Regex("(^Hxl)|(Attribute$)|(AttributeFragment$)", RegexOptions.IgnoreCase);

        // HACK This action to fulfill pre render eval is tacky.

        [ExpressionSerializationMode(ExpressionSerializationMode.Hidden)]
        public Action<HxlTemplateContext, AttributeFragment> Rendering { get; set; }

        public HxlTemplateContext TemplateContext {
            get {
                return _templateContext;
            }
        }

        protected AttributeFragment()
            : base() {
            SetupValue();
        }

        protected AttributeFragment(string name)
            : base(name) {
            SetupValue();
        }

        private void SetupValue() {
            var pd = HxlAttributeFragmentDefinition.ForComponent(this).ValueProperty;

            if (pd != null)
                SetValue(new ValueDomValue(this, pd));
        }

        public static AttributeFragment Create(string name, Func<dynamic, AttributeFragment, string> valueFactory) {
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(name))
                throw Failure.EmptyString("name");

            if (valueFactory == null)
                throw new ArgumentNullException("valueFactory");

            return new ThunkFragment(name, valueFactory);
        }

        internal static string GetImplicitName(Type type) {
            string result = ATTRIBUTE.Replace(type.Name, string.Empty);
            if (result.Length == 0)
                return type.Name.ToLowerInvariant();
            else
                return result.ToLowerInvariant();
        }

        internal static AttributeFragment Combine(
            string name,
            Func<dynamic, AttributeFragment, string> thunk1,
            Func<dynamic, AttributeFragment, string> thunk2) {

            Func<dynamic, AttributeFragment, string> combined = (s, self) => string.Concat(thunk1(s, self), " ", thunk2(s, self));
            return Create(name, combined);
        }

        // `ITextOutput' glue
        protected TextWriter Output {
            get {
                return _outputBuffer.Output;
            }
        }

        TextWriter ITextOutput.Output {
            get {
                return Output;
            }
        }

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

        internal static string RandomName() {
            return "c:__attr" + (globalIndex++);
        }

        protected virtual IElementTemplate OnElementRendering() {
            return null;
        }

        internal IElementTemplate OnElementRendering_(HxlTemplateContext context) {
            try {
                this._templateContext = context;
                this._outputBuffer = CreateOutputBuffer();

                if (Rendering != null) {
                    Rendering(context, this);
                }

                // TODO This may benefit from being overridable behavior
                GetElementData();

                return OnElementRendering();

            } finally {
                this._templateContext = null;
                this._outputBuffer = null;
            }
        }

        private ITextOutput CreateOutputBuffer() {
            // TODO Should assign output buffer according to usage
            return InvalidTextOutput.Instance;
        }

        void GetElementData() {
            var elementData = _templateContext.GetElementData(true);
            var props = HxlAttributeFragmentDefinition.ForComponent(this).ElementDataProperties;
            foreach (var prop in props) {
                try {
                    elementData[prop.Name] = prop.GetValue(this);

                } catch (Exception ex) {
                    Traceables.HandleComponentModelReflection(prop, ex);
                }
            }
        }

        // TODO Consider URI context

        void IHxlNode.AcceptVisitor(IHxlVisitor visitor) {
            AcceptVisitor(visitor);
        }

        internal virtual void AcceptVisitor(IHxlVisitor visitor) {
            visitor.Visit(this);
        }

        internal static bool IsClientAttribute(DomAttribute a) {
            return (a is ThunkFragment) || !(a is AttributeFragment);
        }

        internal sealed class ThunkFragment : AttributeFragment {

            internal readonly Func<dynamic, AttributeFragment, string> _action;

            public ThunkFragment(string name, Func<dynamic, AttributeFragment, string> action) : base(name) {
                _action = action;
                this.Rendering = (context, self) => this.Value = action(context, self);
            }
        }
    }

}
