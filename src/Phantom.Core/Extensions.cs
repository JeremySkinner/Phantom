#region License

// Copyright Jeremy Skinner (http://www.jeremyskinner.co.uk)
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
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/Phantom

#endregion

namespace Phantom.Core {
	using System.Collections.Generic;
	using Boo.Lang;

	internal static class Extensions {
		public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) {
			TValue value;
			if (dictionary.TryGetValue(key, out value)) {
				return value;
			}

			return default(TValue);
		}

		public static TValue ObtainAndRemove<TValue>(this Hash hash, string key, TValue defaultValue) {
			if (hash.ContainsKey(key)) {
				object value = hash[key];
				hash.Remove(key);
				if (value is TValue) {
					return (TValue) value;
				}
				return defaultValue;
			}
			return defaultValue;
		}
	}
}