#region License

// Copyright 2009 Jeremy Skinner (http://www.jeremyskinner.co.uk)
//  
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//  
// http://www.apache.org/licenses/LICENSE-2.0 
//  
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/Spectre

#endregion

namespace Spectre.Core {
	using System.Collections.Generic;

	internal static class Extensions {
		public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) {
			TValue value;
			if(dictionary.TryGetValue(key, out value)) {
				return value;
			}

			return default(TValue);
		}

		public static TValue ObtainAndRemove<TValue>(this Boo.Lang.Hash hash, string key, TValue defaultValue) {
			if(hash.ContainsKey(key)) {
				object value = hash[key];
				hash.Remove(key);
				if(value is TValue) {
					return (TValue) value;
				}
				return defaultValue;
			}
			return defaultValue;
		}

	}
}