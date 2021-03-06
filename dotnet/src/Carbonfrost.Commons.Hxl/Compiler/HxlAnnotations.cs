//
// - HxlAnnotations.cs -
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
using System.Linq;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    static class HxlAnnotations {

        public static object Retained = new RetainedImpl();

        public static bool IsRetained(this DomNode node) {
            return node.HasAnnotation(HxlAnnotations.Retained);
        }

        public static void Retain(this DomObject element) {
            element.AddAnnotation(HxlAnnotations.Retained);
        }

        class RetainedImpl
        {
        }
    }
}
