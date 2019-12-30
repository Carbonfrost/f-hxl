//
// - HxlCompilerResults.cs -
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

namespace Carbonfrost.Commons.Hxl {

    public class HxlCompilerResults {

        private readonly IHxlInternalCompilerResults _nativeCompilerResults;
        private readonly HxlCompiledTemplateInfoCollection _compiledTemplates;

        internal IHxlInternalCompilerResults NativeCompilerResults {
            get {
                return _nativeCompilerResults;
            }
        }

        internal HxlCompilerResults(IHxlInternalCompilerResults nativeCompilerResults) {
            if (nativeCompilerResults == null)
                throw new ArgumentNullException("nativeCompilerResults");

            _nativeCompilerResults = nativeCompilerResults;

            Assembly asm = null;
            try {
                asm = nativeCompilerResults.CompiledAssembly;
            } catch (FileNotFoundException) {
            }

            if (asm == null)
                _compiledTemplates = HxlCompiledTemplateInfoCollection.Empty;
            else
                _compiledTemplates = new HxlCompiledTemplateInfoCollection(asm);

        }

        public HxlCompiledTemplateInfoCollection CompiledTemplates {
            get {
                return _compiledTemplates;
            }
        }

        public Assembly CompiledAssembly {
            get {
                return _nativeCompilerResults.CompiledAssembly;
            }
        }

        public HxlCompilerErrorCollection Errors {
            get {
                return _nativeCompilerResults.Errors;
            }
        }

        public int NativeCompilerReturnValue {
            get {
                return _nativeCompilerResults.NativeCompilerReturnValue;
            }
        }

        public IList<string> Output {
            get {
                return _nativeCompilerResults.Output;
            }
        }

        public string PathToAssembly {
            get {
                return _nativeCompilerResults.PathToAssembly;
            }
        }
    }
}
