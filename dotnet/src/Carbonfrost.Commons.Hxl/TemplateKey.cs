//
// - TemplateKey.cs -
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

namespace Carbonfrost.Commons.Hxl {

    struct TemplateKey {

        readonly string _templateName;
        readonly string _templateType;

        public TemplateKey(string templateName, string templateType) {
            if (string.IsNullOrEmpty(templateType)) {
                templateType = "*";
            }

            _templateType = templateType;
            _templateName = templateName;
        }

        public static bool IsDefaultTemplateType(string type) {
            return string.IsNullOrEmpty(type) || type == "*";
        }

        public override int GetHashCode() {
            int hashCode = 0;
            unchecked {
                hashCode += 37 * StringComparer.OrdinalIgnoreCase.GetHashCode(_templateName);
                hashCode += 17 * StringComparer.OrdinalIgnoreCase.GetHashCode(_templateType);
            }
            return hashCode;
        }

        public override bool Equals(object obj) {
            return (obj is TemplateKey) && Equals((TemplateKey) obj);
        }

        public bool Equals(TemplateKey other) {
            return string.Equals(_templateName, other._templateName, StringComparison.OrdinalIgnoreCase)
                && string.Equals(_templateType, other._templateType, StringComparison.OrdinalIgnoreCase);
        }

    }
}
