//
// Copyright 2013, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public interface IHxlLanguageVisitor : IDomNodeVisitor {

        void Visit(HxlIfElement element);
        void Visit(HxlUnlessElement element);
        void Visit(HxlForEachElement element);
        void Visit(HxlForElement element);
        void Visit(HxlCaseElement element);
        void Visit(HxlWhenElement element);
        void Visit(HxlOtherwiseElement element);
        void Visit(HxlModelElement element);
        void Visit(HxlValueElement element);
        void Visit(HxlTextElement element);
        void Visit(HxlCommentElement element);

        void Visit(HxlTAttribute attribute);
        void Visit(HxlRenderAttribute attribute);
        void Visit(HxlRunAtAttribute attribute);
        void Visit(HxlTextAttribute attribute);
        void Visit(HxlSpaceAttribute attribute);
        void Visit(HxlModelAttribute attribute);
        void Visit(HxlIfAttribute attribute);
        void Visit(HxlUnlessAttribute attribute);
        void Visit(HxlForEachAttribute attribute);
        void Visit(HxlForAttribute attribute);
        void Visit(HxlCaseAttribute attribute);
        void Visit(HxlWhenAttribute attribute);
        void Visit(HxlOtherwiseAttribute attribute);

        void Visit(HxlDirective directive);
    }
}
