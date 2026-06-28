// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;
using UnityEditor;

namespace TinyUtilities.Editor.EnterPlayMode.BeforePlayMode {
    public sealed class BeforePlayModePrefs {
        private const string _ENABLE = "BeforePlayMode_IsEnable";
        private const string _BOOT_SCENE_ID = "BeforePlayMode_DefaultBootSceneId";
        
        public void SaveEnable(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_ENABLE}", value);
        }
        
        public void SaveBootSceneID(int id) {
            EditorPrefs.SetInt($"{ProjectUtility.project}_{_BOOT_SCENE_ID}", id);
        }
        
        public bool LoadEnable() {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_ENABLE}", true);
        }
        
        public int LoadBootSceneId() {
            return EditorPrefs.GetInt($"{ProjectUtility.project}_{_BOOT_SCENE_ID}", 0);
        }
    }
}