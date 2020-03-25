//
// Copyright 2014, 2016, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    [DomProviderFactoryUsage]
    public class HxlProviderFactory : DomProviderFactory {

        static readonly HashSet<Type> PROVIDER_TYPES = new HashSet<Type> {
            typeof(ElementFragment),
            typeof(ProcessingInstructionFragment),
            typeof(AttributeFragment),
            typeof(HxlDocument),
            typeof(HxlDocumentFragment),
            typeof(IHxlDocumentAccessor),

            typeof(HxlWriter),

            // TODO This is an incomplete list
        };

        private readonly IDomNodeFactory _factory;

        // TODO Probably should compose a factory on the argument
        protected override IDomNodeFactory CreateDomNodeFactory(IDomNodeTypeProvider nodeTypeProvider) {
            return _factory;
        }

        // TODO This class would be more useful if other nodes were accessible
        // (perhaps by using CompilerSettings.Default)

        public HxlProviderFactory()
            : this(HxlDomNodeFactory.Compiler) {}

        internal HxlProviderFactory(IDomNodeFactory factory) {
            if (factory == null) {
                throw new ArgumentNullException(nameof(factory));
            }
            _factory = factory;
        }

        public override bool IsProviderObject(Type providerObjectType) {
            if (providerObjectType == null) {
                throw new ArgumentNullException("providerObjectType");
            }
            return PROVIDER_TYPES.Any(t => t.GetTypeInfo().IsAssignableFrom(providerObjectType));
        }

        public override string GenerateDefaultName(Type providerObjectType) {
            if (providerObjectType == null) {
                throw new ArgumentNullException(nameof(providerObjectType));
            }
            if (typeof(ElementFragment).GetTypeInfo().IsAssignableFrom(providerObjectType)) {
                return ElementName(providerObjectType);

            } else if (typeof(AttributeFragment).GetTypeInfo().IsAssignableFrom(providerObjectType)) {
                return AttributeName(providerObjectType);

            }
            return null;
        }

        public new HxlWriter CreateWriter(TextWriter writer) {
            return (HxlWriter) base.CreateWriter(writer);
        }

        public HxlWriter CreateWriter(TextWriter writer, HxlWriterSettings settings) {
            return (HxlWriter) base.CreateWriter(writer);
        }

        public new HxlWriter CreateWriter(TextWriter writer, DomWriterSettings settings) {
            return (HxlWriter) base.CreateWriter(writer, settings);
        }

        protected override DomWriter CreateDomWriter(TextWriter textWriter, DomWriterSettings settings) {
            return new HxlWriter(textWriter, HxlWriterSettings.From(settings));
        }

        static string ElementName(Type type) {
            return FindPrefix(type) + ElementFragment.GetImplicitName(type);
        }

        static string AttributeName(Type type) {
            return FindPrefix(type) + AttributeFragment.GetImplicitName(type);
        }

        static string FindPrefix(Type dt) {
            string prefix = HxlCompilerContext.Current.Prefix;
            if (prefix == null) {
                prefix = AssemblyInfo.GetAssemblyInfo(dt.GetTypeInfo().Assembly)
                    .GetXmlNamespacePrefix(dt.GetQualifiedName().Namespace);
            }

            if (prefix != null) {
                prefix = prefix + ":";
            }

            return prefix;
        }

    }
}
