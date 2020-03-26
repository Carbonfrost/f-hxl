//
// - RenderMode.cs -
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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Core;


namespace Carbonfrost.Commons.Hxl {

    public struct RenderMode : IEquatable<RenderMode> {

        private readonly string _name;
        private readonly Func<IHxlElementTemplate> _templateThunk;

        public string Name {
            get {
                return _name;
            }
        }

        // TODO Pseudo enum

        public static readonly RenderMode Default
            = new RenderMode(() => HxlElementTemplate.Default, "default");

        public static readonly RenderMode Skip
            = new RenderMode(() => HxlElementTemplate.Skip, "skip");

        private static readonly IDictionary<string, RenderMode> DEFAULT_MODES
            = GetDefaultModes();

        private RenderMode(Func<IHxlElementTemplate> template, string name) {
            this._name = name;
            this._templateThunk = template;
        }

        public static RenderMode Parse(string text) {
            return Utility.Parse<RenderMode>(text, _TryParse);
        }

        public static bool TryParse(string text, out RenderMode result) {
            return _TryParse(text, out result) == null;
        }

        private static IDictionary<string, RenderMode> GetDefaultModes() {
            var fields = typeof(RenderMode).GetFields(BindingFlags.Static);
            return fields.ToDictionary(t => t.Name, t => (RenderMode) t.GetValue(null));
        }

        private static Exception _TryParse(string text, out RenderMode result) {
            text = text.Trim();

            if (DEFAULT_MODES.TryGetValue(text, out result))
                return null;

            // TODO Must be a template name
            throw new NotImplementedException();
        }

        internal IHxlElementTemplate ToTemplate() {
            return _templateThunk();
        }

        public override bool Equals(object obj) {
            return (obj is RenderMode) && Equals((RenderMode) obj);
        }

        public bool Equals(RenderMode other) {
            return this._name == other._name;
        }

        public override int GetHashCode() {
            int hashCode = 0;
            unchecked {
                hashCode += 1000000007 * _name.GetHashCode();
            }
            return hashCode;
        }

        public static bool operator ==(RenderMode lhs, RenderMode rhs) {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(RenderMode lhs, RenderMode rhs) {
            return !(lhs == rhs);
        }

        public override string ToString() {
            return _name;
        }

    }
}
