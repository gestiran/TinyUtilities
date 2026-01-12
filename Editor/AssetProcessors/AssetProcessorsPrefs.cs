// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using TinyUtilities.Editor.Utilities;
using UnityEditor;

#if NEWTONSOFT_JSON
using Newtonsoft.Json;
using UnityEngine;
#endif

namespace TinyUtilities.Editor.AssetProcessors {
    public sealed class AssetProcessorsPrefs {
        private const string _IS_ENABLE_PROCESSOR = "CollidersImport_IsEnableProcessor";
        private const string _IS_ENABLE_SHADOW_PROCESSOR = "CollidersImport_IsEnableShadowProcessor";
        private const string _IS_STRIP_PREFIXES = "CollidersImport_StripPrefixes";
        private const string _IS_LAYER_OVERRIDE = "CollidersImport_IsLayerOverride";
        private const string _LAYER = "CollidersImport_Layer";
        
        public void SaveIsEnable(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_IS_ENABLE_PROCESSOR}", value);
        }
        
        public void SaveIsEnableShadow(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_IS_ENABLE_SHADOW_PROCESSOR}", value);
        }
        
        public void SaveStripPrefixes(string[] value) {
        #if NEWTONSOFT_JSON
            string json = JsonConvert.SerializeObject(value);
        #else
            string json = EditorJsonUtility.ToJson(value);
        #endif
            
            EditorPrefs.SetString($"{ProjectUtility.project}_{_IS_STRIP_PREFIXES}", json);
        }
        
        public void SaveIsLayerOverride(bool value) {
            EditorPrefs.SetBool($"{ProjectUtility.project}_{_IS_LAYER_OVERRIDE}", value);
        }
        
        public void SaveLayer(int value) {
            EditorPrefs.SetInt($"{ProjectUtility.project}_{_LAYER}", value);
        }
        
        public bool LoadIsEnable(bool defaultValue) {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_IS_ENABLE_PROCESSOR}", defaultValue);
        }
        
        public bool LoadIsEnableShadow(bool defaultValue) {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_IS_ENABLE_SHADOW_PROCESSOR}", defaultValue);
        }
        
        public string[] LoadStripPrefixes(string[] defaultValue) {
            string key = $"{ProjectUtility.project}_{_IS_STRIP_PREFIXES}";
            
            if (EditorPrefs.HasKey(key) == false) {
                return defaultValue;
            }
            
            string json = EditorPrefs.GetString(key);
            
        #if NEWTONSOFT_JSON
            try {
                return JsonConvert.DeserializeObject<string[]>(json);
            } catch (Exception exception) {
                Debug.LogException(exception);
                return defaultValue;
            }
            
        #else
            EditorJsonUtility.FromJsonOverwrite(json, defaultValue);
            return defaultValue;
        #endif
        }
        
        public bool LoadIsLayerOverride(bool defaultValue) {
            return EditorPrefs.GetBool($"{ProjectUtility.project}_{_IS_LAYER_OVERRIDE}", defaultValue);
        }
        
        public int LoadLayer(int defaultValue) {
            return EditorPrefs.GetInt($"{ProjectUtility.project}_{_LAYER}", defaultValue);
        }
    }
}