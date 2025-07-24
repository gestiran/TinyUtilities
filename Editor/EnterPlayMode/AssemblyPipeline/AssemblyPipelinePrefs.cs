// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;
using UnityEditor;

namespace TinyUtilities.Editor.EnterPlayMode.AssemblyPipeline {
    public sealed class AssemblyPipelinePrefs {
        private const string _ENABLE = "AssemblyPipeline_IsEnable";
        
        public void SaveEnable(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_ENABLE}", value);
        }
        
        public bool LoadEnable() {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_ENABLE}", true);
        }
    }
}