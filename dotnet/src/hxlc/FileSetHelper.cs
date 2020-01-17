//
// - FileSetHelper.cs -
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
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class FileSetHelper {

        readonly string[] _implicitSearchPatterns;
        readonly List<string> _items = new List<string>();

        public FileSetHelper(params string[] implicitSearchPatterns) {
            this._implicitSearchPatterns = implicitSearchPatterns;
        }

        public void Add(string glob) {
            _items.Add(glob);
        }

        public IEnumerable<string> EnumerateFilePaths() {
            return EnumerateFiles().Select(t => t.File);
        }

        public IEnumerable<FileSetHelperItem> EnumerateFiles() {
            var results = new List<FileSetHelperItem>();

            foreach (var glob in _items) {
                ProcessOneGlob(glob, results);
            }

            // TODO Special handling:
            // - if *.* is used, then assume *.html, hxl, htxl
            // - if directory names are used then assume **/*.html
            // - assume no inputs means default
            return results;
        }

        private void ProcessOneGlob(string glob, List<FileSetHelperItem> results) {
            if (!IsGlobLiteral(glob)) {
                var files = Glob.Parse(glob).EnumerateFiles();

                results.AddRange(files.Select(f => new FileSetHelperItem(f, glob)));
                return;
            }

            if (Directory.Exists(glob)) {
                foreach (var im in _implicitSearchPatterns) {
                    var files = Directory.EnumerateFiles(glob, im, SearchOption.AllDirectories);
                    results.AddRange(files.Select(f => new FileSetHelperItem(f, glob)));
                }

            } else if (File.Exists(glob)) {
                results.Add(new FileSetHelperItem(glob, glob));

            } else {
                // TODO Warn on missing files when a literal
                throw new NotImplementedException("missing: " + glob);
            }
        }

        private static bool IsGlobLiteral(string glob) {
            return glob.IndexOfAny(new char[] { '*', ';' }) < 0;
        }

    }

}
