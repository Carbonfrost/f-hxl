//
// - TemplateBase.cs -
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
using System.Reflection;
using System.Linq;
using RExpression = Carbonfrost.Commons.Core.Runtime.Expressions.Expression;

namespace Carbonfrost.Commons.Hxl.Compiler {

    // Base class for all template classes generated in this
    // namespace.

    // Note that we use a hack so that the template classes
    // remain generated as internal classes.

    abstract class TemplateBase {

        // Mono.TextTemplating generates this code ---

        private global::System.Text.StringBuilder builder;

        private global::System.Collections.Generic.IDictionary<string, object> session;

        private string currentIndent = string.Empty;

        private global::System.Collections.Generic.Stack<int> indents;

        private bool endsWithNewline;

        private ToStringInstanceHelper _toStringHelper = new ToStringInstanceHelper();

        public virtual global::System.Collections.Generic.IDictionary<string, object> Session {
            get {
                return this.session;
            }
            set {
                this.session = value;
            }
        }

        public global::System.Text.StringBuilder GenerationEnvironment {
            get {
                if ((this.builder == null)) {
                    this.builder = new global::System.Text.StringBuilder();
                }
                return this.builder;
            }
            set {
                this.builder = value;
            }
        }

        public string CurrentIndent {
            get {
                return this.currentIndent;
            }
        }

        private global::System.Collections.Generic.Stack<int> Indents {
            get {
                if ((this.indents == null)) {
                    this.indents = new global::System.Collections.Generic.Stack<int>();
                }
                return this.indents;
            }
        }

        public ToStringInstanceHelper ToStringHelper {
            get {
                return this._toStringHelper;
            }
        }

        public string PopIndent() {
            if ((this.Indents.Count == 0)) {
                return string.Empty;
            }
            int lastPos = (this.currentIndent.Length - this.Indents.Pop());
            string last = this.currentIndent.Substring(lastPos);
            this.currentIndent = this.currentIndent.Substring(0, lastPos);
            return last;
        }

        public void PushIndent(string indent) {
            this.Indents.Push(indent.Length);
            this.currentIndent = (this.currentIndent + indent);
        }

        public void ClearIndent() {
            this.currentIndent = string.Empty;
            this.Indents.Clear();
        }

        public void Write(string textToAppend) {
            if (string.IsNullOrEmpty(textToAppend)) {
                return;
            }
            if ((((this.GenerationEnvironment.Length == 0)
                  || this.endsWithNewline)
                 && (this.CurrentIndent.Length > 0))) {
                this.GenerationEnvironment.Append(this.CurrentIndent);
            }
            this.endsWithNewline = false;
            char last = textToAppend[(textToAppend.Length - 1)];
            if (((last == '\n')
                 || (last == '\r'))) {
                this.endsWithNewline = true;
            }
            if ((this.CurrentIndent.Length == 0)) {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            int lastNewline = 0;
            for (int i = 0; (i
                             < (textToAppend.Length - 1)); i = (i + 1)) {
                char c = textToAppend[i];
                if ((c == '\r')) {
                    if ((textToAppend[(i + 1)] == '\n')) {
                        i = (i + 1);
                        if ((i
                             == (textToAppend.Length - 1))) {
                            goto breakLoop;
                        }
                    }
                }
                else {
                    if ((c != '\n')) {
                        goto continueLoop;
                    }
                }
                i = (i + 1);
                int len = (i - lastNewline);
                if ((len > 0)) {
                    this.GenerationEnvironment.Append(textToAppend, lastNewline, (i - lastNewline));
                }
                this.GenerationEnvironment.Append(this.CurrentIndent);
                lastNewline = i;
            continueLoop:
                ;
            }
        breakLoop:
            if ((lastNewline > 0)) {
                this.GenerationEnvironment.Append(textToAppend, lastNewline, (textToAppend.Length - lastNewline));
            }
            else {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }

        public void Write(string format, params object[] args) {
            this.Write(string.Format(format, args));
        }

        public void WriteLine(string textToAppend) {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }

        public void WriteLine(string format, params object[] args) {
            this.WriteLine(string.Format(format, args));
        }

        public abstract string TransformText();

        public virtual void Initialize() {}

        public class ToStringInstanceHelper {

            private global::System.IFormatProvider formatProvider = global::System.Globalization.CultureInfo.CurrentCulture;

            public global::System.IFormatProvider FormatProvider {
                get {
                    return this.formatProvider;
                }
                set {
                    if ((this.formatProvider == null)) {
                        throw new global::System.ArgumentNullException("formatProvider");
                    }
                    this.formatProvider = value;
                }
            }

            public string ToStringWithCulture(object objectToConvert) {
                if ((objectToConvert == null)) {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                global::System.Type type = objectToConvert.GetType();
                global::System.Type iConvertibleType = typeof(global::System.IConvertible);
                if (iConvertibleType.GetTypeInfo().IsAssignableFrom(type)) {
                    return ((global::System.IConvertible)(objectToConvert)).ToString(this.formatProvider);
                }
                global::System.Reflection.MethodInfo methInfo = type.GetTypeInfo().GetMethod("ToString", new global::System.Type[] {
                                                                                   iConvertibleType});
                if ((methInfo != null)) {
                    return ((string)(methInfo.Invoke(objectToConvert, new object[] {
                                                         this.formatProvider})));
                }
                return objectToConvert.ToString();
            }
        }
    }
}
