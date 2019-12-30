//
// - HxlPlaceholderTargetAttribute.cs -
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
using System.ComponentModel;
using System.Linq;
using Carbonfrost.Commons.Hxl;
using Carbonfrost.Commons.Core.Runtime.Expressions;

namespace Carbonfrost.Commons.Hxl.Controls {

    public class HxlPlaceholderTargetAttribute : AttributeFragment {

        private string _placeholder;

        // TODO Add name validation, validate decorator

        [Value]
        public string Placeholder {
            get {
                return HxlPlaceholderContentProvider.GetImpliedPlaceholderName(OwnerElement, _placeholder);
            }
            set {
                _placeholder = value;
            }
        }

        [ExpressionSerializationMode(ExpressionSerializationMode.Hidden)]
        public bool Async {
            get { return Options.HasFlag(PlaceholderTargetOptions.Async); }
            set { Options = Options.SetFlag(PlaceholderTargetOptions.Async, value); }
        }

        [ExpressionSerializationMode(ExpressionSerializationMode.Hidden)]
        public bool Append {
            get { return Options.HasFlag(PlaceholderTargetOptions.Append); }
            set { Options = Options.SetFlag(PlaceholderTargetOptions.Append, value); }
        }

        [ExpressionSerializationMode(ExpressionSerializationMode.Hidden)]
        public bool DisableSinglePage {
            get { return Options.HasFlag(PlaceholderTargetOptions.DisableSinglePage); }
            set { Options = Options.SetFlag(PlaceholderTargetOptions.DisableSinglePage, value); }
        }

        public PlaceholderTargetOptions Options { get; set; }

        public string Location { get; set; }

        public HxlPlaceholderTargetAttribute()
            : base("hxl:placeholdertarget") {}

        // TODO Support implicit use within a page

        // TODO Shouldn't need to redeclare placeholders in a nested
        // context (they should inherit automatically, possibly have
        // some policy that controls it)

        protected override IElementTemplate OnElementRendering() {
            if (DoesMasterContainPlaceholder()) {
                return ElementTemplate.Skip;
            }
            else {
                return ElementTemplate.Default;
            }
        }

        private bool DoesMasterContainPlaceholder() {
            if (TemplateContext.FindPlaceholderContent(Placeholder) != null)
                return true;
            else
                return false;
        }
    }
}
