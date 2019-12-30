//
// - ITemplateCodeGenerator.cs -
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

namespace Carbonfrost.Commons.Hxl.Compiler {

    interface ITemplateCodeGenerator {

        // TODO These are code blocks - it should be a list of some sort
        string RenderIslands { get; set; }
        string InitializeComponent { get; set; }

        IList<string> DomNodeVariables { get; set; }
        IList<string> DomObjectVariables { get; set; }
        string Namespace { get; set; }

        string TemplateName { get; set; }
        string ClassName { get; set; }

        string BaseClass { get; set; }
        string ModelType { get; set; }

        string CodeGenerator { get; }
        string CodeGeneratorVersion { get; }

        IList<string> Usings { get; }

    }

}
