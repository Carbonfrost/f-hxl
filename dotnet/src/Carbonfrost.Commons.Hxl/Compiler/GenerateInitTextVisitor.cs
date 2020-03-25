//
// - GenerateInitTextVisitor.cs -
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
using System.Reflection;
using System.Text;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Core.Runtime.Expressions;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class GenerateInitTextVisitor : BaseCompilerVisitor {

        readonly Stack<string> stack = new Stack<string>();
        private readonly ExpressionSerializationManager manager = new ExpressionSerializationManager();
        private readonly IHxlTemplateBuilder _builder;
        private readonly HashSet<string> _slugCache = new HashSet<string>();

        public GenerateInitTextVisitor(TextWriter t,
                                       IHxlTemplateBuilder builder) : base(t) {
            _builder = builder;
        }

        protected override void VisitDocument(DomDocument document) {
            using (manager.CreateSession()) {
                DoChildVisit(document, "this.__document");
            }
        }

        protected override void VisitDocumentFragment(DomDocumentFragment fragment) {
            using (manager.CreateSession()) {
                DoChildVisit(fragment, "this.__document");
            }
        }

        protected override void VisitDirective(HxlDirective directive) {
            // TODO Allow directives second pass preprocessing (emit code)
        }

        protected override void VisitDocumentType(DomDocumentType documentType) {
            string varName = SafeNewVariable(documentType.Name);
            CurrentOutput.WriteLine(
                "{0} = this.__document.CreateDocumentType(\"{1}\", \"{2}\", \"{3}\");",
                varName,
                documentType.Name,
                documentType.PublicId,
                documentType.SystemId);
            DoAppend(varName);
        }

        protected override void VisitProcessingInstructionFragment(ProcessingInstructionFragment fragment) {
            // TODO This shouldn't be here (HxlDocument.CreatePI should init this)
            Activation.Initialize(fragment, HxlDirective.Parse(fragment.Data).Select(t => new KeyValuePair<string, object>(t.Key, t.Value)));
            fragment.GetInitCode("m", this, this.CurrentOutput);
            DoChildVisit(fragment, fragment.Target);
        }

        protected override void VisitText(DomText text) {
            string parent = stack.Peek();
            CurrentOutput.WriteLine("{0}.AppendText(\"{1}\");", parent, CodeUtility.Escape(text.Data));
        }

        protected override void VisitText(HxlTextElement element) {
            // These should have been converted to render islands, BUT there are a few
            // cases where we want to keep an object:
            // - parent is retained (easier to work with)
            // - only writing a space or tab (no need for the overhead)
            string parent = stack.Peek();
            CurrentOutput.WriteLine("{0}.AppendText(\"{1}\");", parent, CodeUtility.Escape(element.Data));
        }

        protected override void VisitElementFragment(ElementFragment fragment) {
            var hxlRenderElement = fragment as HxlRenderWorkElement;

            if (hxlRenderElement != null) {
                VisitRender(hxlRenderElement);
                return;
            }

            string varName = CodeUtility.EmitInstantiation(this.manager, this.output, fragment);
            EmitRenderingThunk(this.output, varName, fragment);

            DoAppend(varName); // Must occur first for factory methods such as AppendText to work
            DoChildVisit(fragment, varName, true);
        }

        protected override void VisitElement(DomElement element) {
            string varName = SafeNewVariable(element.Name);
            CurrentOutput.WriteLine(
                "{1} = this.__document.CreateElement(\"{0}\");",
                element.Name,
                varName);
            foreach (var m in element.Annotations<IRenderWorkElementCodeBuilder>()) {
                m.EmitCode(output, varName, element);
            }

            DoChildVisit(element, varName);
            DoAppend(varName);
        }

        protected override void VisitAttributeFragment(AttributeFragment fragment) {
            HxlExpressionAttribute werk = fragment as HxlExpressionAttribute;

            if (werk == null) {
                string variable = CodeUtility.EmitInstantiation(this.manager, this.output, fragment);
                EmitRenderingThunk(this.output, variable, fragment);
                DoAppend(variable);

            } else {

                string varName= SafeNewVariable(werk.GetType().Name, true);
                // TODO Possibly better to use other name in this attribute render closure

                werk.GetInitCode(varName, this, output);
                DoAppend(varName);
            }
        }

        protected override void VisitAttribute(DomAttribute attribute) {
            string parent = stack.Peek();
            CurrentOutput.WriteLine("{0}.Attribute(\"{1}\", \"{2}\");",
                                    parent,
                                    CodeUtility.Escape(attribute.Name),
                                    CodeUtility.Escape(attribute.Value));
        }

        private string GenerateSlug(HxlRenderWorkElement pre) {
            var line = pre.PreLines.FirstOrDefault(t => !string.IsNullOrEmpty(t)) ?? string.Empty;
            string result = CodeUtility.Slug("work_" + line.Replace("__self.Write", string.Empty));
            if (_slugCache.Add(result))
                return result;

            result += Utility.RandomID();
            _slugCache.Add(result);
            return result;
        }

        private void VisitRender(HxlRenderWorkElement element) {
            string parent = stack.Peek();
            string slug = GenerateSlug(element);
            string renderName = NewVariable("_Render" + slug, false, true);
            string varName = SafeNewVariable(renderName);

            CurrentOutput.WriteLine("{0} = global::{2}.Create({1});", varName, renderName.TrimStart('_'), typeof(ElementFragment).FullName);

            stack.Push(varName);
            PushRenderIsland(renderName.TrimStart('_'));
            element.WritePreLines(CurrentOutput);

            if (element.ChildNodes.Count > 0) {
                CurrentOutput.WriteLine("    __self.RenderBody();");
            }

            element.WritePostLines(CurrentOutput);
            PopRenderIsland();

            CurrentOutput.WriteLine("{0}.Append({1});", parent, varName);

            VisitAll(element.ChildNodes);
            stack.Pop();
        }

        protected override void VisitComment(DomComment comment) {
            // noop
        }

        private string SafeNewVariable(string suffix, bool isAttr = false) {
            string result;
            suffix = CodeUtility.Slug(suffix);

            if (stack.Count <= 1) {
                CurrentOutput.WriteLine();
                result = NewVariable("root_" + suffix, isAttr);
            } else {
                string variable = stack.Peek();
                result = NewVariable(string.Format("{0}_{1}", variable, suffix), isAttr);
            }

            return result;
        }

        private void DoAppend(string variable) {
            CurrentOutput.Write("{0}.Append({1});", stack.Peek(), variable);
            CurrentOutput.WriteLine();
        }

        private void DoChildVisit(DomNode element, string varName, bool skipAttr = false) {
            stack.Push(varName);
            // TODO Indent attributes and child nodes
            if (!skipAttr && element.Attributes != null) {
                VisitAll(element.Attributes);
            }

            VisitAll(element.ChildNodes);
            stack.Pop();
        }

        internal static void EmitRenderingThunk(TextWriter sw, string name, DomObject component) {
            StringBuilder rendering = new StringBuilder();

            // TODO Undesirable that expressions get serialized in EmitInstantiation
            // TODO This serialization logic implies that only strings can be used with expressions
            foreach (PropertyInfo m in Utility.ReflectGetProperties(component.GetType())) {
                if (m.CanWrite && m.PropertyType == typeof(string)) {
                    // TODO Need to check more than CanWrite -- may need to check accessibility of the setter (rare)

                    string text = (string) m.GetValue(component) ?? string.Empty;
                    if (HxlAttributeConverter.IsExpr(text)) {
                        CodeBuffer cb = new CodeBuffer();
                        RewriteExpressionSyntax.MatchVariablesAndEmit(cb, text);
                        rendering.AppendFormat("    {2}.{0} = {1};", m.Name, cb, name);
                        rendering.AppendLine();
                    }
                }
            }

            var additional = component.Annotations<ExpressionInitializers>();
            foreach (ExpressionInitializers m in additional) {
                rendering.AppendFormat("    {2}.{0} = {1};", m.Member.Name, m.Expression, name);
                rendering.AppendLine();
            }

            // TODO Use of __self__ is a hack (will it actually be used?)
            if (rendering.Length > 0) {
                sw.WriteLine("{0}.Rendering = (__context, __self__) => {{", name);
                sw.WriteLine("    dynamic __closure = __context;");
                sw.Write(rendering);
                sw.WriteLine("};");
            }
        }
    }
}
