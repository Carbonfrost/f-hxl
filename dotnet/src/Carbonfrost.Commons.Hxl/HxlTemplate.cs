//
// - HxlTemplate.cs -
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
using System.Threading.Tasks;

namespace Carbonfrost.Commons.Hxl {

    public abstract partial class HxlTemplate {

        public string TransformText() {
            HxlTemplateContext ec = new HxlTemplateContext(this);
            return TransformText(ec);
        }

        public string TransformText(IEnumerable<KeyValuePair<string, object>> variables) {
            HxlTemplateContext ec = new HxlTemplateContext(this);
            ec.Data.AddMany(variables);
            return TransformText(ec);
        }

        public string TransformText(HxlTemplateContext context) {
            StringWriter sw = new StringWriter();
            Transform(sw, context);
            return sw.ToString();
        }

        public string GenerateSource(HxlCompilerSettings settings = null) {
            StringWriter sw = new StringWriter();
            GenerateSource(sw, settings);
            return sw.ToString();
        }

        public void GenerateSource(TextWriter outputWriter, HxlCompilerSettings settings = null) {
            if (outputWriter == null)
                throw new ArgumentNullException("outputWriter");

            HxlCompiler.Create(settings).GenerateSource(outputWriter, this);
        }

        public void Transform(Stream outputStream, IEnumerable<KeyValuePair<string, object>> variables) {
            Transform(outputStream, HxlTemplateContext.WithData(this, variables));
        }

        public void Transform(string outputFile, IEnumerable<KeyValuePair<string, object>> variables) {
            Transform(outputFile, HxlTemplateContext.WithData(this, variables));
        }

        public void Transform(TextWriter outputWriter, IEnumerable<KeyValuePair<string, object>> variables) {
            Transform(outputWriter, HxlTemplateContext.WithData(this, variables));
        }

        public void Transform(Stream outputStream) {
            Transform(outputStream, new HxlTemplateContext(this));
        }

        public void Transform(string outputFile) {
            Transform(outputFile, new HxlTemplateContext(this));
        }

        public virtual void Transform(TextWriter outputWriter) {
            Transform(outputWriter, new HxlTemplateContext(this));
        }

        public void Transform(string outputFile, HxlTemplateContext context) {
            using (FileStream fs = new FileStream(outputFile, FileMode.Create)) {
                using (StreamWriter sw = new StreamWriter(fs)) {
                    Transform(sw, context);
                }
            }
        }

        public abstract void Transform(TextWriter outputWriter, HxlTemplateContext context);

        public virtual void Transform(Stream outputStream, HxlTemplateContext context) {
            if (outputStream == null)
                throw new ArgumentNullException("outputStream");

            using (StreamWriter sw = new StreamWriter(outputStream)) {
                Transform(sw, context);
            }
        }

        public virtual Task TransformAsync(TextWriter outputWriter, HxlTemplateContext context) {
            return Task.Run(() => Transform(outputWriter, context));
        }

        public virtual Task<string> TransformTextAsync(HxlTemplateContext context) {
            return Task.Run(() => TransformText(context));
        }
    }

}
