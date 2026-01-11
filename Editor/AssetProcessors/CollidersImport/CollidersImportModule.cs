// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;
using UnityEngine;

namespace TinyUtilities.Editor.AssetProcessors.CollidersImport {
    public sealed class CollidersImportModule {
        public static bool isEnable { get; private set; }
        public static bool isEnableShadow { get; private set; }
        public static bool overrideLayer { get; private set; }
        public static int layer { get; private set; }
        
        private readonly CollidersImportPrefs _prefs;
        
        public CollidersImportModule() {
            _prefs = new CollidersImportPrefs();
            Init();
        }
        
        public void Init() {
            isEnable = _prefs.LoadIsEnable(false);
            isEnableShadow = _prefs.LoadIsEnableShadow(false);
            overrideLayer = _prefs.LoadIsLayerOverride(false);
            layer = _prefs.LoadLayer(LayerMask.NameToLayer("Default"));
        }
        
        public void Draw() {
            isEnable = GUIDrawUtility.DrawToggle("Import colliders", isEnable, _prefs.SaveIsEnable);
            isEnableShadow = GUIDrawUtility.DrawToggle("Import shadow", isEnableShadow, _prefs.SaveIsEnableShadow);
            
            GUI.enabled = isEnable;
            overrideLayer = GUIDrawUtility.DrawToggle("Override layer", overrideLayer, _prefs.SaveIsLayerOverride);
            
            GUI.enabled = isEnable && overrideLayer;
            layer = GUIDrawUtility.DrawLayer("Layer", layer, _prefs.SaveLayer);
            
            GUI.enabled = true;
        }
    }
}