//
// Copyright 2014, 2015 Carbonfrost Systems, Inc. (http://carbonfrost.com)
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Core;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public partial class HxlAssemblyCollection : Collection<HxlAssembly>, IHxlNamespaceResolver {

        private IDictionary<string, Uri> _namespaceCache = new Dictionary<string, Uri>();

        private IDictionary<Assembly, XmlnsAttribute[]> _attrsCache = new Dictionary<Assembly, XmlnsAttribute[]>();
        private static readonly Dictionary<string, Assembly> _asmCache = new Dictionary<string, Assembly>();

        public bool DisableAutomaticProbing { get; set; }

        public HxlAssemblyCollection() {}

        public HxlAssemblyCollection(IEnumerable<HxlAssembly> items) {
            if (items != null)
                Items.AddMany(items);
        }

        public HxlAssemblyCollection Clone() {
            return new HxlAssemblyCollection(Items) {
                DisableAutomaticProbing = DisableAutomaticProbing,
            };
        }

        public HxlAssembly AddNew(AssemblyName name, Uri source = null) {
            if (name == null)
                throw new ArgumentNullException("name");

            var item = new HxlAssembly(name, source);
            Items.Add(item);
            return item;
        }

        internal HxlAssembly AddNewFile(string file) {
            var asm = AssemblyName.GetAssemblyName(file);
            return AddNew(asm, new Uri("file://" + file));
        }

        public void Add(Assembly assembly) {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            var codeBase = new Uri(assembly.CodeBase);
            AddNew(assembly.GetName(), codeBase);
        }

        public Uri LookupNamespace(string prefix) {
            if (string.IsNullOrEmpty(prefix))
                return null;

            // Memoize for performance
            var result = _namespaceCache.GetValueOrDefault(prefix);
            if (true ) {
                var xml = Search(t => t.Prefix == prefix);
                if (xml != null) {
                    return _namespaceCache[prefix] = new Uri(xml.Xmlns);
                }
            }

            return null;
        }

        public string LookupPrefix(Uri namespaceUri) {
            if (namespaceUri == null)
                throw new ArgumentNullException("namespaceUri");

            // No memoization because this is a much less frequent operation
            var xml = Search(t => t.Xmlns == namespaceUri.ToString());
            return xml == null ? null : xml.Prefix;
        }

        protected void ThrowIfReadOnly() {
            if (this.IsReadOnly)
                throw Failure.ReadOnlyCollection();
        }

        private XmlnsAttribute[] EnsureAttrs(Assembly asm) {
            return _attrsCache.GetValueOrCache(
                asm,
                a => (XmlnsAttribute[]) asm.GetCustomAttributes(typeof(XmlnsAttribute)) ?? Empty<XmlnsAttribute>.Array);
        }

        private static Assembly LoadAssembly(AssemblyName asm, Uri source) {
            // TODO Matching the component name is too loose
            Assembly result;
            if (_asmCache.TryGetValue(asm.Name, out result)) {
                return result;
            }
            result = App.LoadedAssemblies.FirstOrDefault(t => asm.Name == t.GetName().Name);

            if (result != null) {
                _asmCache[asm.Name] = result;
                return result;
            }

            try {
                return Assembly.Load(asm);
            } catch {
                // TODO Should have better handling of this failure
            }
            return null;
        }

        private XmlnsAttribute Search(Func<XmlnsAttribute, bool> pred) {
            return FindXmlnsAttrs().FirstOrDefault(pred);
        }

        private IEnumerable<XmlnsAttribute> FindXmlnsAttrs() {
            IEnumerable<XmlnsAttribute> result = Enumerable.Empty<XmlnsAttribute>();
            foreach (var item in this) {
                var asm = LoadAssembly(item.Name, item.Source);
                if (asm != null) {
                    result = result.Concat(EnsureAttrs(asm));
                }
            }

            if (DisableAutomaticProbing) {
                return result;
            }

            // TODO This will re-enumerate the assemblies enumerated  above (performance)
            foreach (var asm in App.LoadedAssemblies) {
                result = result.Concat(EnsureAttrs(asm));
            }
            return result.Distinct();
        }

        protected override void ClearItems() {
            ThrowIfReadOnly();
            base.ClearItems();
        }

        protected override void InsertItem(int index, HxlAssembly item) {
            ThrowIfReadOnly();
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index) {
            ThrowIfReadOnly();
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, HxlAssembly item) {
            ThrowIfReadOnly();
            base.SetItem(index, item);
        }

        // `IMakeReadOnly' implementation
        public void MakeReadOnly() {
            IsReadOnly = true;
        }

        public bool IsReadOnly {
            get; private set;
        }
    }

}
