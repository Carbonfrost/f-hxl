//
// Copyright 2014, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

using System;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class HxlcLogger : IHxlcLogger {

        private readonly HxlcTraceLevel _traceLevel;

        public HxlcLogger(HxlcTraceLevel traceLevel) {
            _traceLevel = traceLevel;
        }

        public void InfoFormat(string format, params object[] args) {
            Info(string.Format(format, args));
        }

        public void Info(string message) {
            Console.Error.WriteLine(message);
        }

        public void DebugFormat(string format, params object[] args) {
            Debug(string.Format(format, args));
        }

        public void Debug(Exception ex) {
            Debug(ex.ToString());
        }

        public void Debug(string message) {
            if (_traceLevel >= HxlcTraceLevel.Debug) {
                Console.Error.WriteLine(message);
            }
        }

        public void WarnFormat(string format, params object[] args) {
            Warn(string.Format(format, args));
        }

        public void Warn(string message) {
            Console.Error.WriteLine(message);
        }

        public void ErrorFormat(string format, params object[] args) {
            Error(string.Format(format, args));
        }

        public void Error(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void TraceFormat(string format, params object[] args) {
            Trace(string.Format(format, args));
        }

        public void Trace(string message) {
            if (_traceLevel >= HxlcTraceLevel.Trace) {
                Console.Error.WriteLine(message);
            }
        }
    }
}

