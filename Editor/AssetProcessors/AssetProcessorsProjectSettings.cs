// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using TinyUtilities.Editor.AssetProcessors.CollidersImport;
using TinyUtilities.Editor.AssetProcessors.LayerChange;
using TinyUtilities.Editor.AssetProcessors.ShadowsImport;
using UnityEditor;

namespace TinyUtilities.Editor.AssetProcessors {
    [InitializeOnLoad]
    public static class AssetProcessorsProjectSettings {
        private static readonly CollidersImportModule _collidersImport;
        private static readonly ShadowsImportModule _shadowsImport;
        private static readonly LayerChangeModule _layerChange;
        
        static AssetProcessorsProjectSettings() {
            _collidersImport = new CollidersImportModule();
            _shadowsImport = new ShadowsImportModule();
            _layerChange = new LayerChangeModule();
            
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
            _shadowsImport.Init();
            _layerChange.Init();
        }
        
        private static void OnDrawSettings(string obj) {
            _collidersImport.Draw();
            EditorGUILayout.Space();
            _shadowsImport.Draw();
            EditorGUILayout.Space();
            _layerChange.Draw();
        }
    }
}