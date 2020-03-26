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
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Carbonfrost.Commons.Core.Runtime;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class HxlTemplateContext : DynamicObject {

        private readonly IDictionary<string, object> data = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private readonly IProperties _self;
        private readonly IPropertyProvider _selfValues;
        private readonly IPropertyStore _ownerValues;
        private IHxlTemplateFactory templateFactory;
        private readonly PropertyProviderCollection _dataProviders = new PropertyProviderCollection();
        private HxlMasterInfo _masterInfo;
        private IList<BufferContent> _buffers;

        public IHxlTemplateFactory TemplateFactory {
            get {
                if (templateFactory != null) {
                    return templateFactory;
                }
                if (Parent != null) {
                    return Parent.TemplateFactory;
                }

                return HxlTemplateFactory.Null;
            }
            set {
                templateFactory = value;
            }
        }

        public PropertyProviderCollection DataProviders {
            get {
                return _dataProviders;
            }
        }

        public IDictionary<string, object> Data {
            get { return data; }
        }

        public HxlTemplateContext Parent { get; private set; }

        internal PageLayoutInfo SinglePageLayoutInfo {
            get {
                object layoutsObject;
                if (Data.TryGetValue("pageLayoutInfo", out layoutsObject)) {
                    return layoutsObject as PageLayoutInfo;
                }
                if (Parent == null) {
                    return null;
                }
                return Parent.SinglePageLayoutInfo;
            }
        }

        internal HxlMasterInfo MasterInfo {
            get {
                return _masterInfo;
            }
        }

        internal static HxlTemplateContext WithData(HxlTemplate owner, IEnumerable<KeyValuePair<string, object>> variables) {
            var tc = new HxlTemplateContext(owner);

            if (variables != null) {
                foreach (var e in variables)
                    tc.Data.Add(e.Key, e.Value);
            }
            return tc;
        }

        public HxlTemplateContext() : this(null) {
        }

        public HxlTemplateContext(object owner) {
            _self = Properties.FromValue(this);
            _ownerValues = owner == null ? Properties.Null : Properties.FromValue(owner);
            _selfValues = PropertyProvider.Compose(_ownerValues, _self, DataProviders);
        }

        public override IEnumerable<string> GetDynamicMemberNames() {
            return _self.Select(t => t.Key).Concat(Data.Keys).Concat(_ownerValues.Select(t => t.Key));
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            if (binder == null) {
                throw new ArgumentNullException("binder");
            }

            if (_selfValues.TryGetProperty(binder.Name, typeof(object), out result)) {
                return true;
            }

            bool flag = this.Data.TryGetValue(binder.Name, out result);

            if (!flag && this.Parent != null) {
                return this.Parent.TryGetMember(binder, out result);
            }

            if (!flag) {
                result = "undefined";
            }

            // TODO Friendly error message (currently, "TemplateContext doesn't contain a defintion")
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            if (binder == null) {
                throw new ArgumentNullException("binder");
            }

            if (_self.HasProperty(binder.Name)) {
                _self.SetProperty(binder.Name, value);
            } else {
                Data[binder.Name] = value;
            }

            return true;
        }

        public virtual HxlTemplateContext CreateChildContext(object owner) {
            return new HxlTemplateContext(owner) { Parent = this };
        }

        internal void SetMasterInfo(HxlMasterInfo masterInfo) {
            this._masterInfo = masterInfo;
        }

        private class BufferContent {

            public readonly string Name;
            public readonly StringWriter Writer;

            public BufferContent(string name) {
                this.Name = name;
                this.Writer = new SpaFragmentWriter();
            }
        }

        // HACK This wrapper writer facilitates a conversion to JSON content
        // for SPA fragments - Might not be needed if ITextOutput is API
        internal class SpaFragmentWriter : StringWriter {

            public string Name;

            public override string ToString() {
                var sb = new StringBuilder();
                sb.Append("{");
                sb.AppendFormat("\"name\":\"{0}\", \"location\":\"{1}\", \"html\":\"", Name, "-");

                string rawHtml = GetBaseHtmlWO();

                sb.Append(CodeUtility.Escape(rawHtml));
                sb.Append("\" }");
                return sb.ToString();
            }

            private string GetBaseHtmlWO() {
                // HACK We can't allow <body> because some browsers can't use document.write() or innerHTML= with it
                string rawHtml = base.ToString();
                rawHtml = Regex.Replace(rawHtml, @"^\s*<body>", "<body_>");
                rawHtml = Regex.Replace(rawHtml, @"^\s*<body\s", "<body_ ");
                return Regex.Replace(rawHtml, @"</body>\s*$", "</body_>");
            }
        }

        internal class PlaceholderContent {

            public readonly DomElement Element;
            public readonly string Layout;

            public PlaceholderContent(DomElement element, string layout) {
                this.Element = element;
                this.Layout = layout;
            }
        }

        internal PlaceholderContent FindPlaceholderContent(string name) {
            // TODO Could be multiple scheduled
            if (Parent != null) {
                var result = Parent.FindPlaceholderContent(name);
                if (result != null) {
                    return result;
                }
            }

            if (this._masterInfo != null) {
                var content = _masterInfo.PlaceholderContent.GetPlaceholderContent(name);
                if (content == null || !content.Any()) {
                    return null;
                }
                else
                    return new PlaceholderContent(content.First(), _masterInfo.LayoutName);
            }

            return null;
        }

        internal HxlMasterInfo FindMasterInfo() {
            if (_masterInfo != null) {
                return _masterInfo;
            }
            if (Parent == null) {
                return null;
            }

            return Parent.FindMasterInfo();
        }

        // TODO Could be API
        internal Properties GetElementData(bool createIfNecessary) {
            // TODO User could specify element data as a different type (rare)
            var elementData = (Properties) DataProviders["elementData"];

            if (elementData == null && createIfNecessary) {
                elementData = new Properties();
                DataProviders.AddNew("elementData", elementData);
            }

            return elementData;
        }

        private IList<BufferContent> EnsureBuffers() {
            return _buffers ?? (_buffers = new List<BufferContent>());
        }

        internal HxlWriter StartBufferContent(string name) {
            if (name == "spaFragments") {
                var result = new BufferContent(name);
                var writer = new HxlWriter(result.Writer, new HxlWriterSettings {
                    TemplateContext = this
                });
                StartBufferContent(result);
                return writer;
            }

            throw new NotImplementedException();
        }

        private void StartBufferContent(BufferContent result) {
            if (Parent == null) {
                // Always store buffer content at the root level

                EnsureBuffers().Add(result);

            } else {
                Parent.StartBufferContent(result);
            }
        }

        internal IEnumerable<StringWriter> EndBufferContent(string name) {
            if (_buffers == null) {
                yield break;
            }

            for (int i = 0; i < _buffers.Count; i++) {
                var item = _buffers[i];
                if (item.Name == name) {
                    yield return item.Writer;
                    _buffers.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
