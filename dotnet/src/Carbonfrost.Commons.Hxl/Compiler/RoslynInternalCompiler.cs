//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

#if NETSTANDARD

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Reflection;
using Microsoft.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Emit;
using Carbonfrost.Commons.Hxl.Compiler;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class RoslynInternalCompiler : IHxlInternalCompiler {

        public HxlCompilerResults Compile(HxlCompilerSettings settings,
                                          IEnumerable<HxlTemplate> templates) {

            var parseOptions = new CSharpParseOptions(languageVersion: LanguageVersion.CSharp5);

            var session = new HxlCompilerSession(settings);
            string[] sourceFiles = templates.Select(t => GenerateSourceTempFile(session, t)).ToArray();

            // TODO Support generation of version in the output asm more easily
            // TODO Support private key signing in the output asm more easily
            string moduleName = "Hxl-" + session.SessionID;

            var emitOptions = new EmitOptions();

            var compilationOptions = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                moduleName,
                null,
                null,
                Empty<string>.List,
                settings.Debug ? OptimizationLevel.Debug : OptimizationLevel.Release,
                true,
                false,
                null,
                null,
                ImmutableArray<byte>.Empty,
                null,
                Platform.AnyCpu,
                ReportDiagnostic.Default,
                1,
                null,
                false,
                null,
                null,
                null,
                null,
                null
            );

            // TODO Previously, we indirectly supported other CodeDOM compiler parameters
            //   (embedded resources, etc.)
            settings.ImplicitAssemblyReferencePolicy.AddAssemblies(session.ImplicitAssemblyReferences);

            // TODO Only local paths are allowed
            var references = new List<MetadataReference>();
            references.AddRange(settings.Assemblies.Select(
                    t => MetadataReference.CreateFromFile(t.Source.LocalPath,
                         MetadataReferenceProperties.Assembly,
                         null)).ToArray());

            var compilation = CSharpCompilation.Create(
                moduleName,
                GetParseTrees(parseOptions, sourceFiles),
                references,
                compilationOptions);
            var outputPath = session.GetFileName(moduleName + ".dll");
            var outputStream = File.OpenWrite(outputPath);

            var results = compilation.Emit(outputStream,
                null,
                null,
                null,
                Empty<ResourceDescription>.List,
                emitOptions,
                CancellationToken.None);

            outputStream.Flush();
            outputStream.Dispose();

            return new HxlCompilerResults(new HxlCompilerResultsAdapter(outputPath, results));
        }

        IEnumerable<SyntaxTree> GetParseTrees(CSharpParseOptions parseOptions, IEnumerable<string> files) {
            foreach (var file in files) {
                yield return CSharpSyntaxTree.ParseText(File.ReadAllText(file),
                    parseOptions,
                    file,
                    null,
                    CancellationToken.None);
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

            private readonly EmitResult _results;
            private readonly HxlCompilerErrorCollection _errors;
            private readonly IList<string> _output;
            private readonly string _pathToAssembly;

            public HxlCompilerResultsAdapter(string pathToAssembly, EmitResult results) {
                _results = results;
                _errors = new HxlCompilerErrorCollection();
                foreach (var error in results.Diagnostics) {
                    var mapped = error.Location.GetMappedLineSpan();
                    var loc = mapped.StartLinePosition;
                    _errors.AddNew(error.Descriptor.Id,
                                   error.GetMessage(CultureInfo.CurrentCulture),
                                   mapped.Path,
                                   loc.Character,
                                   loc.Line,
                                   error.Severity != DiagnosticSeverity.Error);
                }
                _output = Empty<string>.List;
                _pathToAssembly = pathToAssembly;
            }

            public IList<string> Output {
                get {
                    return _output;
                }
            }

            public string PathToAssembly {
                get {
                    return _pathToAssembly;
                }
            }

            public int NativeCompilerReturnValue {
                get {
                    return 0;
                }
            }

            public Assembly CompiledAssembly {
                get {
                    return null;
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
