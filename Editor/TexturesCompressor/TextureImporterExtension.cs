using UnityEditor;

namespace TinyUtilities.Editor.TexturesCompressor {
    public static class TextureImporterExtension {
        public static void DisableMipMapAndReimport(this TextureImporter importer) {
            importer.mipmapEnabled = false;
            importer.Reimport();
        }
        
        public static void DisableAlphaAndReimport(this TextureImporter importer) {
            importer.alphaSource = TextureImporterAlphaSource.None;
            importer.Reimport();
        }
        
        public static void Reimport(this TextureImporter importer) {
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }
    }
}