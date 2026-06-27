// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if UNITY_ENGINE
using System.Collections.Generic;
using TinyUtilities.Editor.Utilities;
using UnityEditor;

namespace TinyUtilities.Editor.GridSnapping {
    [InitializeOnLoad]
    internal static class GridSnappingProjectSettings {
        public static bool isEnable { get; private set; }
        
        private static readonly GridSnappingPrefs _prefs;
        
        static GridSnappingProjectSettings() {
            _prefs = new GridSnappingPrefs();
            
            isEnable = _prefs.LoadEnable();
        }
        
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider() {
            SettingsProvider provider = new SettingsProvider("Project/Editor/Grid Snapping", SettingsScope.Project);
            
            provider.label = "Grid Snapping";
            provider.guiHandler = OnDrawSettings;
            provider.keywords = new HashSet<string>(new[] { "Snap", "Grid" });
            
            return provider;
        }
        
        private static void OnDrawSettings(string obj) {
            isEnable = GUIDrawUtility.DrawToggle("Adaptive Snap", isEnable, _prefs.SaveEnable);
        }
    }
}
#endif