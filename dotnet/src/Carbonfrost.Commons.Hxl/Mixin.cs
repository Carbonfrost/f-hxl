//
// - Mixin.cs -
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Carbonfrost.Commons.Web.Dom;
using Carbonfrost.Commons.Hxl.Controls;

namespace Carbonfrost.Commons.Hxl {

    static class Mixin {

        public static void AddMany<T>(this ICollection<T> items, IEnumerable<T> others) {
            foreach (var element in others) {
                items.Add(element);
            }
        }

        public static void TryAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> items,
            TKey key, TValue value, Action<TKey, TValue> dup) {

            if (items.ContainsKey(key))
                dup(key, value);
            else
                items.Add(key, value);
        }

        public static TValue GetValueOrCache<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                           TKey key,
                                                           Func<TKey, TValue> factory) {
            TValue value;
            if (source.TryGetValue(key, out value))
                return value;

            else {
                source.Add(key, value = factory(key));
            }
            return value;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                             TKey key) {
            TValue value;
            if (source.TryGetValue(key, out value))
                return value;
            else
                return default(TValue);
        }

        public static IEnumerable<T> WhereNonNull<T>(this IEnumerable<T> items)
            where T : class
        {
            return items.Where(t => t != null);
        }

        public static TResult FirstNonNull<T, TResult>(this IEnumerable<T> items, Func<T, TResult> func)
            where TResult : class
        {
            foreach (var element in items) {
                var result = func(element);
                if (result != null)
                    return result;
            }

            return null;
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> items, T u) {
            return items.Where(t => !object.Equals(t, u));
        }

        public static bool IsEmpty<T>(this ICollection<T> items) {
            return items.Count == 0;
        }

        public static T PeekOrDefault<T>(this Stack<T> items) {
            if (items.Count == 0)
                return default(T);
            else
                return items.Peek();
        }

        public static StringBuilder AppendSeparator(this StringBuilder sb, string sep) {
            if (sb.Length > 0)
                sb.Append(sep);

            return sb;
        }

        private static T GetEnum<T>(ulong value) {
            return (T) Enum.ToObject(typeof(T), value);
        }

        public static IEnumerable<Type> GetTypesHelper(this Assembly a) {
            try {
                return a.GetTypes();

            } catch (ReflectionTypeLoadException ex) {
                return ex.Types.WhereNonNull();
            }
        }

        public static T SetFlag<T>(this T self, T item, bool value) where T: struct, IConvertible, IFormattable {
            if (value)
                return GetEnum<T>(Convert.ToUInt64(self) | Convert.ToUInt64(item));
            else
                return GetEnum<T>(Convert.ToUInt64(self) & ~Convert.ToUInt64(item));
        }

    }
}
