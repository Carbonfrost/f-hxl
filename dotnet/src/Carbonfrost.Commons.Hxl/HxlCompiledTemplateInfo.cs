//
// - HxlCompiledTemplateInfo.cs -
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
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl {

    public class HxlCompiledTemplateInfo : IHxlTemplateOperations {

        private readonly string _templateType;
        private readonly Type _compiledType;
        private readonly string _name;

        public string Name {
            get {
                return _name;
            }
        }

        public string Type {
            get {
                return _templateType;
            }
        }

        public Type CompiledType {
            get {
                return _compiledType;
            }
        }

        internal TemplateKey Key {
            get {
                return new TemplateKey(Name, Type);
            }
        }

        internal HxlCompiledTemplateInfo(string name,
                                         string type,
                                         Type compiled) {
            _name = name;
            _templateType = type;
            _compiledType = compiled;
        }

        public HxlTemplate CreateInstance() {
            return Activation.CreateInstance<HxlTemplate>(CompiledType);
        }

        public void Transform(TextWriter outputWriter, HxlTemplateContext context) {
            CreateInstance().Transform(outputWriter, context);
        }

        public string GenerateSource(HxlCompilerSettings settings = null) {
            return CreateInstance().GenerateSource(settings);
        }

        public string TransformText() {
            return CreateInstance().TransformText();
        }

        public string TransformText(HxlTemplateContext context) {
            return CreateInstance().TransformText(context);
        }

        public string TransformText(IEnumerable<KeyValuePair<string, object>> variables) {
            return CreateInstance().TransformText(variables);
        }

        public void Transform(Stream outputStream, HxlTemplateContext context) {
            CreateInstance().Transform(outputStream, context);
        }

        public void Transform(TextWriter outputWriter) {
            CreateInstance().Transform(outputWriter);
        }

        public void GenerateSource(TextWriter outputWriter, HxlCompilerSettings settings = null) {
            // TODO Generate source attachment
            throw new NotImplementedException();
        }

        public void Transform(Stream outputStream) {
            CreateInstance().Transform(outputStream);
        }

        public void Transform(Stream outputStream, IEnumerable<KeyValuePair<string, object>> variables) {
            CreateInstance().Transform(outputStream, variables);
        }

        public void Transform(string outputFile) {
            CreateInstance().Transform(outputFile);
        }

        public void Transform(string outputFile, HxlTemplateContext context) {
            CreateInstance().Transform(outputFile, context);
        }

        public void Transform(string outputFile, IEnumerable<KeyValuePair<string, object>> variables) {
            CreateInstance().Transform(outputFile, variables);
        }

        public void Transform(TextWriter outputWriter, IEnumerable<KeyValuePair<string, object>> variables) {
            CreateInstance().Transform(outputWriter, variables);
        }

        public override string ToString() {
            if (TemplateKey.IsDefaultTemplateType(_templateType)) {
                return string.Format("{0} => {1}", Name, CompiledType);
            } else {
                return string.Format("{0} ({2}) => {1}", Name, CompiledType, Type);
            }
        }

    }
}

