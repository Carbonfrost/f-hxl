//
// Copyright 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Reflection;
using System.Xml;

using Microsoft.CSharp.RuntimeBinder;

using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;
using System.IO;

namespace Carbonfrost.Commons.Hxl.Compiler {

    partial class HxlAssemblyCollection {

        internal static readonly HxlAssemblyCollection System
            = new HxlAssemblyCollection();
        internal static readonly HxlAssemblyCollection Hxl
            = new HxlAssemblyCollection();

        static readonly Type[] NEED_TYPES = {
            typeof(object),
            typeof(Enumerable),
            typeof(Uri),
            typeof(XmlWriter),
            typeof(RuntimeBinderException),
        };

        static readonly Type[] NEED_HXL_TYPES = {
            typeof(ElementFragment),
            typeof(Activation),
            typeof(DomElement),
        };

        // TODO We add assemblies based on the runtime of this rather
        // that the desired target framework of the output

        static HxlAssemblyCollection() {
            foreach (var t in NEED_HXL_TYPES) {
                Hxl.Add(t.GetTypeInfo().Assembly);
            }
            try {
                TryLoadDefaults();
            } catch {
            }
        }

        static void TryLoadDefaults() {
#if NET
            var assemblies = NEED_TYPES.Select(t => t.GetTypeInfo().Assembly);
            foreach (var asm in assemblies.Distinct()) {
                System.Add(asm);
            }
#else
            // In netcore, we need a generic list
            // TODO Support Windows here, more robust support for directories
            string home = Environment.GetEnvironmentVariable("HOME");

            System.AddNewFile(
                Path.Combine(home, ".nuget/packages/NETStandard.Library/2.0.3/build/netstandard2.0/ref/netstandard.dll")
            );

            // TODO Contains TimeZoneInfo, which is in use by a test, but it isn't clear that this is a good
            // default reference for .NET Standard/2.0
            System.AddNewFile(
                Path.Combine(home, ".nuget/packages/NETStandard.Library/2.0.3/build/netstandard2.0/ref/System.Runtime.dll")
            );
            System.AddNewFile(
                Path.Combine(home, ".nuget/packages/Microsoft.CSharp/4.7.0/ref/netstandard2.0/Microsoft.CSharp.dll")
            );
#endif

        }
    }
}
