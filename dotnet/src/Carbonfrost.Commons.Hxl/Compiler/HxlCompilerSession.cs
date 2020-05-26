//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class HxlCompilerSession {

        static readonly string _workDirectory = Path.Combine(Path.GetTempPath(), "f-hxl");
        private readonly string _temporaryDirectory;
        private bool _createdDirectory;

        public readonly string SessionID;
        public readonly HxlCompilerSettings Settings;

        public readonly ICollection<Assembly> ImplicitAssemblyReferences = new HashSet<Assembly>();

        public HxlCompilerSession(HxlCompilerSettings settings) {
            var sessionID = Utility.RandomID();
            _temporaryDirectory = Path.Combine(_workDirectory, sessionID);
            SessionID = sessionID;
            Settings = settings;
        }

        public string GetFileName(string name) {
            if (!_createdDirectory) {
                Directory.CreateDirectory(_temporaryDirectory);
                _createdDirectory = true;
            }
            return Path.Combine(_temporaryDirectory, name);
        }

        public TextWriter CreateText(string tempFile) {
            return File.CreateText(GetFileName(tempFile));
        }
    }
}
