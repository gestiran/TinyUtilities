// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;
using UnityEditor;

namespace TinyUtilities.Editor.AssetProcessors.CollidersImport {
    public sealed class CollidersImportPrefs {
        private const string _IS_ENABLE_PROCESSOR = "CollidersImport_IsEnableProcessor";
        
        public void SaveIsEnable(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_IS_ENABLE_PROCESSOR}", value);
        }
        
        public bool LoadIsEnable() {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_IS_ENABLE_PROCESSOR}", true);
        }
    }
}