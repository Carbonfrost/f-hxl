//
// - ModelDirective.cs -
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
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl.Compiler;

namespace Carbonfrost.Commons.Hxl.Compiler {

    [DirectiveUsage(Name = "model")]
    public class HxlModelDirective : HxlDirective {

        // TODO Consider supporting named models

        public HxlModelDirective() : base("model") {}

        public TypeReference Type {
            get; set;
        }

        protected override void Preprocess(IServiceProvider serviceProvider) {
            var builder = serviceProvider.GetRequiredService<IHxlTemplateBuilder>();
            builder.ModelType = Type;
        }

        protected override void EmitCode(IHxlTemplateEmitter emitter) {
            string text = string.Format("{0} __model = ({0}) this.Data.Model;", Type);
            emitter.EmitCode(text);
        }
    }
}
