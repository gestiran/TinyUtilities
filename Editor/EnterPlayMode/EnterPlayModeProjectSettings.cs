using System.Collections.Generic;
using TinyUtilities.Editor.EnterPlayMode.AsyncTools;
using TinyUtilities.Editor.EnterPlayMode.BeforePlayMode;
using TinyUtilities.Editor.EnterPlayMode.DoTweenTools;
using UnityEditor;

namespace TinyUtilities.Editor.EnterPlayMode {
    [InitializeOnLoad]
    public static class EnterPlayModeProjectSettings {
        private static readonly BootSceneModule _bootScene;
        private static readonly AsyncToolsModule _asyncTools;
        private static readonly DoTweenToolsModule _doTweenTools;

        static EnterPlayModeProjectSettings() {
            _asyncTools = new AsyncToolsModule();
            _doTweenTools = new DoTweenToolsModule();
            _bootScene = new BootSceneModule();
            
            LoadStartState();
            
            EditorApplication.playModeStateChanged += PlayModeStateChanged;
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider() {
            SettingsProvider provider = new SettingsProvider("Project/Editor/Before Play Mode", SettingsScope.Project);

            provider.label = "Before Play Mode";
            provider.guiHandler = OnDrawSettings;
            provider.keywords = new HashSet<string>(new[] { "Async", "DoTween" });

            LoadStartState();

            return provider;
        }

        private static void LoadStartState() {
            _bootScene.Init();
            _asyncTools.Init();
            _doTweenTools.Init();
        }

        private static void PlayModeStateChanged(PlayModeStateChange state) {
            _asyncTools.PlayModeStateChanged(state);
            _doTweenTools.PlayModeStateChanged(state);
        }

        private static void OnDrawSettings(string obj) {
            _bootScene.Draw();
            _asyncTools.Draw();
            _doTweenTools.Draw();
        }
    }
}