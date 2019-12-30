//
// - FutureFeatures.cs -
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

namespace Carbonfrost.Commons.Hxl {

    static class FutureFeatures {

        public static NotImplementedException InterpretedLanguageElements() {
            return new NotImplementedException("Interpreter not implemented.  Please compile and run HXL using generated source.");
        }

        public static NotImplementedException CompileAndEmit() {
            return new NotImplementedException("Emitting a type directly is not implemented.  Please generate source from HXL and compile.");
        }

        public static NotImplementedException ExpressionNSSyntax() {
            // TODO: Syntax such as ${h + shared::Glob.Parse(), for example
            return new NotImplementedException("Expressions can't contain the namespace syntax.");
        }

    }
}
