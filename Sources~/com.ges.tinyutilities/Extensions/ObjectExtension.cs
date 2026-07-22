// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Diagnostics.Contracts;

namespace TinyUtilities.Extensions {
    /// <summary> Global object extension. </summary>
    public static class ObjectExtension {
        /// <summary> Returns the current object, if it exists, or create new. </summary>
        /// <param name="obj"> Current object. </param>
        /// <returns> Current or new object. </returns>
        [Pure]
        public static T Create<T>(this T obj) where T : new() {
            if (obj != null) {
                return obj;
            }
            
            return new T();
        }
    }
}