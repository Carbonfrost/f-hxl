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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Core;

namespace Carbonfrost.Commons.Hxl.Compiler {

    public class HxlCompiledTemplateInfoCollection : ReadOnlyCollection<HxlCompiledTemplateInfo> {

        internal static readonly HxlCompiledTemplateInfoCollection Empty = new HxlCompiledTemplateInfoCollection();

        private readonly ILookup<string, HxlCompiledTemplateInfo> _templates;
        private Assembly asm;

        public HxlCompiledTemplateInfo this[string name] {
            get {
                return _templates[name].FirstOrDefault();
            }
        }

        private HxlCompiledTemplateInfoCollection()
            : base(Empty<HxlCompiledTemplateInfo>.List)
        {
        }

        internal HxlCompiledTemplateInfoCollection(Assembly compiledAssembly)
            : base(Search(compiledAssembly))
        {
            _templates = Items.ToLookup(t => t.Name, StringComparer.OrdinalIgnoreCase);
        }

        private static List<HxlCompiledTemplateInfo> Search(Assembly assembly) {
            var result = new List<HxlCompiledTemplateInfo>();
            var checksForDups = new Dictionary<TemplateKey, HxlCompiledTemplateInfo>();

            foreach (var t in assembly.GetTypesHelper()) {
                if (typeof(HxlTemplate).IsAssignableFrom(t)) {
                    string name, type;
                    HxlTemplateAttribute.NameOrDefault(t, out name, out type);

                    var item = new HxlCompiledTemplateInfo(name, type, t);
                    var existing = checksForDups.GetValueOrDefault(item.Key);
                    if (existing != null) {
                        Traceables.ReflectedTemplateFactoryDuplicatedTemplate(
                            assembly,
                            t,
                            name,
                            existing.CompiledType);
                        continue;
                    }

                    result.Add(item);
                    checksForDups.Add(item.Key, item);
                }
            }

            return result;
        }

        public HxlCompiledTemplateInfo FindTemplate(string name, string type) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrEmpty(name)) {
                throw Failure.EmptyString(nameof(name));
            }

            var candidates = _templates[name];

            Func<HxlCompiledTemplateInfo, bool> predicate = t => t.Type == type
                || TemplateKey.IsDefaultTemplateType(t.Type);

            return candidates.FirstOrDefault(predicate);
        }

        public IEnumerable<HxlCompiledTemplateInfo> FindTemplates(string name) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrEmpty(name)) {
                throw Failure.EmptyString(nameof(name));
            }

            return _templates[name];
        }
    }
}

