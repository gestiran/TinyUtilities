// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor {
    public static class SelectionUtility {
        [Pure]
        public static List<T> GetComponents<T>(bool includeInactive = false) {
            GameObject[] objects = Selection.gameObjects;
            List<T> result = new List<T>(objects.Length);
            
            for (int objId = 0; objId < objects.Length; objId++) {
                result.AddRange(objects[objId].GetComponentsInChildren<T>(includeInactive));
            }
            
            return result;
        }
    }
}