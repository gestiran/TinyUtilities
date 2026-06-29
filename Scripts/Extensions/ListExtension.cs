// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace TinyUtilities.Extensions {
    public static class ListExtension {
        public static void Shuffle<T>(this List<T> list) {
            for (int i = 0; i < list.Count; i++) {
                int newIndex = Random.Range(0, list.Count);
                
                if (newIndex == i) {
                    continue;
                }
                
                (list[newIndex], list[i]) = (list[i], list[newIndex]);
            }
        }
        
        [Pure]
        public static T Any<T>(this List<T> list, T defaultValue = default) {
            if (list.Count > 0) {
                return list[Random.Range(0, list.Count - 1)];
            }
            
            return defaultValue;
        }
    }
}