//
// - TemplateDirective.cs -
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
using Carbonfrost.Commons.Hxl.Compiler;

namespace Carbonfrost.Commons.Hxl.Compiler {

    [DirectiveUsage(Name = "template")]
    public class HxlTemplateDirective : HxlDirective {

        public string Namespace {
            get;
            set;
        }

        public string Class {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public HxlTemplateDirective() : base("template") {}

        protected override void Preprocess(IHxlTemplateBuilder builder) {
            builder.Namespace = Namespace;
            builder.ClassName = Class;
            builder.TemplateName = Name;
        }

    }
}

