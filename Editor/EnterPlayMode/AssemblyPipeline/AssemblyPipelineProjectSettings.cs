// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEditor;

namespace TinyUtilities.Editor.EnterPlayMode.AssemblyPipeline {
    [InitializeOnLoad]
    public static class AssemblyPipelineProjectSettings {
        public static bool isEnable { get; private set; }
        public static bool isNeedReload { get; private set; }
        
        private static readonly AssemblyPipelinePrefs _prefs;

        static AssemblyPipelineProjectSettings() {
            _prefs = new AssemblyPipelinePrefs();
            isEnable = _prefs.LoadEnable();

            if (EditorSettings.enterPlayModeOptions == EnterPlayModeOptions.DisableDomainReload) {
                Activate();
            } else if (EditorSettings.enterPlayModeOptions == (EnterPlayModeOptions.DisableDomainReload | EnterPlayModeOptions.DisableSceneReload)) {
                Activate();
            }
        }
        
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider() {
            SettingsProvider provider = new SettingsProvider("Project/Editor/Assembly Pipeline", SettingsScope.Project);

            provider.label = "Assembly Pipeline";
            provider.guiHandler = OnDrawSettings;
            provider.keywords = new HashSet<string>(new[] { "Assembly" });

            isEnable = _prefs.LoadEnable();

            return provider;
        }
        
        [MenuItem("Assets/Refresh Assembly", false, 39)]
        public static void ForceReloading() {
            AssetDatabase.RefreshSettings();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
            EditorUtility.RequestScriptReload();
        }
        
        private static void OnDrawSettings(string obj) {
            bool newValue = EditorGUILayout.Toggle("Auto Reload Assembly", isEnable);
            
            if (newValue != isEnable) {
                isEnable = newValue;
                _prefs.SaveEnable(newValue);
            }
        }
        
        private static void Activate() {
            if (isEnable) {
                isNeedReload = false;
                EditorApplication.playModeStateChanged += OnStateChanged;
            }
        }

        private static void OnStateChanged(PlayModeStateChange state) {
            if (state == PlayModeStateChange.ExitingEditMode) {
                if (isNeedReload) {
                    ForceReloading();
                }
            } else if (state == PlayModeStateChange.EnteredEditMode) {
                isNeedReload = true;
            }
        }
    }
}