//
// - OptimizeRenderIslands.cs -
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
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    static class OptimizeRenderIslands {

        // TODO Optimize adjacent text nodes so that multiple calls to write aren't needed
        static readonly ICompilerDomOptimization[] OPTIMIZATIONS =  {
            ReduceRenderFragmentOptimization.Instance,
            ReduceAnyFragmentOptimization.Instance,
            // RemoveWhitespaceOptimization.Instance,
        };

        public static void Rewrite(DomContainer document) {
            ProcessElement(document);
        }

        static void ProcessElement(DomNode item) {
            ConsiderChildren(item);

            foreach (var opt in OPTIMIZATIONS) {
                if (!opt.Optimize(item))
                    break;
            }
        }

        static void ConsiderChildren(DomNode item) {
            if (item.ChildNodes.Count == 0)
                return;

            // TODO Wasteful to copy into an array (performance)
            foreach (var m in item.ChildNodes.ToArray())
                ProcessElement(m);
        }
    }
}
