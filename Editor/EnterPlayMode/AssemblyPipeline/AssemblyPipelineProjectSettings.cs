using System.Collections.Generic;
using UnityEditor;

namespace TinyUtilities.Editor.EnterPlayMode.AssemblyPipeline {
    [InitializeOnLoad]
    public static class AssemblyPipelineProjectSettings {
        private static bool _isEnable;
        private static bool _isNeedReload;

        private static readonly AssemblyPipelinePrefs _prefs;

        static AssemblyPipelineProjectSettings() {
            _prefs = new AssemblyPipelinePrefs();
            _isEnable = _prefs.LoadEnable();

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

            _isEnable = _prefs.LoadEnable();

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
            bool newValue = EditorGUILayout.Toggle("Auto Reload Assembly", _isEnable);
            
            if (newValue == _isEnable) {
                return;
            }
            
            _isEnable = newValue;
            _prefs.SaveEnable(newValue);
        }
        
        private static void Activate() {
            if (_isEnable == false) {
                return;
            }
            
            _isNeedReload = false;
            EditorApplication.playModeStateChanged += OnStateChanged;
        }

        private static void OnStateChanged(PlayModeStateChange state) {
            if (state == PlayModeStateChange.ExitingEditMode) {
                if (_isNeedReload == false) {
                    return;
                }

                ForceReloading();
            } else if (state == PlayModeStateChange.EnteredEditMode) {
                _isNeedReload = true;
            }
        }
    }
}