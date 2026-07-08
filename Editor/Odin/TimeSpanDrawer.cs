// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.Odin {
    public sealed class TimeSpanDrawer : OdinValueDrawer<TimeSpan> {
        protected override void DrawPropertyLayout(GUIContent label) {
            TimeSpan time = ValueEntry.SmartValue;
            
            SirenixEditorGUI.BeginHorizontalPropertyLayout(label);
            
            EditorGUI.BeginChangeCheck();
            
            int days = OdinGUI.DrawWithSuffixInline("Day", time.Days);
            int hours = OdinGUI.DrawWithSuffixInline("Hour", time.Hours);
            int minutes = OdinGUI.DrawWithSuffixInline("Min", time.Minutes);
            int seconds = OdinGUI.DrawWithSuffixInline("Sec", time.Seconds);
            
            if (EditorGUI.EndChangeCheck()) {
                try {
                    ValueEntry.SmartValue = new TimeSpan(days, hours, minutes, seconds);
                } catch (Exception exception) {
                    Debug.LogError(exception);
                }
            }
            
            SirenixEditorGUI.EndHorizontalPropertyLayout();
        }
    }
}