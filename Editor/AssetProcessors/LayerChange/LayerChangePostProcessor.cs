// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.AssetProcessors.ShadowsImport;
using TinyUtilities.Extensions.Unity;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.AssetProcessors.LayerChange {
    public sealed class LayerChangePostProcessor : AssetPostprocessor {
        public const int ORDER = ShadowImportPostProcessor.ORDER + 10;
        
        private void OnPostprocessModel(GameObject root) {
            if (LayerChangeModule.isEnabled == false) {
                return;
            }
            
            root.layer = LayerChangeModule.layer;
            
            foreach (Transform child in root.transform.GetAllChildren()) {
                child.gameObject.layer = LayerChangeModule.layer;
            }
        }
        
        public override int GetPostprocessOrder() => ORDER;
    }
}