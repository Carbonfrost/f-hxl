//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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

namespace Carbonfrost.Commons.Hxl {

    static class AttributeFragmentPriority {

        public const int Language = 300;

        internal const int LanguageConditional = Language + 10;
        internal const int LanguageIterative   = Language + 20;
        internal const int LanguageCase        = Language + 30;
        internal const int LanguageText        = Language + 40;

        public const int Layout = 600;
    }
}
