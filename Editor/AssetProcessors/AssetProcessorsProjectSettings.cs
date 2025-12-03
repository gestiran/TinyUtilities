// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using TinyUtilities.Editor.AssetProcessors.CollidersImport;
using UnityEditor;

namespace TinyUtilities.Editor.AssetProcessors {
    [InitializeOnLoad]
    public static class AssetProcessorsProjectSettings {
        private static readonly CollidersImportModule _collidersImport;
        
        static AssetProcessorsProjectSettings() {
            _collidersImport = new CollidersImportModule();
            
            LoadStartState();
        }
        
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider() {
            SettingsProvider provider = new SettingsProvider("Project/Editor/Asset Processors", SettingsScope.Project);
            
            provider.label = "Asset Processors";
            provider.guiHandler = OnDrawSettings;
            provider.keywords = new HashSet<string>(new[] { "Asset", "Import", "AssetProcessors", "Processors" });
            
            LoadStartState();
            
            return provider;
        }
        
        private static void LoadStartState() {
            _collidersImport.Init();
        }
        
        private static void OnDrawSettings(string obj) {
            _collidersImport.Draw();
        }
    }
}