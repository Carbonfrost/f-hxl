//
// - ProgramOptions.cs -
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
using Carbonfrost.Commons.Core.Runtime;
using NDesk.Options;
using Hxlc.Resources;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class ProgramOptions {

        private readonly IDictionary<string, string> _defines = new Dictionary<string, string>();
        private readonly IDictionary<string, string> _namespaces = new Dictionary<string, string>();

        public LogoType ShowLogo { get; set; }
        public string OutputFile { get; set; }
        public TypeReference BaseType { get; set; }
        public bool NoCompile { get; set; }
        public HxlcTraceLevel TraceLevel { get; set; }

        public IDictionary<string, string> Namespaces {
            get {
                return _namespaces;
            }
        }
        public IDictionary<string, string> Defines {
            get {
                return _defines;
            }
        }

        public readonly FileSetHelper References = new FileSetHelper("*.dll");
        public readonly FileSetHelper Inputs = new FileSetHelper("*.hxl", "*.htxl");

        public bool Web;
        public bool NoTemplateFactory; // corresponds to EmitTemplateFactory

        // TODO Encapsulate some of the compiler parameters here (compling directly to assembly)
        // TODO Allow disabling optimization
        // TODO Allow defining namespace prefix=http://ns.carbonfrost.com/

        readonly OptionSet OptionSet;

        public ProgramOptions() {
            // TODO Implement verbosity -v -vv, etc.
            // TODO Localizations
            // TODO Support other csc style options
            this.OptionSet = new OptionSet {
                { "version",      SR.UVersion(),       v => ShowLogo = LogoType.LogoExtendedVersion },
                { "?|help",       SR.UHelp(),          v => ShowLogo = LogoType.Usage },
                { "out=",         SR.UOut(),           v => OutputFile = v },
                { "no-compile",   SR.UNoCompile(),     v => NoCompile = true },
                { "verbose",      SR.UDebug(),         v => TraceLevel = HxlcTraceLevel.Trace },
                { "debug",        SR.UDebug(),         v => TraceLevel = HxlcTraceLevel.Debug },
                { "base-type=",   SR.UBaseType(),      v => BaseType = TypeReference.Parse(v) },
                { "include=",     SR.UInclude(),       v => Inputs.Add(v) },
                { "N|namespace:=",SR.UNamespace(),(k, v) => Namespaces.Add(k, v) },
                { "D|define:=",   SR.UDefine(),   (k, v) => Defines.Add(k, v) },
                { "r|reference=", SR.UReference(),     v => References.Add(v) },
            };
        }

        public List<string> Parse(string[] args) {
            return this.OptionSet.Parse(args);
        }

        public void Usage() {
            Console.WriteLine("Usage:  hxlc [OPTION]... [FILE]...");
            Console.WriteLine("Compile HXL files into assemblies or generate C# source.");
            Console.WriteLine();

            OptionSet.WriteOptionDescriptions(Console.Out);
        }

        public enum LogoType {
            Logo,
            None,
            LogoExtendedVersion,
            Usage,
        }
    }
}

