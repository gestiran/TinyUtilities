// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityObject = UnityEngine.Object;

namespace TinyUtilities.Extensions {
    public static class UnityObjectExtension {
        public static void TrySetDirty<T>(this T obj) where T : UnityObject {
        #if UNITY_EDITOR
            if (obj != null) {
                UnityEditor.EditorUtility.SetDirty(obj);
            }
        #endif
        }
    }
}