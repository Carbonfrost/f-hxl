//
// - HxlLanguageVisitor.cs -
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

namespace Carbonfrost.Commons.Hxl.Compiler {

    public abstract class HxlLanguageVisitor : NodeFragmentVisitor, IHxlLanguageVisitor {

        protected virtual void DefaultVisit(HxlLangElement element) {
        }

        protected virtual void DefaultVisit(HxlLangAttribute attribute) {
        }

        protected virtual void VisitCase(HxlCaseAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitCase(HxlCaseElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitOtherwise(HxlOtherwiseAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitOtherwise(HxlOtherwiseElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitWhen(HxlWhenAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitWhen(HxlWhenElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitDirective(HxlDirective directive) {
            DefaultVisit(directive);
        }

        protected virtual void VisitIf(HxlIfElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitIf(HxlIfAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitValue(HxlValueElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitText(HxlTextElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitText(HxlTextAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitUnless(HxlUnlessElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitUnless(HxlUnlessAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitForEach(HxlForEachElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitForEach(HxlForEachAttribute element) {
            DefaultVisit(element);
        }

        protected virtual void VisitFor(HxlForElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitFor(HxlForAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitRunAt(HxlRunAtAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitRender(HxlRenderAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitSpace(HxlSpaceAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitModel(HxlModelElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitModel(HxlModelAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitTemplate(HxlTAttribute attribute) {
            DefaultVisit(attribute);
        }

        protected virtual void VisitComment(HxlCommentElement element) {
            DefaultVisit(element);
        }

        void IHxlLanguageVisitor.Visit(HxlIfElement element) {
            VisitIf(element);
        }

        void IHxlLanguageVisitor.Visit(HxlValueElement element) {
            VisitValue(element);
        }

        void IHxlLanguageVisitor.Visit(HxlTextElement element) {
            VisitText(element);
        }

        void IHxlLanguageVisitor.Visit(HxlUnlessElement element) {
            VisitUnless(element);
        }

        void IHxlLanguageVisitor.Visit(HxlForEachElement element) {
            VisitForEach(element);
        }

        void IHxlLanguageVisitor.Visit(HxlForElement element) {
            VisitFor(element);
        }

        void IHxlLanguageVisitor.Visit(HxlIfAttribute attribute) {
            VisitIf(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlUnlessAttribute attribute) {
            VisitUnless(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlForEachAttribute attribute) {
            VisitForEach(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlForAttribute attribute) {
            VisitFor(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlRunAtAttribute attribute) {
            VisitRunAt(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlRenderAttribute attribute) {
            VisitRender(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlSpaceAttribute attribute) {
            VisitSpace(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlModelElement element) {
            VisitModel(element);
        }

        void IHxlLanguageVisitor.Visit(HxlModelAttribute attribute) {
            VisitModel(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlCommentElement element) {
            VisitComment(element);
        }

        void IHxlLanguageVisitor.Visit(HxlDirective directive) {
            VisitDirective(directive);
        }

        void IHxlLanguageVisitor.Visit(HxlTextAttribute attribute) {
            VisitText(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlCaseElement element) {
            VisitCase(element);
        }

        void IHxlLanguageVisitor.Visit(HxlWhenElement element) {
            VisitWhen(element);
        }

        void IHxlLanguageVisitor.Visit(HxlOtherwiseElement element) {
            VisitOtherwise(element);
        }

        void IHxlLanguageVisitor.Visit(HxlCaseAttribute attribute) {
            VisitCase(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlWhenAttribute attribute) {
            VisitWhen(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlOtherwiseAttribute attribute) {
            VisitOtherwise(attribute);
        }

        void IHxlLanguageVisitor.Visit(HxlTAttribute attribute) {
            VisitTemplate(attribute);
        }
    }
}
