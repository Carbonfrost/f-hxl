//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class HxlCompilerReferencePath : IHxlCompilerReferencePath {

        private static readonly Assembly WEBDOM_ASM = typeof(DomElement).Assembly;
        private static readonly Assembly HXL_ASM = typeof(HxlElement).Assembly;

        private readonly HashSet<Type> _implicit = new HashSet<Type>();
        private readonly HashSet<Assembly> _implicitAsm = new HashSet<Assembly>();

        public IReadOnlyCollection<Type> ImplicitTypeUses {
            get {
                return _implicit;
            }
        }

        public IReadOnlyCollection<Assembly> ImplicitAssemblyReferences {
            get {
                return _implicitAsm;
            }
        }

        public void AddImplicitTypeUse(Type type) {
            // Certain types are not ever needed implicitly
            if (type.Assembly == WEBDOM_ASM) {
                return;
            }
            if (type.Assembly == HXL_ASM) {
                return;
            }
            _implicit.Add(type);
            _implicitAsm.Add(type.Assembly);
        }
    }
}
