// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using TinyUtilities.Extensions.Global;
using UnityEditor;
using UnityEngine;

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
        
        public static string DrawTextField(string value, out bool isDirty) {
            string newValue = EditorGUILayout.TextField(value);
            
            if (newValue == value) {
                isDirty = false;
                return value;
            }
            
            isDirty = true;
            return newValue;
        }
        
        public static string[] DrawList(string label, string[] values, Action<string[]> save) {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField($"{label} ({values.Length} objects)");
            
            if (GUILayout.Button("Add", GUILayout.Width(80))) {
                values = values.AddToArray("");
                save(values);
            }
            
            EditorGUILayout.EndHorizontal();
            
            for (int i = 0; i < values.Length; i++) {
                EditorGUILayout.BeginHorizontal();
                
                values[i] = DrawTextField(values[i], out bool isDirty);
                
                if (isDirty) {
                    save(values);
                    EditorGUILayout.EndHorizontal();
                }
                
                if (GUILayout.Button("Remove", GUILayout.Width(80))) {
                    values = values.RemoveFromArray(i);
                    save(values);
                    EditorGUILayout.EndHorizontal();
                    break;
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();
            
            return values;
        }
    }
}