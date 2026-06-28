// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Text;
using TinyUtilities.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.AssetProcessors.ShadowsImport {
    public sealed class ShadowsImportModule {
        public static bool isEnabled { get; private set; }
        public static string[] stripPrefixes { get; private set; }
        
        private readonly AssetProcessorsPrefs _prefs;
        
        public ShadowsImportModule() {
            _prefs = new AssetProcessorsPrefs();
            Init();
        }
        
        public void Init() {
            isEnabled = _prefs.LoadIsEnableShadow(false);
            stripPrefixes = _prefs.LoadStripPrefixes(Array.Empty<string>());
        }
        
        public void Draw() {
            EditorGUILayout.LabelField($"Shadows ({(isEnabled ? "Enabled" : "Disabled")})", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(HelpMessage(), MessageType.Info);
            
            GUIContent isEnableLabel = new GUIContent("Enabled");
            isEnableLabel.tooltip = "Enable shadows import post processors.";
            isEnabled = GUIDrawUtility.DrawToggle(isEnableLabel, isEnabled, _prefs.SaveIsEnableShadow);
            GUI.enabled = isEnabled;
            stripPrefixes = GUIDrawUtility.DrawList("Strip prefixes", stripPrefixes, _prefs.SaveStripPrefixes);
            GUI.enabled = true;
        }
        
        private string HelpMessage() {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Configures the custom ShadowOnly object. Disables the display of shadows on the parent or objects with the same name.");
            builder.AppendLine();
            builder.AppendLine("Used prefixes:");
            builder.AppendLine($"{ImportPrefixes.SHADOW_OBJECT} - Custom ShadowOnly object.");
            
            return builder.ToString();
        }
    }
}