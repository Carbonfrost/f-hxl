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

using System;
using System.Reflection;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public class HxlAssembly : IEquatable<HxlAssembly> {

        private readonly AssemblyName _name;
        private Uri _source;

        public AssemblyName Name {
            get {
                return _name;
            }
        }

        public Uri Source {
            get {
                return _source;
            }
        }

        public HxlAssembly(AssemblyName name) : this(name, null) {}

        public HxlAssembly(AssemblyName name, Uri source) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            _name = name;
            _source = source;
        }

        public bool Equals(HxlAssembly other) {
            if (other == null) {
                return false;
            }

            return object.Equals(_name, other._name) && object.Equals(_source, other._source);
        }

        public override string ToString() {
            if (Source == null)
                return _name.ToString();

            return string.Format("{0} ({1})", _name, _source);
        }

        public override bool Equals(object obj) {
            HxlAssembly other = obj as HxlAssembly;
            return Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return 37 * _name.GetHashCode();
            }
        }
    }
}
