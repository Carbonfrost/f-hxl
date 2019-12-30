//
// - CSTemplateGenerator.cs -
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
using System.Reflection;

namespace Carbonfrost.Commons.Hxl.Compiler {

    partial class CSTemplateGenerator : ITemplateCodeGenerator {

        private readonly List<string> _usings = new List<string>();

        public string RenderIslands {
            get;
            set;
        }

        public string InitializeComponent {
            get;
            set;
        }

        public IList<string> DomObjectVariables {
            get;
            set;
        }

        public IList<string> DomNodeVariables {
            get;
            set;
        }

        public string Namespace {
            get;
            set;
        }

        public string TemplateName {
            get;
            set;
        }

        public string TransformTextCore {
            get;
            set;
        }

        public string ClassName {
            get;
            set;
        }

        public string BaseClass {
            get;
            set;
        }

        public string ModelType {
            get;
            set;
        }

        public string CodeGenerator {
            get {
                return "HxlCompiler";
            }
        }

        public bool HasDocument { get; set; }

        public string CodeGeneratorVersion {
            get {
                return GetType().GetTypeInfo().Assembly.GetName().Version.ToString();
            }
        }

        // TODO Allow emiting a file directive in generated CS source for each using
        public IList<string> Usings {
            get {
                return _usings;
            }
        }

        public CSTemplateGenerator() {
            DomNodeVariables = Empty<string>.List;
            DomObjectVariables = Empty<string>.List;
            RenderIslands = string.Empty;
            InitializeComponent = string.Empty;
            TransformTextCore = string.Empty;
        }

    }
}
