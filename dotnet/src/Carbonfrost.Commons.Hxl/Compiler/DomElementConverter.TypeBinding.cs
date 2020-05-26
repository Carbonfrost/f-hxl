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

using System.Linq;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl.Compiler {

    abstract partial class DomElementConverter {

        // Reifies a DomElement that has a server element name to an actual instance of it
        public static readonly DomElementConverter TypeBinding = new TypeBindingDomElementConverter();

        class TypeBindingDomElementConverter : DomElementConverter {

            public override DomElement Convert(DomConverter parent, DomElement element, HxlServices services) {
                // FIXME Use getnodetype instead
                var factory = services.NodeFactory;
                var result = factory.CreateElement(element.NodeName);
                if (result == null) {
                    throw HxlFailure.ServerElementCannotBeCreated(element.NodeName, -1, -1);
                }

                // FIXME Optimization? same result?
                services.ReferencePath.AddImplicitTypeUse(result.GetType());

                // Attributes can be moved over to save from copying them
                foreach (var attr in element.Attributes.ToArray()) {
                    result.Append(attr);
                }
                result.Append(
                    parent.ConvertAll(element.ChildNodes).ToList()
                );
                return result;
            }
        }
    }
}
