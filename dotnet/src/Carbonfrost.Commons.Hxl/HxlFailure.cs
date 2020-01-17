//
// Copyright 2013, 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using Carbonfrost.Commons.Hxl.Resources;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Hxl {

    static class HxlFailure {

        public static HxlException CompiledWithErrors(string temp) {
            return Failure.Prepare(new HxlException(SR.CompiledWithErrors(temp)));
        }

        public static ArgumentException AssemblyIsReflectionOnly(string argumentName) {
            return Failure.Prepare(new ArgumentException(SR.AssemblyIsReflectionOnly(), argumentName));
        }

        public static FormatException InvalidDirective() {
            return Failure.Prepare(new FormatException(SR.InvalidDirective()));
        }

        // TODO Add line numbers to parse exceptions

        public static HxlParseException DirectiveNotDefined(string key, int line, int pos) {
            return Failure.Prepare(new HxlParseException(SR.DirectiveNotDefined(key), line, pos));
        }

        public static ArgumentException PrefixAlreadyDefined(string argName, string prefix) {
            return Failure.Prepare(new ArgumentException(SR.PrefixAlreadyDefined(prefix), argName));
        }

        public static HxlParseException FailedToReadServerElement(Exception exception) {
            return Failure.Prepare(new HxlParseException(SR.FailedToReadServerElement(), exception));
        }

        public static HxlParseException NotValidVariableName(string varName) {
            return Failure.Prepare(new HxlParseException(SR.NotValidVariableName(varName)));
        }

        public static HxlParseException CannotLoadMasterTemplate(string layoutFile) {
            return Failure.Prepare(new HxlParseException(SR.CannotLoadMasterTemplate(layoutFile)));
        }

        public static HxlParseException CannotLoadAnyMasterTemplates() {
            return Failure.Prepare(new HxlParseException(SR.CannotLoadAnyMasterTemplates()));
        }

        public static HxlParseException CannotCreateAttributeOnConversion(string name, Exception ex) {
            return Failure.Prepare(new HxlParseException(SR.CannotCreateAttributeOnConversion(name), ex));
        }

        public static ArgumentException CannotUseBuiltinPrefixes(string argumentName) {
            return Failure.Prepare(new ArgumentException(SR.CannotUseBuiltinPrefixes(), argumentName));
        }

        public static HxlParseException ServerElementCannotBeCreated(string nodeName, int line, int pos) {
            return Failure.Prepare(new HxlParseException(SR.ServerElementCannotBeCreated(nodeName), line, pos));
        }

        public static HxlParseException ServerAttributeCannotBeCreated(string name, int line, int pos) {
            return Failure.Prepare(new HxlParseException(SR.ServerAttributeCannotBeCreated(name), line, pos));
        }

        public static HxlParseException ServerAttributePropertyNotFound(Type type, string property, int line, int pos) {
            return Failure.Prepare(new HxlParseException(SR.ServerAttributePropertyNotFound(type, property), line, pos));
        }

        public static InvalidOperationException InvalidToWriteTextOutput() {
            return Failure.Prepare(new InvalidOperationException(SR.InvalidToWriteTextOutput()));
        }

        public static ArgumentException CannotSpecifyNameArgumentWithPosition(string argumentName) {
            return Failure.Prepare(new ArgumentException(SR.CannotSpecifyNameArgumentWithPosition(), argumentName));
        }

        public static ArgumentException MustSpecifyNameArgumentWithPosition(string argumentName) {
            return Failure.Prepare(new ArgumentException(SR.MustSpecifyNameArgumentWithPosition(), argumentName));
        }

        public static HxlParseException CannotFindMatchingTemplate(string type, string name, int line, int pos) {
            return Failure.Prepare(new HxlParseException(SR.CannotFindMatchingTemplate(name, type), line, pos));
        }

        public static HxlException NoMatchingServerAttribute(string fullName) {
            return Failure.Prepare(new HxlException(SR.NoMatchingServerAttribute(fullName)));
        }

        public static HxlException NoMatchingServerElement(string fullName) {
            return Failure.Prepare(new HxlException(SR.NoMatchingServerElement(fullName)));
        }

        public static HxlException PrefixDoesNotMapToNamespace(string prefix) {
            return Failure.Prepare(new HxlException(SR.PrefixDoesNotMapToNamespace(prefix)));
        }
    }
}
