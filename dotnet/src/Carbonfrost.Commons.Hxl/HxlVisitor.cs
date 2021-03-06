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

namespace Carbonfrost.Commons.Hxl {

    public abstract class HxlVisitor : DomNodeVisitor, IHxlVisitor {

        protected override void VisitProcessingInstruction(DomProcessingInstruction instruction) {
            DefaultVisit(instruction);
        }

        protected override void VisitElement(DomElement element) {
            DefaultVisit(element);
        }

        protected virtual void VisitAttributeFragment(HxlAttribute fragment) {
            DefaultVisit(fragment);
        }

        protected virtual void VisitProcessingInstructionFragment(HxlProcessingInstruction fragment) {
            DefaultVisit(fragment);
        }

        protected virtual void VisitElementFragment(HxlElement fragment) {
            DefaultVisit(fragment);
        }

        void IHxlVisitor.Visit(HxlAttribute attribute) {
            VisitAttributeFragment(attribute);
        }

        void IHxlVisitor.Visit(HxlElement element) {
            VisitElementFragment(element);
        }

        void IHxlVisitor.Visit(HxlProcessingInstruction macro) {
            VisitProcessingInstructionFragment(macro);
        }
    }

}
