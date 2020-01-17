//
// - ExpressionBuffer.cs -
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
using System.Linq;
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Hxl.Compiler {

    internal class ExpressionBuffer : IHxlTemplateEmitter {

        readonly IList<Expression> _parts = new List<Expression>();

        public IList<Expression> Parts {
            get {
                return _parts;
            }
        }

        void IHxlTemplateEmitter.EmitCode(string code) {
            // Unused by expression expander
        }

        void IHxlTemplateEmitter.EmitLiteral(string text) {
            if (string.IsNullOrEmpty(text))
                return;

            _parts.Add(Expression.Constant(text));
        }

        void IHxlTemplateEmitter.EmitValue(Expression expr) {
            _parts.Add(expr);
        }
    }
}


