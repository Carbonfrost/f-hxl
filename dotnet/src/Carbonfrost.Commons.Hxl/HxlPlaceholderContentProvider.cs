//
// - HxlPlaceholderContentProvider.cs -
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
using System.IO;
using System.Linq;
using Carbonfrost.Commons.Web.Dom;

namespace Carbonfrost.Commons.Hxl {

    public class HxlPlaceholderContentProvider : IHxlPlaceholderContentProvider {

        private readonly IDictionary<string, DomElement> _values
            = new Dictionary<string, DomElement>();

        public static readonly IHxlPlaceholderContentProvider Null
            = new NullImpl();

        private HxlPlaceholderContentProvider(IEnumerable<DomElement> descendents) {
            foreach (var child in descendents) {
                var attr = GetImpliedPlaceholderName(child, child.Attribute("hxl:placeholdertarget"));

                // TODO Validate placeholder target name
                // TODO Allow multiple if placeholder supports it
                if (_values.ContainsKey(attr))
                    continue; // TODO Should aggregate

                _values.Add(attr, (DomElement) child);
            }
        }

        public HxlPlaceholderContentProvider(DomElement element) : this(FindDescendants(element)) {
        }

        public static IHxlPlaceholderContentProvider FromElement(DomElement element) {
            if (element == null)
                throw new ArgumentNullException("element");

            var descendents = FindDescendants(element);
            if (descendents.Any())
                return new HxlPlaceholderContentProvider(descendents);
            else
                return Null;
        }

        internal static void Render(DomElement element,
                                    string name,
                                    string layoutName,
                                    HxlWriter hxlOutput) {
            var templateContext = hxlOutput.TemplateContext;
            var pc = templateContext.FindPlaceholderContent(name);
            DomElement content;

            if (pc == null) {
                content = element;

            } else {
                content = pc.Element;
                layoutName = pc.Layout;
                MergeAttributes(content, element);
            }

            var attr = element.Attributes["class"];
            if (attr == null)
                element.Attribute("class", "app-placeholder");
            else
                AttributeAppend(attr, "app-placeholder");

            element.Attribute("data-placeholder", name);
            element.Attribute("data-layout", layoutName);

            // TODO Skip over single-page elements that were disabled
            // TODO Output correct locations
            var output = hxlOutput.BaseWriter;

            // TODO We always render element - should attribute selection be allowed to choose another element template?
            HxlElementTemplate.ProcessAttributesForTemplate(element, templateContext);
            HxlElementTemplate.RenderElementStart(element, output);

            hxlOutput.Write(content.ChildNodes);
            HxlElementTemplate.RenderElementEnd(element, output);
        }

        public IEnumerable<string> PlaceholderContentNames {
            get {
                return _values.Keys;
            }
        }

        public IEnumerable<DomElement> GetPlaceholderContent(string name) {
            var item = _values.GetValueOrDefault(name);
            if (item == null)
                return Empty<DomElement>.List;
            else
                return new [] { item };
        }

        static IEnumerable<DomElement> FindDescendants(DomElement element) {
            if (element == null)
                throw new ArgumentNullException("element");

            return element.Descendants.Where(t => t.HasAttribute("hxl:placeholdertarget"));
        }

        internal static string GetImpliedPlaceholderName(DomElement e, string explicitName) {
            if (!string.IsNullOrWhiteSpace(explicitName))
                return explicitName;

            if (e == null)
                return null;

            string id = e.Attribute("id");

            if (!string.IsNullOrEmpty(id))
                return id;

            return e.NodeName;
        }

        // TODO There might be other merging attributes (rare)

        private static bool IsMerging(string attribName) {
            return attribName == "class";
        }

        private static bool IsLayoutAttr(DomAttribute attr) {
            return attr.Name == "hxl:layout"
                || attr.Name == "hxl:placeholder"
                || attr.Name == "hxl:placeholdertarget";
        }

        internal static void MergeAttributes(DomElement fromElement,
                                             DomElement toElement) {

            // TODO Use of attr.Name here might not respect xmlns (rare)
            // Merge attributes
            foreach (var attr in fromElement.Attributes) {

                // Don't copy layout attributes
                if (IsLayoutAttr(attr)) {
                    continue;
                }

                var current = toElement.Attributes[attr.Name];

                if (current == null) {
                    toElement.Attributes.Add(attr.Clone());

                } else if (IsMerging(current.Name)) {
                    AttributeAppend(current, attr);

                } else {
                    toElement.Attributes.Remove(attr.Name);
                    toElement.Attributes.Add(attr.Clone());
                }
            }
        }

        // Appends the given value to the attribute which uses a
        // merging semantic like "class"
        private static void AttributeAppend(DomAttribute left,
                                            string appendValue) {
            var toElement = left.OwnerElement;
            var thunkFragment = left as HxlAttribute.ThunkFragment;

            if (thunkFragment != null) {
                toElement.RemoveAttribute(left.Name);

                var combo = HxlAttribute.Combine(
                    left.Name,
                    thunkFragment._action,
                    (x, y) => appendValue
                );
                toElement.Append(combo);
            }

            else if (!string.IsNullOrWhiteSpace(left.Value))
                left.Value += " " + appendValue;

            else
                left.Value = appendValue;
        }

        private static void AttributeAppend(DomAttribute left,
                                            DomAttribute append) {
            var toElement = left.OwnerElement;
            var appendThunkFrag = append as HxlAttribute.ThunkFragment;

            if (appendThunkFrag == null)
                AttributeAppend(left, append.Value);

            else {
                toElement.Attributes.Remove(append.Name);
                Func<dynamic, HxlAttribute, string> thunkLeft;

                var leftThunkFrag = left as HxlAttribute.ThunkFragment;
                if (leftThunkFrag == null) {
                    // No need to create a closure on the whole attribute
                    string leftValue = left.Value;
                    thunkLeft = (x, y) => leftValue;

                } else {
                    thunkLeft = leftThunkFrag._action;
                }

                var attr = HxlAttribute.Combine(left.Name, thunkLeft, appendThunkFrag._action);
                toElement.Append(attr);
            }
        }

        class NullImpl : IHxlPlaceholderContentProvider {

            public IEnumerable<string> PlaceholderContentNames {
                get {
                    return Enumerable.Empty<string>();
                }
            }

            public IEnumerable<DomElement> GetPlaceholderContent(string name) {
                return Empty<DomElement>.List;
            }

        }

    }
}
