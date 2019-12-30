//
// - HxlcException.cs -
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
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Carbonfrost.Commons.Hxl.Compiler {

    class HxlcException : Exception {

        private readonly int errorCode;
        private const string ERROR_CODE = "ErrorCode";

        public int ErrorCode {
            get { return errorCode; }
        }

        public HxlcException() {}

        public HxlcException(int errorCode, string message)
            : base(message)
        {
            this.errorCode = errorCode;
        }

        public HxlcException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.errorCode = errorCode;
        }

        protected HxlcException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.errorCode = info.GetInt32(ERROR_CODE);
        }

        [SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);

            info.AddValue(ERROR_CODE, errorCode);
        }
    }
}
