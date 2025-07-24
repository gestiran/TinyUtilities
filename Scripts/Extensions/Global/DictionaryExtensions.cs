// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;

namespace TinyUtilities.Extensions.Global {
    public static class DictionaryExtensions {
        public static void AddOrSet<T>(this Dictionary<T, int> source, T key, int value) {
            if (!source.TryAdd(key, value)) {
                source[key] += value;
            }
        }
    }
}