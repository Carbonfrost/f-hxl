//
// - InvalidTextOutput.cs -
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
using System.Text;

namespace Carbonfrost.Commons.Hxl {

    class InvalidTextOutput : TextWriter, ITextOutput {

        public static readonly ITextOutput Instance = new InvalidTextOutput();

        private void InvalidWrite() {
            throw HxlFailure.InvalidToWriteTextOutput();
        }

        public override Encoding Encoding {
            get {
                return Encoding.ASCII;
            }
        }

        public override void Write(bool value) {
            InvalidWrite();
        }

        public override void Write(char value) {
            InvalidWrite();
        }

        public override void Write(char[] buffer) {
            InvalidWrite();
        }

        public override void Write(char[] buffer, int index, int count) {
            InvalidWrite();
        }

        public override void Write(decimal value) {
            InvalidWrite();
        }

        public override void Write(double value) {
            InvalidWrite();
        }

        public override void Write(float value) {
            InvalidWrite();
        }

        public override void Write(int value) {
            InvalidWrite();
        }

        public override void Write(long value) {
            InvalidWrite();
        }

        public override void Write(string format, object arg0) {
            InvalidWrite();
        }

        public override void Write(string format, object arg0, object arg1) {
            InvalidWrite();
        }

#if NET
        public override void Close() {}
        // TODO Complete this override list
#endif

        void ITextOutput.Write(string value) {
            InvalidWrite();
        }

        void ITextOutput.WriteLine(string value) {
            InvalidWrite();
        }

        void ITextOutput.Write(object value) {
            InvalidWrite();
        }

        void ITextOutput.WriteLine(object value) {
            InvalidWrite();
        }

        void ITextOutput.Write(string value, params object[] args) {
            InvalidWrite();
        }

        void ITextOutput.WriteLine(string format, params object[] args) {
            InvalidWrite();
        }

        TextWriter ITextOutput.Output {
            get {
                return this;
            }
        }

        HxlWriter ITextOutput.StartBufferContent(string name) {
            InvalidWrite();
            return null;
        }

        public void EndBufferContent(string name) {
            InvalidWrite();
        }

    }

}
