// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;
using UnityEditor;

namespace TinyUtilities.Editor.EnterPlayMode.AsyncTools {
    public sealed class AsyncToolsPrefs {
        private const string _IS_ENABLE_BOOT_SCENE = "AsyncTools_IsEnableAsyncStop";
        
        public void SaveIsEnable(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_IS_ENABLE_BOOT_SCENE}", value);
        }
        
        public bool LoadIsEnable() {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_IS_ENABLE_BOOT_SCENE}", true);
        }
    }
}