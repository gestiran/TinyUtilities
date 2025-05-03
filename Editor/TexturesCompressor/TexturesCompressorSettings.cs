using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.TexturesCompressor {
    [InitializeOnLoad]
    public static class TexturesCompressorSettings {
        public static bool isNeedOverridenValidation { get; private set; }
        public static int maxTextureSize => (int)_maxTextureSize;
        public static TextureImporterFormat solidFormat { get; private set; }
        public static TextureImporterFormat alphaFormat { get; private set; }
        
        private static readonly string _projectKey;
        private static MaxTextureSize _maxTextureSize;
        
        private enum MaxTextureSize {
            _32 = 32,
            _64 = 64,
            _128 = 128,
            _256 = 256,
            _512 = 512,
            _1024 = 1024,
            _2048 = 2048,
            _4096 = 4096,
        }
        
        static TexturesCompressorSettings() {
            _projectKey = Path.GetFileName(Path.GetDirectoryName(Application.dataPath));
            LoadStartState();
        }
        
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider() {
            SettingsProvider provider = new SettingsProvider("Project/Player/Texture Compression", SettingsScope.Project);
            
            provider.label = "Texture Compression";
            provider.guiHandler = OnDrawSettings;
            provider.keywords = new HashSet<string>(new[] { "Texture", "Compression" });
            
            LoadStartState();
            
            return provider;
        }
        
        private static void LoadStartState() {
            isNeedOverridenValidation = EditorPrefs.GetBool($"{_projectKey}_{nameof(isNeedOverridenValidation)}", true);
            _maxTextureSize = (MaxTextureSize)EditorPrefs.GetInt($"{_projectKey}_{nameof(maxTextureSize)}", (int)MaxTextureSize._2048);
            solidFormat = (TextureImporterFormat)EditorPrefs.GetInt($"{_projectKey}_{nameof(solidFormat)}", (int)TextureImporterFormat.ETC2_RGB4);
            alphaFormat = (TextureImporterFormat)EditorPrefs.GetInt($"{_projectKey}_{nameof(alphaFormat)}", (int)TextureImporterFormat.ETC2_RGBA8);
        }
        
        private static void OnDrawSettings(string _) {
            bool isNeedOverridenValidationTemp = EditorGUILayout.Toggle("Overriden Validation", isNeedOverridenValidation);
            EditorGUILayout.Space();
            
            MaxTextureSize maxTextureSizeTemp = (MaxTextureSize)EditorGUILayout.EnumPopup("Max Size", _maxTextureSize);
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Formats:", EditorStyles.boldLabel);
            TextureImporterFormat solidFormatTemp = (TextureImporterFormat)EditorGUILayout.EnumPopup("Solid (no alpha)", solidFormat);
            TextureImporterFormat alphaFormatTemp = (TextureImporterFormat)EditorGUILayout.EnumPopup("Alpha (has alpha)", alphaFormat);
            EditorGUILayout.Space();
            
            
            if (isNeedOverridenValidationTemp != isNeedOverridenValidation) {
                isNeedOverridenValidation = isNeedOverridenValidationTemp;
                EditorPrefs.SetBool($"{_projectKey}_{nameof(isNeedOverridenValidation)}", isNeedOverridenValidationTemp);
            }
            
            if (maxTextureSizeTemp != _maxTextureSize) {
                _maxTextureSize = maxTextureSizeTemp;
                EditorPrefs.SetInt($"{_projectKey}_{nameof(maxTextureSize)}", (int)maxTextureSizeTemp);
            }
            
            if (solidFormatTemp != solidFormat) {
                solidFormat = solidFormatTemp;
                EditorPrefs.SetInt($"{_projectKey}_{nameof(solidFormat)}", (int)solidFormatTemp);
            }
            
            if (alphaFormatTemp != alphaFormat) {
                alphaFormat = alphaFormatTemp;
                EditorPrefs.SetInt($"{_projectKey}_{nameof(alphaFormat)}", (int)alphaFormatTemp);
            }
        }
    }
}