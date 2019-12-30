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
using System.Diagnostics;
using System.Globalization;
using Carbonfrost.Commons.Instrumentation;

namespace Carbonfrost.Commons.Hxl {

    public class HxlTemplateLogHelper {

        private readonly HxlTemplateExtension _template;

        public HxlTemplateLogHelper(HxlTemplateExtension template) {
            _template = template;
        }

        // TODO Log messages from templates

        [Conditional("DEBUG")]
        public void Debug(object value, object data = null) {
            Log(LoggerLevel.Debug, value, data);
        }

        [Conditional("DEBUG")]
        public void Debug(string message, object data = null) {
            Log(LoggerLevel.Debug, message, data);
        }

        [Conditional("DEBUG")]
        public void Debug(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Debug, message, exception, data);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Debug, format, args);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Debug, format, arg0);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Debug, formatProvider, format, arg0);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Debug, formatProvider, format, args);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Debug, format, arg0, arg1);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(IFormatProvider formatProvider, string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Debug, formatProvider, format, arg0, arg1);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Debug, format, arg0, arg1, arg2);
        }

        [Conditional("DEBUG")]
        public void DebugFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Debug, formatProvider, format, arg0, arg1, arg2);
        }

        public bool Enabled(LoggerLevel level) {
            return false;
        }

        public void Error(object value, object data = null) {
            Log(LoggerLevel.Error, value, data);
        }

        public void Error(string message, object data = null) {
            Log(LoggerLevel.Error, message, data);
        }

        public void Error(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Error, message, exception, data);
        }

        public void ErrorFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Error, format, arg0);
        }

        public void ErrorFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Error, format, args);
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Error, formatProvider, format, args);
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Error, formatProvider, format, arg0);
        }

        public void ErrorFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Error, format, arg0, arg1);
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Error, formatProvider, format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Error, format, arg0, arg1, arg2);
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Error, formatProvider, format, arg0, arg1, arg2);
        }

        public void Fatal(object value, object data = null) {
            Log(LoggerLevel.Fatal, value, data);
        }

        public void Fatal(string message, object data = null) {
            Log(LoggerLevel.Fatal, message, data);
        }

        public void Fatal(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Fatal, message, exception, data);
        }

        public void FatalFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Fatal, format, arg0);
        }

        public void FatalFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Fatal, format, args);
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Fatal, formatProvider, format, args);
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Fatal, formatProvider, format, arg0);
        }

        public void FatalFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Fatal, format, arg0, arg1);
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Fatal, formatProvider, format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Fatal, format, arg0, arg1, arg2);
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Fatal, formatProvider, format, arg0, arg1, arg2);
        }

        public void Info(object value, object data = null) {
            Log(LoggerLevel.Info, value, data);
        }

        public void Info(string message, object data = null) {
            Log(LoggerLevel.Info, message, data);
        }

        public void Info(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Info, message, exception, data);
        }

        public void InfoFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Info, format, args);
        }

        public void InfoFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Info, format, arg0);
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Info, formatProvider, format, args);
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Info, formatProvider, format, arg0);
        }

        public void InfoFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Info, format, arg0, arg1);
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Info, formatProvider, format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Info, format, arg0, arg1, arg2);
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Info, formatProvider, format, arg0, arg1, arg2);
        }

        public void Inspect(object value) {
            Inspect(null, value);
        }

        public void Inspect(string name, object value) {
            InspectionEvent event3 = new InspectionEvent();
            event3.Name = name;
            InspectionEvent event2 = event3;
            // TODO event2.Value.Value = value;
            Log(LoggerLevel.Trace, event2, null);
        }

        public InspectionContext Inspecting(string name) {
            return null;
        }

        public void Log(LoggerLevel level, object value, object data = null) {
            return;
        }

        public void Log(LoggerLevel level, string message, object data = null) {
            return;
        }

        public void Log(LoggerLevel level, string message, Exception exception, object data = null) {
            return;
        }

        public void LogFormat(LoggerLevel level, string format, object arg0) {
            LogFormat(level, CultureInfo.CurrentCulture, format, new object[] { arg0 });
        }

        public void LogFormat(LoggerLevel level, string format, params object[] args) {
            LogFormat(level, CultureInfo.CurrentCulture, format, args);
        }

        public void LogFormat(LoggerLevel level, IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(level, formatProvider, format, new object[] { arg0 });
        }

        public void LogFormat(LoggerLevel level, IFormatProvider formatProvider, string format, params object[] args) {
            return;
        }

        public void LogFormat(LoggerLevel level, string format, object arg0, object arg1) {
            LogFormat(level, CultureInfo.CurrentCulture, format, new object[] {
                               arg0,
                               arg1
                           });
        }

        public void LogFormat(LoggerLevel level, IFormatProvider formatProvider, string format, object arg0, object arg1) {
            LogFormat(level, formatProvider, format, new object[] {
                               arg0,
                               arg1
                           });
        }

        public void LogFormat(LoggerLevel level, string format, object arg0, object arg1, object arg2) {
            LogFormat(level, CultureInfo.CurrentCulture, format, new object[] {
                               arg0,
                               arg1,
                               arg2
                           });
        }

        public void LogFormat(LoggerLevel level, IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(level, formatProvider, format, new object[] {
                               arg0,
                               arg1,
                               arg2
                           });
        }

        [Conditional("TRACE")]
        public void Trace(object value, object data = null) {
            Log(LoggerLevel.Trace, value, data);
        }

        [Conditional("TRACE")]
        public void Trace(string message, object data = null) {
            Log(LoggerLevel.Trace, message, data);
        }

        [Conditional("TRACE")]
        public void Trace(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Trace, message, exception, data);
        }

        [Conditional("TRACE")]
        public void TraceFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Trace, format, args);
        }

        [Conditional("TRACE")]
        public void TraceFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Trace, format, arg0);
        }

        [Conditional("TRACE")]
        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Trace, formatProvider, format, args);
        }

        [Conditional("TRACE")]
        public void TraceFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Trace, formatProvider, format, arg0);
        }

        [Conditional("TRACE")]
        public void TraceFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Trace, format, arg0, arg1);
        }

        [Conditional("TRACE")]
        public void TraceFormat(IFormatProvider formatProvider, string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Trace, formatProvider, format, arg0, arg1);
        }

        [Conditional("TRACE")]
        public void TraceFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Trace, format, arg0, arg1, arg2);
        }

        [Conditional("TRACE")]
        public void TraceFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Trace, formatProvider, format, arg0, arg1, arg2);
        }

        public void Warn(object value, object data = null) {
            Log(LoggerLevel.Warn, value, data);
        }

        public void Warn(string message, object data = null) {
            Log(LoggerLevel.Warn, message, data);
        }

        public void Warn(string message, Exception exception, object data = null) {
            Log(LoggerLevel.Warn, message, exception, data);
        }

        public void WarnFormat(string format, params object[] args) {
            LogFormat(LoggerLevel.Warn, format, args);
        }

        public void WarnFormat(string format, object arg0) {
            LogFormat(LoggerLevel.Warn, format, arg0);
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, object arg0) {
            LogFormat(LoggerLevel.Warn, formatProvider, format, arg0);
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args) {
            LogFormat(LoggerLevel.Warn, formatProvider, format, args);
        }

        public void WarnFormat(string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Warn, format, arg0, arg1);
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, object arg0, object arg1) {
            LogFormat(LoggerLevel.Warn, formatProvider, format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Warn, format, arg0, arg1, arg2);
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, object arg0, object arg1, object arg2) {
            LogFormat(LoggerLevel.Warn, formatProvider, format, arg0, arg1, arg2);
        }

        public bool DebugEnabled {
            get {
                return Enabled(LoggerLevel.Debug);
            }
        }

        public bool ErrorEnabled {
            get {
                return Enabled(LoggerLevel.Error);
            }
        }

        public bool FatalEnabled {
            get {
                return Enabled(LoggerLevel.Fatal);
            }
        }

        public bool InfoEnabled {
            get {
                return Enabled(LoggerLevel.Info);
            }
        }

        public bool TraceEnabled {
            get {
                return Enabled(LoggerLevel.Trace);
            }
        }

        public bool WarnEnabled {
            get {
                return Enabled(LoggerLevel.Warn);
            }
        }

    }
}
