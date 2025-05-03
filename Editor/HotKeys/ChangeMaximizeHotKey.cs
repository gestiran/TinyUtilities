using System;
using UnityEditor;

namespace TinyUtilities.Editor.HotKeys {
    public static class ChangeMaximizeHotKey {
        [MenuItem("Window/Change Maximize State")]
        public static void ChangeMaximizeState() {
            EditorWindow window = EditorWindow.focusedWindow;
            
            if (window == null) {
                return;
            }
            
            try {
                window.maximized = !window.maximized;
            } catch (Exception) {
                // Do nothing
            }
        }
    }
}