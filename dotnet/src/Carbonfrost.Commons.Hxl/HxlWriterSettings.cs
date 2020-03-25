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

using System;
using System.Text;
using Carbonfrost.Commons.Html;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class HxlWriterSettings : DomWriterSettings {

        private HxlTemplateContext _templateContext;
        private HtmlWriterSettings _htmlWriterSettings;

        public HxlTemplateContext TemplateContext {
            get {
                if (_templateContext == null) {
                    _templateContext = new HxlTemplateContext();
                }
                return _templateContext;
            }
            set {
                ThrowIfReadOnly();
                _templateContext = value;
            }
        }

        public HtmlWriterSettings HtmlWriterSettings {
            get {
                if (_htmlWriterSettings == null) {
                    _htmlWriterSettings = new HtmlWriterSettings();
                }
                return _htmlWriterSettings;
            }
            set {
                ThrowIfReadOnly();
                _htmlWriterSettings = value;
            }
        }

        public bool PrettyPrint {
            get {
                return HtmlWriterSettings.PrettyPrint;
            }
            set {
                ThrowIfReadOnly();
                HtmlWriterSettings.PrettyPrint = value;
            }
        }

        public int Indent {
            get {
                return HtmlWriterSettings.Indent;
            }
            set {
                ThrowIfReadOnly();
                HtmlWriterSettings.Indent = value;
            }
        }

        public EscapeMode EscapeMode {
            get {
                return HtmlWriterSettings.EscapeMode;
            }
            set {
                ThrowIfReadOnly();
                HtmlWriterSettings.EscapeMode = value;
            }
        }

        public Encoding Charset {
            get {
                return HtmlWriterSettings.Charset;
            }
            set {
                ThrowIfReadOnly();
                HtmlWriterSettings.Charset = value;
            }
        }

        public bool IsXhtml {
            get {
                ThrowIfReadOnly();
                return HtmlWriterSettings.IsXhtml;
            }
            set {
                HtmlWriterSettings.IsXhtml = value;
            }
        }

        public HxlWriterSettings(HxlWriterSettings settings) {
            if (settings != null) {
                HtmlWriterSettings = settings.HtmlWriterSettings;
                TemplateContext = settings.TemplateContext;
            }
        }

        public HxlWriterSettings() {
        }

        public static HxlWriterSettings ReadOnly(HxlWriterSettings settings) {
            if (settings == null) {
                throw new ArgumentNullException(nameof(settings));
            }
            return (HxlWriterSettings) settings.CloneReadOnly();
        }

        public new HxlWriterSettings Clone() {
            return (HxlWriterSettings) base.Clone();
        }

        protected override DomWriterSettings CloneCore() {
            return new HxlWriterSettings(this);
        }

        internal static HxlWriterSettings From(DomWriterSettings settings) {
            if (settings is HxlWriterSettings hw) {
                return hw;
            }
            return new HxlWriterSettings();
        }
    }
}
