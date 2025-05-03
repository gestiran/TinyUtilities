using Sirenix.OdinInspector.Editor.Validation;
using TinyUtilities.Editor.TexturesCompressor;
using UnityEditor;
using UnityEngine;

[assembly: RegisterValidationRule(typeof(TexturesCompressorRule), "Textures Compression", "Checks texture compression", false)]

namespace TinyUtilities.Editor.TexturesCompressor {
    public sealed class TexturesCompressorRule : AssetImporterValidator<Texture2D, TextureImporter> {
        private readonly string _platform;
        
        public TexturesCompressorRule() {
            _platform = EditorUserBuildSettings.activeBuildTarget.ToString();
        }
        
        protected override void Validate(ValidationResult result) {
            if (TryLoadImporter(out TextureImporter importer) == false) {
                return;
            }
            
            bool isNeedOverridenValidation = TexturesCompressorSettings.isNeedOverridenValidation;
            int maxTextureSize = TexturesCompressorSettings.maxTextureSize;
            TextureImporterFormat solidFormat = TexturesCompressorSettings.solidFormat;
            TextureImporterFormat alphaFormat = TexturesCompressorSettings.alphaFormat;
            
            TextureImporterPlatformSettings settings = importer.GetPlatformTextureSettings(_platform);
            importer.GetSourceTextureWidthAndHeight(out int width, out int height);
            bool isHaveAlpha = importer.DoesSourceTextureHaveAlpha();
            bool isCanCompressed = width % 4 == 0 || height % 4 == 0;
            
            if (importer.mipmapEnabled) {
                result.AddError("MipMap enabled!").WithFix("Disable Mip Maps", importer.DisableMipMapAndReimport);
            }
            
            if (isHaveAlpha) {
                if (importer.alphaSource == TextureImporterAlphaSource.None) {
                    result.AddWarning("Alpha disabled, but it's contained in image!");
                }
                
                if (isCanCompressed && (settings.overridden == false || isNeedOverridenValidation)) {
                    if (settings.format != alphaFormat) {
                        result.AddError("Invalid compression format!").WithFix($"Apply {alphaFormat} format", () => FixFormatType(importer, settings, alphaFormat));
                    }
                }
            } else {
                if (importer.alphaSource != TextureImporterAlphaSource.None) {
                    result.AddWarning("Alpha enabled, but it's missing in image!").WithFix("Disable Alpha", importer.DisableAlphaAndReimport);
                }
                
                if (isCanCompressed && (settings.overridden == false || isNeedOverridenValidation)) {
                    if (settings.format != solidFormat) {
                        result.AddError("Invalid compression format!").WithFix($"Apply {solidFormat} format", () => FixFormatType(importer, settings, solidFormat));
                    }
                }
            }
            
            if (isCanCompressed == false) {
                result.AddError("Invalid texture size, can't be compressed!");
            }
            
            
            if (settings.maxTextureSize > maxTextureSize && (settings.overridden == false || isNeedOverridenValidation)) {
                result.AddError("Max size limit!").WithFix($"Apply {maxTextureSize} size", () => FixMaxSize(importer, settings, maxTextureSize));
            }
        }
        
        private void FixFormatType(TextureImporter importer, TextureImporterPlatformSettings settings, TextureImporterFormat format) {
            settings.overridden = true;
            settings.format = format;
            importer.SetPlatformTextureSettings(settings);
            importer.Reimport();
        }
        
        private void FixMaxSize(TextureImporter importer, TextureImporterPlatformSettings settings, int size) {
            settings.overridden = true;
            settings.maxTextureSize = size;
            importer.SetPlatformTextureSettings(settings);
            importer.Reimport();
        }
    }
}