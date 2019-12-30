//
// - HxlDocument.cs -
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
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class HxlDocument : DomDocument {

        private readonly HxlProviderFactory _providerFactory;

        public override DomProviderFactory ProviderFactory {
            get {
                return _providerFactory;
            }
        }

        private IDomNodeFactory NodeFactory {
            get {
                return ProviderFactory.NodeFactory;
            }
        }

        public HxlDocument() {
            _providerFactory = new HxlProviderFactory();
        }

        internal HxlDocument(HxlProviderFactory factory) {
            _providerFactory = factory;
        }

        public override void WriteTo(TextWriter writer) {
            WriteTo(writer, null);
        }

        // TODO Reconsider this API - it is invoked by generated templates
        // Could at least change HxlTemplateContext => IDictionary<>

        public void WriteTo(TextWriter writer, HxlTemplateContext templateContext) {
            if (writer == null)
                throw new ArgumentNullException("writer");

            new HxlWriter(writer, templateContext).Write(this);
        }

    }

}
