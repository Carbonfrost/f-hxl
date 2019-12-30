//
// - HxlMasterInfo.cs -
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
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    class HxlMasterInfo {

        private readonly DomElement _layoutElement;
        private readonly IHxlPlaceholderContentProvider _placeholderContent;
        private readonly HxlTemplateContext _applyingTo;
        private readonly string _layoutName;

        public bool InSpa { get; set; }

        public string LayoutName {
            get {
                return _layoutName;
            }
        }

        public IHxlPlaceholderContentProvider PlaceholderContent {
            get {
                return _placeholderContent;
            }
        }

        public HxlMasterInfo(IHxlPlaceholderContentProvider placeholderContent,
                             HxlTemplateContext applyingTo,
                             DomElement mergeAttributes,
                             string layoutName)
        {
            _placeholderContent = placeholderContent;
            _applyingTo = applyingTo;
            _layoutElement = mergeAttributes;
            _layoutName = layoutName;
        }

        public DomElement HeadElement {
            get {
                if (_layoutElement.Name == "html") {
                    return _layoutElement.Element("head");
                }
                return null;
            }
        }

        public DomElement LayoutElement {
            get {
                return _layoutElement;
            }
        }
    }
}
