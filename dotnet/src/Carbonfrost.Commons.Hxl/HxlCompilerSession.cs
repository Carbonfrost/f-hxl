//
// - HxlCompilerSession.cs -
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl {

    class HxlCompilerSession {

        static readonly string WorkDirectory = Path.Combine(Path.GetTempPath(), "ff-hxl");
        private readonly string TemporaryDirectory;
        public readonly string SessionID;
        public readonly HxlCompilerSettings Settings;

        private bool createdDirectory;

        public readonly ICollection<Assembly> ImplicitAssemblyReferences
            = new HashSet<Assembly>();

        public HxlCompilerSession(HxlCompilerSettings settings) {
            var sessionID = Utility.RandomID();
            this.TemporaryDirectory = Path.Combine(WorkDirectory, sessionID);
            this.SessionID = sessionID;
            this.Settings = settings;
        }

        public string GetFileName(string name) {
            if (!createdDirectory) {
                Directory.CreateDirectory(this.TemporaryDirectory);
                createdDirectory = true;
            }
            return Path.Combine(TemporaryDirectory, name);
        }

        public TextWriter CreateText(string tempFile) {
            return File.CreateText(GetFileName(tempFile));
        }
    }


}
