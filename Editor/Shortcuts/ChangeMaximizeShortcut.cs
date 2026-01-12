// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using UnityEditor;

namespace TinyUtilities.Editor.Shortcuts {
    public static class ChangeMaximizeShortcut {
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