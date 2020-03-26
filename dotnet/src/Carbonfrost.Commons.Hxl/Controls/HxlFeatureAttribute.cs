//
// - HxlFeatureAttribute.cs -
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
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.Commons.Hxl.Controls {

    public class HxlFeatureAttribute : HxlAttribute {

        [Value]
        public string Feature {
            get;
            set;
        }

        protected override IHxlElementTemplate OnElementRendering() {
            object myFeatures = null;

            try {
                myFeatures = ((dynamic) TemplateContext).Features;

            } catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) {
            }

            DomStringTokenList features = ConvertFeatures(myFeatures);

            if (features != null && features.Contains(Feature))
                return null;

            return HxlElementTemplate.Skip;
        }

        static DomStringTokenList ConvertFeatures(object myFeatures) {
            if (myFeatures == null)
                return null;

            var features = myFeatures as DomStringTokenList;
            if (features != null)
                return features;

            string text = myFeatures as string;
            if (text != null &&
                DomStringTokenList.TryParse(text, out features))
                return features;

            return null;
        }
    }
}
