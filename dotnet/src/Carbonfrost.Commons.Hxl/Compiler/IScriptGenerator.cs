//
// Copyright 2014, 2016 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Hxl.Compiler {

    interface IScriptGenerator {
        IEnumerable<string> Start(HxlForEachElement e);
        IEnumerable<string> End(HxlForEachElement e);

        IEnumerable<string> Start(HxlForElement e);
        IEnumerable<string> End(HxlForElement e);

        IEnumerable<string> Start(HxlValueElement e);
        IEnumerable<string> End(HxlValueElement e);

        IEnumerable<string> Start(HxlTextElement e);
        IEnumerable<string> End(HxlTextElement e);

        IEnumerable<string> Start(HxlModelElement e);
        IEnumerable<string> End(HxlModelElement e);

        IEnumerable<string> Start(HxlCommentElement e);
        IEnumerable<string> End(HxlCommentElement e);

        IEnumerable<string> Start(HxlCaseElement e);
        IEnumerable<string> End(HxlCaseElement e);

        IEnumerable<string> Start(HxlWhenElement e);
        IEnumerable<string> End(HxlWhenElement e);

        IEnumerable<string> Start(HxlOtherwiseElement e);
        IEnumerable<string> End(HxlOtherwiseElement e);

        IEnumerable<string> Start(HxlRootElement e);
        IEnumerable<string> End(HxlRootElement e);
    }

}
