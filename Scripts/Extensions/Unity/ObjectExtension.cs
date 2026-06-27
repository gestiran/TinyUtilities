// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if UNITY_ENGINE
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class ObjectExtension {
        public static void TrySetDirty<T>(this T obj) where T : Object {
        #if UNITY_EDITOR
            if (obj != null) {
                UnityEditor.EditorUtility.SetDirty(obj);
            }
        #endif
        }
    }
}
#endif