// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.Odin {
    public sealed class DateTimeDrawer : OdinValueDrawer<DateTime> {
        protected override void DrawPropertyLayout(GUIContent label) {
            DateTime date = ValueEntry.SmartValue;
            
            SirenixEditorGUI.BeginHorizontalPropertyLayout(label);
            
            EditorGUI.BeginChangeCheck();
            
            int year = OdinGUI.DrawWithSuffixInline("Year", date.Year);
            int month = OdinGUI.DrawWithSuffixInline("Month", date.Month);
            int day = OdinGUI.DrawWithSuffixInline("Day", date.Day);
            int hour = OdinGUI.DrawWithSuffixInline("Hour", date.Hour);
            int minute = OdinGUI.DrawWithSuffixInline("Minute", date.Minute);
            int second = OdinGUI.DrawWithSuffixInline("Second", date.Second);
            
            if (EditorGUI.EndChangeCheck()) {
                try {
                    ValueEntry.SmartValue = new DateTime(year, month, day, hour, minute, second);
                } catch (Exception exception) {
                    Debug.LogError(exception);
                }
            }
            
            SirenixEditorGUI.EndHorizontalPropertyLayout();
        }
    }
}