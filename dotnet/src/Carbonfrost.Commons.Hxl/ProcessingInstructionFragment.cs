//
// - ProcessingInstructionFragment.cs -
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
using System.Linq;

using System.Reflection;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public abstract class ProcessingInstructionFragment : DomProcessingInstruction<ProcessingInstructionFragment>, IHxlNode {

        public HxlTemplateContext TemplateContext {
            get;
            private set;
        }

        protected ProcessingInstructionFragment(string target) : base(target) {
        }

        void IHxlNode.AcceptVisitor(IHxlVisitor visitor) {
            AcceptVisitor(visitor);
        }

        internal virtual void AcceptVisitor(IHxlVisitor visitor) {
            visitor.Visit(this);
        }

        internal bool NeedsEmit {
            get {
                var methodInfo = GetType().GetMethod("EmitCode", BindingFlags.Instance | BindingFlags.NonPublic);
                return methodInfo.DeclaringType != typeof(ProcessingInstructionFragment);
            }
        }

        internal void Execute_(HxlTemplateContext context) {
            try {
                this.TemplateContext = context;
                SyncDataProperty();
                Execute();

            } finally {
                this.TemplateContext = null;
            }
        }

        protected virtual void Execute() {
            // TODO This isn't called if the PI is being interpreted
            throw FutureFeatures.InterpretedLanguageElements();
        }

        protected virtual void EmitCode(IHxlTemplateEmitter emitter) {
        }

        internal void Preprocess_(IHxlTemplateBuilder builder) {
            SyncDataProperty();
            Preprocess(builder);
        }

        protected virtual void Preprocess(IHxlTemplateBuilder builder) {
        }

        internal void GetInitCode(string variable, IHxlTemplateEmitter context, TextWriter tw) {
            EmitCode(context);
        }

        private void SyncDataProperty() {
            var props = HxlDirective.Parse(Data);
            Activation.Initialize(this, props);

        }
    }

}
