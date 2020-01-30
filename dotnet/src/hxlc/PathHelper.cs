//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.IO;
using System.Linq;

namespace Carbonfrost.Commons.Hxl.Compiler {

    static class PathHelper {

        public static string UnixPath(string str) {
            return str.Replace("\\", "/");
        }

        private static string EnsureTrailingSlash(string fileSpec) {
            if ((fileSpec.Length > 0) && !EndsWithSlash(fileSpec)) {
                fileSpec = fileSpec + Path.DirectorySeparatorChar;
            }
            return fileSpec;
        }

        private static bool EndsWithSlash(string fileSpec) {
            if (string.IsNullOrEmpty(fileSpec))
                return false;

            return IsSlash(fileSpec[fileSpec.Length - 1]);
        }

        private static bool IsSlash(char c) {
            return c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
        }

        private static Uri CreateUriFromPath(string path) {
            Uri result = null;
            if (!Uri.TryCreate(path, UriKind.Absolute, out result)) {
                result = new Uri(path, UriKind.Relative);
            }
            return result;
        }

        private static Uri EnsureAbsoluteFile(string file) {
            if (file[0] == '/') {
                return new Uri("file://" + file, UriKind.Absolute);
            }
            return new Uri("file://" + Path.GetFullPath(file), UriKind.Absolute);
        }

        internal static string MakeRelative(string basePath, string path) {
            if (string.IsNullOrEmpty(basePath)) {
                return path;
            }

            Uri baseUri = EnsureAbsoluteFile(EnsureTrailingSlash(basePath));
            Uri relativeUri = CreateUriFromPath(path);

            if (!relativeUri.IsAbsoluteUri) {
                relativeUri = new Uri(baseUri, relativeUri);
            }

            Uri result = baseUri.MakeRelativeUri(relativeUri);
            return Uri.UnescapeDataString(result.IsAbsoluteUri ? result.LocalPath : result.ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }
    }
}
