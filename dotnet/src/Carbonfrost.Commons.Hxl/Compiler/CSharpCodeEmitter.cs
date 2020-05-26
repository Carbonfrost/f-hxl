//
// - CSharpCodeEmitter.cs -
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
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Instrumentation;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class CSharpCodeEmitter {

        private readonly TextWriter output;
        private readonly ParsedTemplate template;

        public CSharpCodeEmitter(TextWriter output, ParsedTemplate template) {
            this.template = template;
            this.output = output;
        }

        public void WriteCode(IProfilerScope scope) {
            var result = this.template.PreparedDocument;

            if (IsCompletelyInlined(result, scope)) {
                return;
            }

            // We buffer this output so that we can present variables upfront for readability
            var buffer = new StringWriter();
            var visitor = new GenerateInitTextVisitor(buffer, this.template);
            visitor.Visit(result);

            CSTemplateGenerator g = new CSTemplateGenerator();
            ApplySettings(template, g);
            g.DomNodeVariables = visitor.AllDomNodeVariables;
            g.DomObjectVariables = visitor.AllDomObjectVariables;
            g.InitializeComponent = buffer.ToString();
            g.RenderIslands = visitor.RenderIslands;
            g.HasDocument = true;
            g.Accessibility = "public";

            string code = g.TransformText();
            output.WriteLine(code);
            output.WriteLine("// r references");
            foreach (var r in template.ImplicitAssemblyReferences) {
                output.WriteLine("// r {0}", r);
            }

            if (Metrics.EnableAdvancedParserMetrics) {
                scope.SourceCodeMetrics(false,
                                        this.template.PreparedDocument.DescendantNodes.Count(),
                                        code.Length,
                                        visitor.RenderIslandCount,
                                        g.DomNodeVariables.Count + g.DomNodeVariables.Count);
            }
        }

        private bool IsCompletelyInlined(DomContainer document, IProfilerScope scope) {
            if (document.ChildNodes.Count != 1)
                return false;

            var hxlRenderElement = document.ChildNodes[0] as HxlRenderWorkElement;

            if (hxlRenderElement == null || hxlRenderElement.ChildNodes.Count > 0)
                return false;

            scope.AddMetric("completelyInlined", true);
            CSTemplateGenerator g = new CSTemplateGenerator();
            ApplySettings(template, g);
            g.TransformTextCore =
                string.Join(Environment.NewLine, hxlRenderElement.PreLines)
                + string.Join(Environment.NewLine, hxlRenderElement.PostLines);

            g.HasDocument = false;

            string code = g.TransformText();
            output.WriteLine(code);

            if (Metrics.EnableAdvancedParserMetrics) {
                scope.SourceCodeMetrics(true,
                                        1,
                                        code.Length,
                                        0,
                                        0);
            }

            return true;
        }

        private void ApplySettings(ParsedTemplate tmp, ITemplateCodeGenerator g) {
            g.Namespace = tmp.Namespace;
            g.ClassName = tmp.ClassName;
            g.TemplateName = tmp.TemplateName;
            g.BaseClass = GetBaseClass(tmp);
            g.ModelType = Convert.ToString(tmp.ModelType);
            g.Accessibility = "public";
        }

        private string GetBaseClass(ParsedTemplate tmp) {
            // TODO Better handling of assembly qualified names (the assembly implies assembly that needs to be loaded by compiler)
            // TODO Resolving will probably be required if OriginalString is qual name (uncommon)

            var tr = tmp.Settings.SelectBaseClass(tmp.BaseClass, tmp.ModelType)
                ?? tmp.Settings.TemplateBaseClass
                ?? TypeReference.FromType(typeof(HxlTemplateExtension));
            return Regex.Replace(tr.ToString(), ",.*$", string.Empty);
        }
    }
}
