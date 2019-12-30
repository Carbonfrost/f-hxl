//
// - ITextOutput.cs -
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
using System.IO;
using System.Linq;

namespace Carbonfrost.Commons.Hxl {

    interface ITextOutput {

        TextWriter Output { get; }

        void Write(string value);
        void WriteLine(string value);
        void Write(object value);
        void WriteLine(object value);

        void Write(string value, params object[] args);
        void WriteLine(string format, params object[] args);

        HxlWriter StartBufferContent(string name);
        void EndBufferContent(string name);
    }
}
