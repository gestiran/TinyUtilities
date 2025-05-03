using Sirenix.OdinInspector.Editor.Validation;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.TexturesCompressor {
    public abstract class AssetImporterValidator<TValue, TImporter> : RootObjectValidator<TValue> where TValue : Object where TImporter : AssetImporter {
        public override RevalidationCriteria RevalidationCriteria => RevalidationCriteria.OnValueChange;
        
        protected bool TryLoadImporter(out TImporter importer) {
            string pathToAsset = AssetDatabase.GetAssetPath(Value);
            
            if (string.IsNullOrEmpty(pathToAsset)) {
                importer = null;
                return false;
            }
            
            AssetImporter assetImporter = AssetImporter.GetAtPath(pathToAsset);
            
            if (assetImporter is TImporter target) {
                importer = target;
                return true;
            }
            
            importer = null;
            return false;
        }
    }
}