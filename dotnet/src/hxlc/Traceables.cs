//
// - Traceables.cs -
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

using System.Linq;
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.Commons.Hxl.Compiler {

    static class Traceables {

        // TODO Handle logging and output

        public static void LoadAssembly(string reference) {
        }

        public static void FailedToLoadAssemblyReference(this IHxlcLogger log, string reference, Exception ex) {
            log.ErrorFormat("Compilation may fail - Failed to load an assembly: {0}", reference);
            log.Debug(ex);
        }

        public static void NoSourceFilesSpecified(this IHxlcLogger log) {
            log.Error("No source files specified.");
        }

        public static void SavedGeneratedSourceFile(this IHxlcLogger log, string outputFile) {
            log.Trace(outputFile);
        }

        public static void SavedOutputFile(this IHxlcLogger log, string outputFile) {
            log.TraceFormat("Saved output file to: {0}", outputFile);
        }

        public static void LogCompilerErrors(this IHxlcLogger log, HxlCompilerErrorCollection errors) {
            foreach (var e in errors) {
                var str = e.ToString();
                if (e.IsWarning)
                    log.Warn(str);
                else
                    log.Error(str);
            }
        }

        public static void ParsingTemplate(this IHxlcLogger log, string file) {
            log.DebugFormat(file);
        }
    }
}

