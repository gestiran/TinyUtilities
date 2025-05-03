using System;
using UnityEditor;

namespace TinyUtilities.Editor.Utilities {
    public static class GUIDrawUtility {
        public static bool DrawToggle(string label, bool value, Action<bool> save) {
            bool newValue = EditorGUILayout.Toggle(label, value);

            if (newValue == value) {
                return value;
            }

            save(newValue);
            return newValue;
        }
    }
}