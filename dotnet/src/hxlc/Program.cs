//
// Copyright 2014, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl.Compiler {

    static class Program {

        public static int Main(string[] args) {
            HxlCompiler.CommandLineHost = true;
            ProgramOptions options = new ProgramOptions();

            var unknown = options.Parse(args);
            int result = 0;

            // TODO Allow specifying src files directly as positional

            string unrecognized;
            IEnumerable<string> fileNames;
            if (!ParseArguments(unknown, out fileNames, out unrecognized)) {
                Logo(false);
                Console.WriteLine("hxlc: Unrecognized option: " + unrecognized);
                options.Usage();
                result = 2;

            } else if (ShowLogo(options)) {
                foreach (var fn in fileNames) {
                    options.Inputs.Add(fn);
                }
                var app = new HxlcApp(options);
                result = app.Run();
            }

            if (Debugger.IsAttached) {
                Console.WriteLine("Press any key to exit . . .");
                Console.ReadKey();
            }
            return result;
        }

        static bool ParseArguments(IList<string> unknowns,
                                   out IEnumerable<string> fileNames,
                                   out string unrecognized) {

            unrecognized = null;
            fileNames = Enumerable.Empty<string>();

            if (unknowns.Count == 0)
                return true;

            // Determine if there is an unknown option --example
            if (!VerifyNotOptions(unknowns, out unrecognized))
                return false;

            fileNames = unknowns;
            return true;
        }

        static bool VerifyNotOptions(IList<string> unknowns, out string unrecognized) {
            unrecognized = null;

            foreach (string s in unknowns) {
                if (s.StartsWith("-", StringComparison.Ordinal)) {
                    unrecognized = s;
                    return false;
                }
            }

            return true;
        }

        // Returns true if program should run
        static bool ShowLogo(ProgramOptions options) {
            switch (options.ShowLogo) {
                case ProgramOptions.LogoType.Logo:
                    Logo(false);
                    return true;

                case ProgramOptions.LogoType.None:
                    return true;

                case ProgramOptions.LogoType.LogoExtendedVersion:
                    Logo(true);
                    return false;

                case ProgramOptions.LogoType.Usage:
                default:
                    Logo(false);
                    options.Usage();
                    return false;
            }
        }

        private static DateTime? GetBuildDate() {
            foreach (AssemblyMetadataAttribute meta in typeof(Program).GetTypeInfo().Assembly.GetCustomAttributes(typeof(AssemblyMetadataAttribute))) {
                if (meta.Key == "[share:BuildDate]") {
                    if (DateTime.TryParse(meta.Value, out DateTime result)) {
                        return result;
                    }
                    return null;
                }
            }
            return null;
        }

        static void Logo(bool longVersion) {
            var asm = typeof(Program).GetTypeInfo().Assembly;
            string version = asm.GetName().Version.ToString();

            var fva = (AssemblyFileVersionAttribute) Attribute.GetCustomAttribute(asm, typeof(AssemblyFileVersionAttribute));
            if (fva != null) {
                version = fva.Version;
            }

            string sku = ".NET Framework";
            if (Platform.Current.IsMono)
                sku = "Mono";

            string skuVersion = RuntimeEnvironment.GetSystemVersion();
            if (longVersion) {
                var attr = (AssemblyInformationalVersionAttribute) asm.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute));
                string infoVersion = attr.InformationalVersion;
                version += " - " + infoVersion;
            }

            string registered = " (R)";
            string copy = "(c)";

            if (Environment.OSVersion.Platform == PlatformID.Unix) {
                registered = "®";
                copy = "©";
            }
            DateTime buildDate = GetBuildDate().GetValueOrDefault(new DateTime(2020, 1, 1));
            Console.WriteLine("Carbonfrost{1} HXL Compiler version {0}", version, registered);
            if (longVersion) {
                Console.WriteLine("[{0}, version {1}]", sku, skuVersion);
            }
            Console.WriteLine("Copyright {0} 2015-{1:yyyy} Carbonfrost Systems, Inc. All rights reserved.", copy, buildDate);
            Console.WriteLine();
        }
    }

}
