//
// - IHxlRenderingProvider.cs -
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
using System.IO;
using System.Linq;

namespace Carbonfrost.Commons.Hxl {

    public interface IHxlRenderingProvider {

        void RenderAction(TextWriter output,
                          string action,
                          string controller,
                          ElementTemplateInfo templateInfo,
                          ElementFragment body,
                          IDictionary<string, object> routeValues);

        void RenderPartial(TextWriter output,
                           string partialViewName,
                           ElementTemplateInfo templateInfo,
                           object model,
                           IDictionary<string, object> viewData);

        Stream ReadFile(string virtualPath);
        
    }
}


