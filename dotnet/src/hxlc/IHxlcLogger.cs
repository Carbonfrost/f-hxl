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

    interface IHxlcLogger {

        void DebugFormat(string format, params object[] args);
        void Debug(string message);
        void Debug(Exception ex);

        void WarnFormat(string format, params object[] args);
        void Warn(string message);

        void ErrorFormat(string format, params object[] args);
        void Error(string message);

        void InfoFormat(string format, params object[] args);
        void Info(string message);

        void TraceFormat(string format, params object[] args);
        void Trace(string message);
    }
}
