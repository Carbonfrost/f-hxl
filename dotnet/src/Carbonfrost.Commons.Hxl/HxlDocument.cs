//
// Copyright 2013, 2020 Carbonfrost Systems, Inc. (https://carbonfrost.com)
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
using System.IO;

using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class HxlDocument : DomDocument {

        private readonly HxlProviderFactory _providerFactory;

        public new HxlProviderFactory ProviderFactory {
            get {
                return (HxlProviderFactory) base.ProviderFactory;
            }
        }

        protected override DomProviderFactory DomProviderFactory {
            get {
                return _providerFactory;
            }
        }

        public HxlDocument() {
            _providerFactory = new HxlProviderFactory();
        }

        internal HxlDocument(HxlProviderFactory factory) {
            _providerFactory = factory;
        }

        public void WriteTo(TextWriter writer, HxlTemplateContext templateContext) {
            WriteTo(writer, new HxlWriterSettings {
                TemplateContext = templateContext
            });
        }

        public void WriteTo(TextWriter writer, HxlWriterSettings settings) {
            if (writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            new HxlWriter(writer, settings).Write(this);
        }
    }
}
