//
// - HxlCompilerSettings.cs -
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
using System.Reflection;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Hxl.Compiler;
using Carbonfrost.Commons.Hxl;

namespace Carbonfrost.Commons.Hxl {

    public class HxlCompilerSettings {

        private bool debug;
        private bool emitTemplateFactory;
        private readonly HxlNamespaceCollection namespaces = new HxlNamespaceCollection();
        private readonly DomNodeFactoryCollection factories = new DomNodeFactoryCollection();
        private readonly HxlAssemblyCollection _assemblies = new HxlAssemblyCollection();
        private TypeReference _defaultBaseClass = TypeReference.FromType(typeof(HxlTemplateExtension));
        private IHxlTemplateTypeSelector _templateTypeSelector = HxlTemplateTypeSelector.Default;

        internal IImplicitAssemblyReferencePolicy ImplicitAssemblyReferencePolicy {
            get {
                // TODO Should be able to select more secure policy (security)
                return Compiler.ImplicitAssemblyReferencePolicy.AllowAll(this);
            }
        }

        public IHxlTemplateTypeSelector TemplateTypeSelector {
            get {
                return _templateTypeSelector;
            }
            set {
                ThrowIfReadOnly();
                _templateTypeSelector = value;
            }
        }

        public TypeReference TemplateBaseClass {
            get {
                return _defaultBaseClass;
            }
            set {
                ThrowIfReadOnly();
                _defaultBaseClass = value;
            }
        }

        public DomNodeFactoryCollection NodeFactories {
            get {
                return factories;
            }
        }

        public static HxlCompilerSettings Default {
            get {
                return HxlConfiguration.Current.Compiler;
            }
        }

        public bool EmitTemplateFactory {
            get {
                return emitTemplateFactory;
            }
            set {
                ThrowIfReadOnly();
                emitTemplateFactory = value;
            }
        }

        public bool Debug {
            get { return debug; }
            set {
                ThrowIfReadOnly();
                debug = value;
            }
        }

        public HxlNamespaceCollection Namespaces {
            get { return namespaces; }
        }

        public HxlAssemblyCollection Assemblies {
            get { return _assemblies; }
        }

        public HxlCompilerSettings() : this(HxlCompilerSettings.Default) {}

        public HxlCompilerSettings(HxlCompilerSettings settings) {
            if (settings == null)
                return;

            this.debug = settings.Debug;
            this.emitTemplateFactory = settings.emitTemplateFactory;
            this.namespaces.AddMany(settings.namespaces);
            this.factories.AddMany(settings.factories);
            this._assemblies.AddMany(settings._assemblies);
            this._defaultBaseClass = settings.TemplateBaseClass;
        }

        public HxlCompilerSettings Clone() {
            return new HxlCompilerSettings(this);
        }

        public bool IsReadOnly { get; private set; }

        public void MakeReadOnly() {
            this.IsReadOnly = true;
            this.Namespaces.MakeReadOnly();
            this.Assemblies.MakeReadOnly();
        }

        void ThrowIfReadOnly() {
            if (IsReadOnly)
                throw Failure.Sealed();
        }

        internal TypeReference SelectBaseClass(TypeReference baseClass, TypeReference modelType) {
            if (this.TemplateTypeSelector == null)
                return baseClass;

            return TemplateTypeSelector.GetTemplateBaseClass(baseClass, modelType);
        }
    }
}
