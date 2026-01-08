// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;
using UnityEditor;

namespace TinyUtilities.Editor.AssetProcessors.CollidersImport {
    public sealed class CollidersImportPrefs {
        private const string _IS_ENABLE_PROCESSOR = "CollidersImport_IsEnableProcessor";
        private const string _IS_LAYER_OVERRIDE = "CollidersImport_IsLayerOverride";
        private const string _LAYER = "CollidersImport_Layer";
        
        public void SaveIsEnable(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_IS_ENABLE_PROCESSOR}", value);
        }
        
        public void SaveIsLayerOverride(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_IS_LAYER_OVERRIDE}", value);
        }
        
        public void SaveLayer(int value) {
            EditorPrefs.SetInt($"{ProjectUtility.project}_{_LAYER}", value);
        }
        
        public bool LoadIsEnable(bool defaultValue) {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_IS_ENABLE_PROCESSOR}", defaultValue);
        }
        
        public bool LoadIsLayerOverride(bool defaultValue) {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_IS_LAYER_OVERRIDE}", defaultValue);
        }
        
        public int LoadLayer(int defaultValue) {
            return EditorPrefs.GetInt($"{ProjectUtility.project}_{_LAYER}", defaultValue);
        }
    }
}