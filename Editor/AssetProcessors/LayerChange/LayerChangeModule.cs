// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.AssetProcessors.LayerChange {
    public sealed class LayerChangeModule {
        public static bool isEnabled { get; private set; }
        public static int layer { get; private set; }
        
        private readonly AssetProcessorsPrefs _prefs;
        
        public LayerChangeModule() {
            _prefs = new AssetProcessorsPrefs();
            Init();
        }
        
        public void Init() {
            isEnabled = _prefs.LoadIsLayerOverride(false);
            layer = _prefs.LoadLayer(LayerMask.NameToLayer("Default"));
        }
        
        public void Draw() {
            EditorGUILayout.LabelField($"Override default layer ({(isEnabled ? "Enabled" : "Disabled")})", EditorStyles.boldLabel);
            
            GUIContent isEnableLabel = new GUIContent("Enabled");
            isEnableLabel.tooltip = "Enable layer override post processors.";
            isEnabled = GUIDrawUtility.DrawToggle(isEnableLabel, isEnabled, _prefs.SaveIsLayerOverride);
            GUI.enabled = isEnabled;
            layer = GUIDrawUtility.DrawLayer("Default Layer", layer, _prefs.SaveLayer);
            GUI.enabled = true;
        }
    }
}