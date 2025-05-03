using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TinyUtilities.Editor.EnterPlayMode.BeforePlayMode {
    public sealed class BootSceneModule {
        private bool _isEnable;
        private int _bootSceneId;

        private readonly BeforePlayModePrefs _prefs;

        public BootSceneModule() {
            _prefs = new BeforePlayModePrefs();
            
            Init();
            RefreshBootSceneSettings();
        }

        public void Init() {
            _isEnable = _prefs.LoadEnable();
            _bootSceneId = _prefs.LoadBootSceneId();
        }

        public void Draw() {
            bool newIsEnableBootScene = EditorGUILayout.Toggle("Is Enable Boot Scene", _isEnable);

            if (newIsEnableBootScene != _isEnable) {
                _isEnable = newIsEnableBootScene;
                _prefs.SaveEnable(_isEnable);
                RefreshBootSceneSettings();
            }

            DrawBootSceneConfig(_isEnable);
        }

        private void DrawBootSceneConfig(bool isEnable) {
            string[] scenesNamesInBuildSettings = new string[EditorBuildSettings.scenes.Length];
            int[] scenesIdsInBuildSettings = new int[EditorBuildSettings.scenes.Length];

            for (int sceneId = 0; sceneId < scenesNamesInBuildSettings.Length; sceneId++) {
                scenesNamesInBuildSettings[sceneId] = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[sceneId].path).name;
                scenesIdsInBuildSettings[sceneId] = sceneId;
            }

            GUI.enabled = isEnable;
            int newBootSceneId = EditorGUILayout.IntPopup("\tBoot Scene Id", _bootSceneId, scenesNamesInBuildSettings, scenesIdsInBuildSettings);
            GUI.enabled = true;

            if (newBootSceneId != _bootSceneId) {
                _bootSceneId = newBootSceneId;
                _prefs.SaveBootSceneID(_bootSceneId);
                RefreshBootSceneSettings();
            }
        }

        private void RefreshBootSceneSettings() {
            if (_isEnable) {
                EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[_bootSceneId].path);
            } else {
                EditorSceneManager.playModeStartScene = null;
            }
        }
    }
}