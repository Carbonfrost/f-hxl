//
// Copyright 2015, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System.Collections.Generic;
using System.Linq;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public class HxlCompilerErrorCollection : IList<HxlCompilerError> {

        private readonly List<HxlCompilerError> _items
            = new List<HxlCompilerError>();

        public bool HasErrors {
            get {
                return this.Any(t => !t.IsWarning);
            }
        }

        internal void AddNew(string errorNumber,
                             string errorText,
                             string fileName,
                             int column,
                             int line,
                             bool isWarning)
        {
            var item = new HxlCompilerError {
                ErrorNumber = errorNumber,
                ErrorText = errorText,
                FileName = fileName,
                Line = line,
                Column = column,
                IsWarning = isWarning,
            };
            Add(item);
        }

        public int IndexOf(HxlCompilerError item) {
            return _items.IndexOf(item);
        }

        public void Insert(int index, HxlCompilerError item) {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index) {
            _items.RemoveAt(index);
        }

        public HxlCompilerError this[int index] {
            get {
                return _items[index];
            }
            set {
                _items[index] = value;
            }
        }

        public void Add(HxlCompilerError item) {
            _items.Add(item);
        }

        public void Clear() {
            _items.Clear();
        }

        public bool Contains(HxlCompilerError item) {
            return _items.Contains(item);
        }

        public void CopyTo(HxlCompilerError[] array, int arrayIndex) {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(HxlCompilerError item) {
            return _items.Remove(item);
        }

        public int Count {
            get {
                return _items.Count;
            }
        }

        public bool IsReadOnly {
            get {
                return false;
            }
        }

        public IEnumerator<HxlCompilerError> GetEnumerator() {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
