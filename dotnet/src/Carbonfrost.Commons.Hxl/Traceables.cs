//
// - Traceables.cs -
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Carbonfrost.Commons.Instrumentation;

namespace Carbonfrost.Commons.Hxl {

    static class Traceables {

        internal static readonly Logger log = Logger.FromName("carbonfrost.commons.hxl");

        public static void HxlTemplateFactoryCreateTemplate(string name, string templateType, Type type) {
            if (log.DebugEnabled) {
                log.DebugFormat("Template factory created a template {0}{2} => {1}",
                                name, type, string.IsNullOrEmpty(templateType) ? string.Empty : "(" + templateType + ")");
            }
        }

        public static void HxTemplateFactoryFromAssemblyExplicit(Assembly assembly,
                                                                 IEnumerable<HxlTemplateFactoryAttribute> attrs) {
            if (log.TraceEnabled) {
                string items = Utility.JoinList(
                    ", ",
                    attrs,
                    (sb, t) => sb.Append(t.FactoryType));

                log.TraceFormat("Created template factory for assembly {0} ({1}).",
                                assembly.GetName(),
                                items);
            }
        }

        public static void HxTemplateFactoryFromAssemblySlow(Assembly assembly, ReflectedTemplateFactory result) {
            if (log.TraceEnabled) {
                log.TraceFormat("Created template factory for assembly {0} (templates: {1}).",
                                assembly.GetName(), result.TemplateCount);
            }
            if (log.DebugEnabled) {
                log.DebugFormat("Available templates for {0}: {1}",
                                assembly.GetName(),
                                string.Join("\n  ", result));
            }
        }

        public static void HxTemplateFactoryFromAssemblySkip(Assembly assembly) {
            if (log.DebugEnabled) {
                log.DebugFormat("Skipped adding a template factory for assembly {0}).",
                                assembly.GetName());
            }
        }

        public static void ReflectedTemplateFactoryDuplicatedTemplate(Assembly assembly, Type currentType, string name, Type previousType) {
            if (log.WarnEnabled) {
                log.WarnFormat("Template {0} defines duplicate template name `{1}' (previously defined by {2}) (assembly: {3})",
                               currentType,
                               name,
                               previousType.AssemblyQualifiedName,
                               assembly.GetName());
            }
        }

        public static void ReflectedTemplateFactoryCreateTemplate(Assembly assembly,
                                                                  Type type,
                                                                  string name,
                                                                  bool success)
        {

            // TODO success should be in the message so that it is structurally logged
            if (log.TraceEnabled && success)
                log.TraceFormat("Create template {0} ({2}) (assembly: {1})", name, assembly.GetName(), type);
            else if (log.DebugEnabled)
                log.DebugFormat("Create template {0} (assembly: {1}) - not found", name, assembly.GetName());
        }

        public static void ReflectedTemplateFactoryCreateTemplateError(Assembly assembly,
                                                                       Type type,
                                                                       string name,
                                                                       Exception ex) {
            log.ErrorFormat("Failed to create template {0} ({2}) (assembly: {1}): {3} ",
                            name,
                            assembly.GetName(),
                            type,
                            ex);
        }

        public static void ProviderDomNodeFactoryDuplicate(object key, object value) {
            if (log.TraceEnabled) {
                log.TraceFormat("Skipped item because of a duplicate key {0} ({1})",
                                key,
                                value);
            }
        }

        public static void HandleComponentModelReflection(PropertyInfo pd, Exception ex) {
            // TODO Handle errors - possibly use an error object or null
            log.WarnFormat("{0}.{1} - {2}", pd.DeclaringType, pd.Name, ex.GetType());
        }
    }


}
