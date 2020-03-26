//
// - HxlNodeConverterTestBase.cs -
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
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Spec;

namespace Carbonfrost.UnitTests.Hxl.Compiler {

    public abstract class HxlNodeConverterTestBase {

        internal static DomObject ConvertNode(DomObject attr) {
            var __document = new HxlDocument();
            DomElement root_article = __document.CreateElement("article");
            DomObject root_article_hxlexpressionattribute = global::Carbonfrost.Commons.Hxl.HxlAttribute.Create("id", (__closure, __self__) => string.Concat((object) (__closure.Inside)));
            root_article.Append(root_article_hxlexpressionattribute);

            var conv = HxlCompilerConverter.ChooseConverter(attr);
            return conv.Convert(attr, CSharpScriptGenerator.Instance);
        }

    }
}
