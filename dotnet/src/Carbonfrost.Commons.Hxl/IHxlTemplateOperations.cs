//
// - IHxlTemplateOperations.cs -
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

namespace Carbonfrost.Commons.Hxl {

    interface IHxlTemplateOperations {

        // Glue for HxlTemplate and HxlCompiledTemplateInfo
        void Transform(TextWriter outputWriter, HxlTemplateContext context);
        string GenerateSource(HxlCompilerSettings settings = null);
        string TransformText();
        string TransformText(HxlTemplateContext context);
        string TransformText(IEnumerable<KeyValuePair<string, object>> variables);
        void Transform(Stream outputStream, HxlTemplateContext context);
        void Transform(TextWriter outputWriter);
        void GenerateSource(TextWriter outputWriter, HxlCompilerSettings settings = null);
        void Transform(Stream outputStream);
        void Transform(Stream outputStream, IEnumerable<KeyValuePair<string, object>> variables);
        void Transform(string outputFile);
        void Transform(string outputFile, HxlTemplateContext context);
        void Transform(string outputFile, IEnumerable<KeyValuePair<string, object>> variables);
        void Transform(TextWriter outputWriter, IEnumerable<KeyValuePair<string, object>> variables);
    }
}
