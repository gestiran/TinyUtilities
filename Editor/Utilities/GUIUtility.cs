// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

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
        
        public static int DrawLayer(string label, int value, Action<int> save) {
            int newValue = EditorGUILayout.LayerField(label, value);
            
            if (newValue == value) {
                return value;
            }
            
            save(newValue);
            return newValue;
        }
    }
}