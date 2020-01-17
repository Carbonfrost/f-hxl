//
// - HxlcApp.cs -
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
using System.Reflection;
using System.Xml;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;
using Microsoft.CSharp.RuntimeBinder;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class HxlcApp {

        public readonly ProgramOptions Options;

        private readonly IHxlcLogger logger;

        public HxlcApp(ProgramOptions options) {
            this.Options = options;
            logger = new HxlcLogger();
        }

        public int Run() {
            try {

                return RunCore();

            } catch (HxlcException ex) {
                Console.WriteLine("HXLC{0:0000}: {1}", ex.ErrorCode, ex.Message);

            } catch (FileLoadException ex) {
                Console.WriteLine(ex.ToString());

            } catch (BadImageFormatException ex) {
                Console.WriteLine(ex.ToString());

            } catch (Exception ex) {
                // TODO Display this appropriately
                Console.WriteLine(ex.ToString());
            }

            return 1;
        }

        static IEnumerable<string> DefaultCompilerSettings() {
            foreach (var asm in HxlAssemblyCollection.System) {
                yield return asm.Source.LocalPath;
            }
        }

        private IEnumerable<KeyValuePair<AssemblyName, string>> LoadReferences() {
            // HACK This code clone is required because of Shared Runtime provider loading order bug
            // This must be done first so as to make sure that providers aren't cached
            // within Shared Runtime

            var collectedReferences = new Dictionary<AssemblyName, string>(Comparers.AssemblyNameComparer);
            var requiredReferences = Options.References.EnumerateFilePaths()
                .Concat(DefaultCompilerSettings());

            foreach (var reference in requiredReferences) {
                Traceables.LoadAssembly(reference);

                try {
                    // Don't reload assemblies
                    var name = AssemblyName.GetAssemblyName(reference);

                    try {
                        // We prefer assemblies that HXLC was built against
                        Assembly.Load(name);

                    } catch {
                        Assembly.LoadFrom(reference);
                    }

                    collectedReferences[name] = reference;

                } catch (Exception ex) {
                    logger.FailedToLoadAssemblyReference(reference, ex);
                    // TODO Should have robust error handlng
                }
            }

            return collectedReferences;
        }

        private int RunCore() {
            // TODO Validate options

            var collectedReferences = LoadReferences();
            var settings = new HxlCompilerSettings();

            foreach (var reference in collectedReferences) {
                settings.Assemblies.AddNew(reference.Key, new Uri(reference.Value, UriKind.RelativeOrAbsolute));
            }
            // TODO Support output assembly name
            //   Path.GetFileNameWithoutExtension(Options.OutputFile);
            foreach (var kvp in Options.Namespaces) {
                settings.Namespaces.AddNew(kvp.Key, new Uri(kvp.Value));
            }

            var compiler = HxlCompiler.Create(settings);
            var templates = new List<HxlTemplate>();

            foreach (var item in Options.Inputs.EnumerateFiles()) {
                string file = item.File;
                logger.ParsingTemplate(file);

                // TODO Get access to template builder another way
                var template = compiler.LoadTemplate(file);
                var builder = (IHxlTemplateBuilder) template;

                var viewType = Options.BaseType
                    ?? TypeReference.Parse("Carbonfrost.Commons.Hxl.HxlViewTemplate");

                // TODO Drop this- should be using the type selector
                // TODO Allow type selector to be configurable
                builder.BaseClass = viewType;
                if (builder.ModelType == null)
                    builder.BaseClass = viewType;
                else {
                    string text = viewType.OriginalString + "<" + builder.ModelType + ">";
                    builder.BaseClass = TypeReference.Parse(text);
                }

                builder.TemplateName = PathHelper.UnixPath(PathHelper.MakeRelative(item.OriginalGlob, file));
                builder.ClassName = CodeUtility.Slug(builder.TemplateName);
                templates.Add(template);
            }
            if (templates.Count == 0) {
                logger.NoSourceFilesSpecified();
                return 1;
            }

            if (Options.NoCompile) {
                return GenerateSource(compiler, templates);
            } else {
                return CompileSource(compiler, templates);
            }
        }

        private int GenerateSource(HxlCompiler compiler, List<HxlTemplate> templates) {
            string outputDirectory = ".";
            if (!string.IsNullOrWhiteSpace(Options.OutputFile)) {
                outputDirectory = Options.OutputFile;
            }

            foreach (var templ in templates) {
                var generatedSource = compiler.GenerateSource(templ);

                // TODO Get access to template builder another way
                var builder = (IHxlTemplateBuilder) templ;
                var outputFile = Path.Combine(outputDirectory, builder.ClassName + ".g.cs");

                // TODO Handle file errors more gracefully
                File.WriteAllText(outputFile, generatedSource);
                logger.SavedOutputFile(outputFile);
            }
            return 0;
        }

        private int CompileSource(HxlCompiler compiler, List<HxlTemplate> templates) {
            var compilerResult = compiler.Compile(templates);
            logger.LogCompilerErrors(compilerResult.Errors);

            // TODO Not handling compile errors appropriately

            if (compilerResult.Errors.HasErrors) {
                throw new NotImplementedException();
            }

            if (compilerResult.NativeCompilerReturnValue != 0) {
                throw new NotImplementedException();
            }

            // TODO Handle file errors more gracefully

            File.Copy(
                compilerResult.PathToAssembly,
                Options.OutputFile,
                true);

            logger.SavedOutputFile(Options.OutputFile);
            return 0;
        }
    }
}
