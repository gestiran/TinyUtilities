// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if UNITY_ENGINE
using TinyUtilities.Editor.Utilities;
using UnityEditor;

namespace TinyUtilities.Editor.GridSnapping {
    internal sealed class GridSnappingPrefs {
        private const string _ENABLE = "GridSnapping_IsEnable";
        
        public void SaveEnable(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_ENABLE}", value);
        }
        
        public bool LoadEnable() {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_ENABLE}", false);
        }
    }
}
#endif