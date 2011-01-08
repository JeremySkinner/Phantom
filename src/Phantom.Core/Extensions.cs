#region License

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk) and Contributors
// 
// Licensed under the Microsoft Public License. You may
// obtain a copy of the license at:
// 
// http://www.microsoft.com/opensource/licenses.mspx
// 
// By using this source code in any fashion, you are agreeing
// to be bound by the terms of the Microsoft Public License.
// 
// You must not remove this notice, or any other, from this software.

#endregion

namespace Phantom.Core {
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	internal static class Extensions {
		public static TValue ValueOrDefault<TValue>(this IDictionary hash, string key, TValue defaultValue) {
			if (hash.Contains(key)) {
				object value = hash[key];
				if (value is TValue) {
					return (TValue)value;
				}
				return defaultValue;
			}
			return defaultValue;
		}

		public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) {
			TValue value;
			if (dictionary.TryGetValue(key, out value)) {
				return value;
			}

			return default(TValue);
		}

		public static string JoinWith(this IEnumerable<string> strings, string separator) {
			return string.Join(separator, strings.ToArray());
		}
	}
}