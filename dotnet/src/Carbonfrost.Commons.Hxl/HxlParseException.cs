//
// - HxlParseException.cs -
//
// Copyright 2013 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Runtime.Serialization;

using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Hxl.Resources;
using Carbonfrost.Commons.Html;

namespace Carbonfrost.Commons.Hxl {

    public class HxlParseException : HxlException, IHtmlLineInfo, IDomLineInfo {

        public int LineNumber { get; private set; }
        public int LinePosition { get; private set; }
        public string SourceUri { get; set; }

        public HxlParseException() {
            this.LineNumber = 0;
            this.LinePosition = 0;
        }

        public HxlParseException(string message) : base(message) {
            this.LineNumber = 0;
            this.LinePosition = 0;
        }

        public HxlParseException(string message, Exception innerException) : base(message, innerException) {
            this.LineNumber = 0;
            this.LinePosition = 0;
        }

        public HxlParseException(string message, int lineNumber, int linePosition) : base(BuildMessage(message, lineNumber, linePosition)) {
            this.LineNumber = lineNumber;
            this.LinePosition = linePosition;
        }

        public HxlParseException(string message, Exception innerException, int lineNumber, int linePosition) : base(BuildMessage(message, lineNumber, linePosition), innerException) {
            this.LineNumber = lineNumber;
            this.LinePosition = linePosition;
        }

#if NET
        protected HxlParseException(SerializationInfo info, StreamingContext context) : base(info, context) {
            this.LineNumber = info.GetInt32("lineNumber");
            this.LinePosition = info.GetInt32("linePosition");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
            info.AddValue("lineNumber", this.LineNumber);
            info.AddValue("linePosition", this.LinePosition);
            info.AddValue("sourceUri", this.SourceUri);
        }
#endif

        bool IHtmlLineInfo.HasLineInfo() {
            return ((this.LineNumber > 0) && (this.LinePosition > 0));
        }

        bool IDomLineInfo.HasLineInfo() {
            return ((IHtmlLineInfo) this).HasLineInfo();
        }

        private static string BuildMessage(string message, int lineNumber, int linePosition) {
            if (linePosition > 0 && lineNumber > 0)
                return (message + Environment.NewLine + Environment.NewLine + SR.LineInfo(lineNumber, linePosition));
            else
                return message;
        }

    }
}
