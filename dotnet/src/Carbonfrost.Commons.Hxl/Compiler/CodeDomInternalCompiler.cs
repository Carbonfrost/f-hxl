//
// - CodeDomInternalCompiler.cs -
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

#if NET

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;
using Carbonfrost.Commons.Hxl.Compiler;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class CodeDomInternalCompiler : IHxlInternalCompiler {

        public HxlCompilerResults Compile(HxlCompilerSettings settings,
                                          IEnumerable<HxlTemplate> templates) {

            using (CSharpCodeProvider provider = new CSharpCodeProvider()) {
                CompilerParameters options = new CompilerParameters();

                var session = new HxlCompilerSession(settings);
                string[] sourceFiles = templates.Select(t => GenerateSourceTempFile(session, t)).ToArray();

                // TODO Support generation of version in the output asm more easily
                // TODO Support private key signing in the output asm more easily
                if (string.IsNullOrEmpty(options.OutputAssembly)) {
                    options.OutputAssembly = session.SessionID;
                }

                // TODO Previously, we indirectly supported other CodeDOM compiler parameters
                //   (embedded resources, etc.)
                settings.ImplicitAssemblyReferencePolicy.AddAssemblies(session.ImplicitAssemblyReferences);

                // TODO Only local paths are allowed
                options.ReferencedAssemblies.AddRange(settings.Assemblies.Select(t => t.Source.LocalPath).ToArray());
                options.IncludeDebugInformation = settings.Debug;

                var results = provider.CompileAssemblyFromFile(options, sourceFiles);
                return new HxlCompilerResults(new HxlCompilerResultsAdapter(results));
            }
        }

        string GenerateSourceTempFile(HxlCompilerSession session, HxlTemplate template) {
            string name, type;
            HxlTemplateAttribute.NameOrDefault(template, out name, out type);
            string tempFile = CodeUtility.Slug(name) + "-" + ((ParsedTemplate) template).Signature + ".cs";

            using (var sw = session.CreateText(tempFile)) {
                HxlCompiler.GenerateOneSourceFile(session, template, sw);
            }
            return session.GetFileName(tempFile);
        }

        class HxlCompilerResultsAdapter : IHxlInternalCompilerResults {

            private readonly CompilerResults _results;
            private readonly HxlCompilerErrorCollection _errors;
            private readonly IList<string> _output;

            public HxlCompilerResultsAdapter(CompilerResults results) {
                _results = results;
                _errors = new HxlCompilerErrorCollection();
                foreach (CompilerError error in results.Errors) {
                    _errors.AddNew(error.ErrorNumber,
                                   error.ErrorText,
                                   error.FileName,
                                   error.Column,
                                   error.Line,
                                   error.IsWarning);
                }
                _output = _results.Output.Cast<string>().ToList();
            }

            public IList<string> Output {
                get {
                    return _output;
                }
            }

            public string PathToAssembly {
                get {
                    return _results.PathToAssembly;
                }
            }

            public int NativeCompilerReturnValue {
                get {
                    return _results.NativeCompilerReturnValue;
                }
            }

            public Assembly CompiledAssembly {
                get {
                    return _results.CompiledAssembly;
                }
            }

            public HxlCompilerErrorCollection Errors {
                get {
                    return _errors;
                }
            }
        }
    }
}

#endif
