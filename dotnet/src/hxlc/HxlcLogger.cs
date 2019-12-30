//
// - HxlcLogger.cs -
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

namespace Carbonfrost.Commons.Hxl.Compiler {

    class HxlcLogger : IHxlcLogger {

        public static readonly IHxlcLogger Null = new HxlcLogger();

        public void InfoFormat(string format, params object[] args) {
            Info(string.Format(format, args));
        }

        public void Info(string message) {
            Console.WriteLine(message);
        }
        public void DebugFormat(string format, params object[] args) {
            Debug(string.Format(format, args));
        }

        public void Debug(string message) {
            Console.WriteLine(message);
        }

        public void WarnFormat(string format, params object[] args) {
            Warn(string.Format(format, args));
        }

        public void Warn(string message) {
            Console.WriteLine(message);
        }

        public void Debug(Exception ex) {
            Console.WriteLine(ex);
        }

        public void ErrorFormat(string format, params object[] args) {
            Error(string.Format(format, args));
        }

        public void Error(string message) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}

