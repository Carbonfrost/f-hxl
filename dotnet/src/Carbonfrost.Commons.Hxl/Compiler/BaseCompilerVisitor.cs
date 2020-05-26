//
// - BaseCompilerVisitor.cs -
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
using System.IO;
using System.Linq;
using System.Text;
using RExpression = Carbonfrost.Commons.Core.Runtime.Expressions.Expression;

namespace Carbonfrost.Commons.Hxl.Compiler {

    abstract class BaseCompilerVisitor : HxlLanguageVisitor, IHxlTemplateEmitter {

        protected readonly TextWriter output;

        private readonly StringBuilder literalCache = new StringBuilder();
        private readonly Dictionary<string, int> varPrefixes = new Dictionary<string, int>();
        private readonly List<string> _allDomNodeVars = new List<string>(64);
        private readonly List<string> _allDomObjectVars = new List<string>(64);
        private int _renderIslandCount;

        readonly StringBuilder render = new StringBuilder();
        readonly Stack<TextWriter> outputStack = new Stack<TextWriter>();

        public TextWriter CurrentOutput {
            get {
                return outputStack.Peek();
            }
        }

        public int RenderIslandCount {
            get {
                return _renderIslandCount;
            }
        }

        public string RenderIslands {
            get { return this.render.ToString(); }
        }

        public IList<string> AllDomNodeVariables {
            get {
                return this._allDomNodeVars;
            }
        }

        public IList<string> AllDomObjectVariables {
            get {
                return this._allDomObjectVars;
            }
        }

        protected BaseCompilerVisitor(TextWriter output) {
            this.output = output;
            this.outputStack.Push(output);
        }

        protected void PushRenderIsland(string name) {
            var sw = new StringWriter();
            sw.WriteLine("private void {0}(dynamic __closure, global::{1} __self) {{", name, typeof(HxlElement));
            this.outputStack.Push(sw);
        }

        protected void PopRenderIsland() {
            ClearLiteralCache();
            var island = this.outputStack.Pop();
            island.Write("}");
            _renderIslandCount++;
            render.AppendLine(((StringWriter) island).ToString());
        }

        void IHxlTemplateEmitter.EmitCode(string code) {
            EmitCode(code);
        }

        void IHxlTemplateEmitter.EmitLiteral(string text) {
            EmitLiteral(text);
        }

        void IHxlTemplateEmitter.EmitValue(RExpression expr) {
            EmitValue(expr);
        }

        public string NewVariable(string prefix, bool isAttr, bool dontStore = false) {
            int count = varPrefixes.GetValueOrDefault(prefix);
            count++;
            varPrefixes[prefix] = count;
            string newVar = (count == 1) ? prefix : prefix + count;

            if (!dontStore) {
                (isAttr ? _allDomObjectVars : _allDomNodeVars).Add(newVar);
            }
            return newVar;
        }

        protected void EmitLiteral(string literal) {
            literalCache.Append(literal);
        }

        protected void EmitValue(RExpression expr) {
            EmitCode("base.Write({0});", expr);
        }

        protected void EmitCode(string str) {
            ClearLiteralCache();
            CurrentOutput.WriteLine(str);
        }

        protected void EmitCode(string format, params object[] args) {
            EmitCode(string.Format(format, args));
        }

        private void ClearLiteralCache() {
            if (literalCache.Length > 0) {
                CurrentOutput.WriteLine("base.Write(\"{0}\");", literalCache);
                literalCache.Length = 0;
            }
        }

        protected void RequireValidVariable(string name, bool checkNew) {
            // TODO Check whether a valid C# identifier
            // TODO Check whether the variable is new to the scope
        }
    }
}

