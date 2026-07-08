// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.Odin {
    public static class OdinGUI {
        private static readonly Color _inlineTextColor;
        
        static OdinGUI() {
            _inlineTextColor = new Color(1f, 1f, 1f, 0.75f);
        }
        
        public static int DrawWithSuffixInline(string suffix, int value) {
            Rect rect = EditorGUILayout.GetControlRect(false, GUILayout.ExpandWidth(true));
            
            int newValue = SirenixEditorFields.IntField(rect, value);
            
            GUIStyle suffixStyle = new GUIStyle(EditorStyles.miniLabel) {
                alignment = TextAnchor.MiddleRight,
                normal = { textColor = _inlineTextColor }
            };
            
            Rect suffixRect = new Rect(rect.x, rect.y, rect.width - 8f, rect.height);
            
            GUI.enabled = false;
            GUI.Label(suffixRect, suffix, suffixStyle);
            GUI.enabled = true;
            
            return newValue;
        }
    }
}