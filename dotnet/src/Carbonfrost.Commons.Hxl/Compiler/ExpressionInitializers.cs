//
// - ExpressionInitializers.cs -
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
using System.Reflection;
using System.Linq;
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class ExpressionInitializers {

        public readonly Expression Expression;
        public readonly PropertyInfo Member;

        public ExpressionInitializers(PropertyInfo member, Expression expression) {
            if (member == null)
                throw new ArgumentNullException("member");
            if (expression == null)
                throw new ArgumentNullException("expression");

            this.Expression = expression;
            this.Member = member;
        }
    }
}
