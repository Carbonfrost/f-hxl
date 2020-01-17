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
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Hxl {

    public struct AppendTextPosition : IEquatable<AppendTextPosition> {

        public static readonly AppendTextPosition LastChild = new AppendTextPosition(KnownAppendTextPosition.LastChild);
        public static readonly AppendTextPosition FirstChild = new AppendTextPosition(KnownAppendTextPosition.FirstChild);
        public static readonly AppendTextPosition Replace = new AppendTextPosition(KnownAppendTextPosition.Replace);
        public static readonly AppendTextPosition Before = new AppendTextPosition(KnownAppendTextPosition.Before);
        public static readonly AppendTextPosition After = new AppendTextPosition(KnownAppendTextPosition.After);
        public static readonly AppendTextPosition None = new AppendTextPosition(KnownAppendTextPosition.None);

        private readonly KnownAppendTextPosition _position;
        private readonly string _name;

        public KnownAppendTextPosition Position {
            get {
                return _position;
            }
        }

        public string Name {
            get {
                return _name;
            }
        }

        public AppendTextPosition(KnownAppendTextPosition position) {
            if (RequiresName(position))
                throw HxlFailure.CannotSpecifyNameArgumentWithPosition("position");

            _position = position;
            _name = null;
        }

        public AppendTextPosition(KnownAppendTextPosition position, string name) {
            if (!RequiresName(position))
                throw HxlFailure.MustSpecifyNameArgumentWithPosition("position");
            if (name == null)
                throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(name))
                throw Failure.EmptyString("name");

            _position = position;
            _name = name;
        }

        private static bool RequiresName(KnownAppendTextPosition position) {
            switch (position) {
                case KnownAppendTextPosition.LastChild:
                case KnownAppendTextPosition.FirstChild:
                case KnownAppendTextPosition.Replace:
                case KnownAppendTextPosition.Before:
                case KnownAppendTextPosition.After:
                case KnownAppendTextPosition.None:
                case KnownAppendTextPosition.Head:
                case KnownAppendTextPosition.Body:
                    return false;

                case KnownAppendTextPosition.Element:
                case KnownAppendTextPosition.Placeholder:
                    return true;

                default:
                    throw Failure.NotDefinedEnum("position", position);
            }
        }

        public static AppendTextPosition Parse(string text) {
            return Utility.Parse<AppendTextPosition>(text, _TryParse);
        }

        public static bool TryParse(string text, out AppendTextPosition result) {
            return _TryParse(text, out result) == null;
        }

        public override bool Equals(object obj) {
            return (obj is AppendTextPosition)
                && Equals((AppendTextPosition)obj);
        }

        public bool Equals(AppendTextPosition other) {
            return _position == other._position
                && _name == other._name;
        }

        public override int GetHashCode() {
            int hashCode = 0;
            unchecked {
                hashCode += 1000000007 * _position.GetHashCode();
                if (_name != null)
                    hashCode += 1000000009 * _name.GetHashCode();
            }
            return hashCode;
        }

        public static bool operator ==(AppendTextPosition lhs, AppendTextPosition rhs) {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(AppendTextPosition lhs, AppendTextPosition rhs) {
            return !(lhs == rhs);
        }

        private static Exception _TryParse(string text, out AppendTextPosition result) {
            result = default(AppendTextPosition);

            switch (text.ToLowerInvariant()) {
                case "last-child":
                case "lastchild":
                    result = AppendTextPosition.LastChild;
                    return null;

                case "first-child":
                case "firstchild":
                    result = AppendTextPosition.FirstChild;
                    return null;

                case "replace":
                    result = AppendTextPosition.Replace;
                    return null;

                case "before":
                    result = AppendTextPosition.Before;
                    return null;

                case "after":
                    result = AppendTextPosition.After;
                    return null;

                case "none":
                    result = AppendTextPosition.None;
                    return null;
            }

            // TODO Parse element() and placeholder()
            return Failure.NotParsable("text", typeof(AppendTextPosition));
        }
    }
}
