//
// - HxlUsingDirective.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Hxl.Compiler;

namespace Carbonfrost.Commons.Hxl.Compiler {

    [DirectiveUsage(Name = "using")]
    public class HxlUsingDirective : HxlDirective {

        public string Namespace { get; set; }
        public AssemblyName Assembly { get; set; }

        public HxlUsingDirective() : base("using") {}

        protected override void Preprocess(IServiceProvider serviceProvider) {
            var builder = serviceProvider.GetRequiredService<IHxlTemplateBuilder>();
            // TODO Should be one or the other
            if (!string.IsNullOrEmpty(Namespace))
                builder.Imports.Add(Namespace);

            if (Assembly != null)
                builder.AssemblyReferences.Add(Assembly);
        }

    }

}
