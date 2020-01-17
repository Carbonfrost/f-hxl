//
// - HxlQualifiedNameHelper.cs -
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carbonfrost.Commons.Hxl {

    class HxlQualifiedNameHelper : IHxlQualifiedName {

        // TODO Weak cache instances of this class based upon parse text (performance)

        // prefix:Name.Variable
        // prefix:Name1:Name2:Name3.Variable

        public string Prefix { get; private set; }
        public string Variable { get; private set; }
        public IReadOnlyList<string> Names { get; private set; }

        IReadOnlyList<string> IHxlQualifiedName.Names {
            get {
                return this.Names;
            }
        }

        public bool IsExtendedUsage {
            get {
                return Names.Count > 1;
            }
        }

        public string[] ExtendedPart {
            get {
                return Names.Skip(1).ToArray();
            }
        }

        // prefix:Name1
        public string QualifiedName {
            get {
                return GetQualifiedName(this);
            }
        }

        // prefix:Name1:Name2:Name3
        public string FullName {
            get {
                return GetFullName(this);
            }
        }

        // prefix:Name1
        public string Name {
            get {
                if (Prefix.Length == 0)
                    return LocalName;
                else
                    return Prefix + ":" + LocalName;
            }
        }

        // prefix:Name1
        public string LocalName {
            get {
                return Names[0];
            }
        }

        public string Property {
            get {
                return this.GetProperty();
            }
        }

        public HxlQualifiedNameHelper(string prefix,
                                      string variable,
                                      string[] names) {

            this.Prefix = prefix;
            this.Variable = variable;
            this.Names = names;
        }

        public HxlQualifiedName ToName(Uri ns) {
            return new HxlQualifiedName(Prefix, Names, Variable, ns);
        }

        public static HxlQualifiedNameHelper Parse(string text) {
            string[] nameAndVariable = text.Split('.');
            string fullName = nameAndVariable[0];
            string variable = null;
            string[] allNames = fullName.Split(':');
            string prefix = null;
            string[] names;

            if (nameAndVariable.Length == 2) {
                variable = nameAndVariable[1];
            }

            if (allNames.Length >= 2) {
                prefix = allNames[0];
                names = allNames.Skip(1).ToArray();
            } else {
                names = allNames;
            }

            return new HxlQualifiedNameHelper(prefix, variable, names);
        }

        internal static string GetQualifiedName(IHxlQualifiedName helper) {
            if (string.IsNullOrEmpty(helper.Prefix))
                return helper.Name;
            else
                return string.Format("{0}:{1}", helper.Prefix, helper.Name);
        }

        internal static string GetFullName(IHxlQualifiedName helper) {
            StringBuilder sb = new StringBuilder();
            foreach (var s in helper.Names) {
                sb.Append(":");
                sb.Append(s);
            }

            if (string.IsNullOrEmpty(helper.Prefix))
                return sb.ToString(1, sb.Length - 1);
            else
                return helper.Prefix + sb;
        }
    }
}
