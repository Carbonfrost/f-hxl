//
// - HxlTemplate.Static.cs -
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
using System.Reflection;
using System.Text.RegularExpressions;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl {

    partial class HxlTemplate {

        public static HxlTemplate Parse(string text) {
            return HxlCompiler.Create().ParseTemplate(text);
        }

        public static bool TryParse(string text, out HxlTemplate result) {
            result = Parse(text);
            return true;
        }

        public static string RenderText(string inputFile, IEnumerable<KeyValuePair<string, object>> variables = null) {
            HxlTemplate template = HxlTemplate.FromFile(inputFile);
            return template.TransformText(variables);
        }

        public static void Render(string inputFile, string outputFile, IEnumerable<KeyValuePair<string, object>> variables = null) {
            HxlTemplate template = HxlTemplate.FromFile(inputFile);
            template.Transform(outputFile, variables);
        }

        public static HxlTemplate FromFile(string fileName) {
            return FromStreamContext(StreamContext.FromFile(fileName));
        }

        public static HxlTemplate FromStream(Stream input) {
            if (input == null)
                throw new ArgumentNullException("input");

            // TODO Better name template name generation
            return FromStreamContext(StreamContext.FromStream(input));
        }

        public static HxlTemplate FromStreamContext(StreamContext input) {
            if (input == null)
                throw new ArgumentNullException("input");

            return HxlCompiler.Create().LoadTemplate(input);
        }

        public static HxlTemplate FromSource(Uri source) {
            return FromStreamContext(StreamContext.FromSource(source));
        }

        public static HxlTemplate FromName(Assembly assembly, string name) {
            return FromName(assembly, name, null);
        }

        public static HxlTemplate FromName(Assembly assembly, string name, string type) {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

#if NET
            if (assembly.ReflectionOnly)
                throw HxlFailure.AssemblyIsReflectionOnly("assembly");
#endif
            var f = HxlTemplateFactory.FromAssembly(assembly);
            return f.CreateTemplate(name, null, null);
        }
    }

}
