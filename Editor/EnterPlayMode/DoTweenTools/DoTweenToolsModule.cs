using DG.Tweening;
using TinyUtilities.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.EnterPlayMode.DoTweenTools {
    public sealed class DoTweenToolsModule {
        private bool _isEnable;
        
        private readonly DoTweenToolsPrefs _prefs;

        public DoTweenToolsModule() {
            _prefs = new DoTweenToolsPrefs();
            Init();
        }
        
        public void Init() => _isEnable = _prefs.LoadIsEnable();

        public void PlayModeStateChanged(PlayModeStateChange state) {
            if (_isEnable == false) {
                return;
            }
            
            if (state != PlayModeStateChange.ExitingPlayMode) {
                return;
            }
            
            StopTween();
        }

        public void Draw() {
            _isEnable = GUIDrawUtility.DrawToggle("Restart DoTween", _isEnable, _prefs.SaveIsEnable);
        }
        
        private static void StopTween() {
            DOTween.ClearCachedTweens();
            DOTween.Clear(true);
            Application.Quit(0);
        }
    }
}