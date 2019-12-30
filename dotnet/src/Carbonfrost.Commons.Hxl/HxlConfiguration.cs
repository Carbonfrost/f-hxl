//
// Copyright 2013, 2019 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class HxlConfiguration {

        private readonly HxlCompilerSettings compiler = DefaultCompilerSettings();
        private readonly PropertyProviderCollection _globalDataProviders = DefaultDataProviders();

        public HxlCompilerSettings Compiler {
            get {
                return compiler;
            }
        }

        public PropertyProviderCollection GlobalDataProviders {
            get {
                return _globalDataProviders;
            }
        }

        public static HxlConfiguration Current {
            get {
                // UNDONE Support configuration loader
                return _current ?? (_current = new HxlConfiguration());
            }
        }

        private static HxlConfiguration _current;

        // TODO These should probably be init from machine universal config

        static HxlCompilerSettings DefaultCompilerSettings() {
            var result = new HxlCompilerSettings(null);
            result.Assemblies.AddMany(HxlAssemblyCollection.System);
            result.Assemblies.AddMany(HxlAssemblyCollection.Hxl);

            var globalFactory = DomNodeFactory.Compose(
                HxlDomNodeFactory.Compiler,
                new ProviderDomNodeFactory());

            result.NodeFactories.AddNew("global", globalFactory);

            return result;
        }

        static PropertyProviderCollection DefaultDataProviders() {
            var pc = new PropertyProviderCollection();
            pc.AddNew("global", new HxlGlobalFunctions());
            return pc;
        }
    }

}
