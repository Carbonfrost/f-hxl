//
// Copyright 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    abstract partial class DomElementConverter {

        public static readonly DomElementConverter Server = new ServerDomElementConverter();

        class ServerDomElementConverter : DomElementConverter {

            public override DomElement Convert(DomConverter parent, DomElement element, HxlServices services) {
                var factory = services.NodeFactory;
                // var result = factory.CreateElement(element.NodeName);

                // if (result == null) {
                //     throw HxlFailure.ServerElementCannotBeCreated(element.NodeName, -1, -1);
                // }

                // services.ReferencePath.AddImplicitTypeUse(result.GetType());
                if (!(element is HxlElement)) {
                    return parent.KeepButConvertChildren(element);
                }

                var result = parent.KeepButConvertChildren(element);
                // result.Append(
                //     parent.ConvertAll(element.ChildNodes)
                // );

                // TODO This will not work if expressions are in non-string types
                var myValues = element.Attributes.Select(t => new KeyValuePair<string, object>(t.Name, t.Value));

                // Locate the property handling inner text
                // TODO Using inner text (but it could contain markup, which would technically require special handling logic)
                // TODO Memoize this lookup (performance)
                foreach (PropertyInfo p in Utility.ReflectGetProperties(result.GetType())) {
                    if (p.IsDefined(typeof(ValueAttribute))) {
                        var kvp = new KeyValuePair<string, object>(p.Name, result.InnerText);
                        myValues = Utility.Cons(kvp, myValues);
                    }
                }

                Activation.Initialize(result, myValues);
                return result;
            }
        }
    }
}
