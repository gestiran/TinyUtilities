// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Extensions.Unity;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.AssetProcessors.CollidersImport {
    public sealed class LayerChangePostProcessor : AssetPostprocessor {
        public const int ORDER = ShadowImportPostProcessor.ORDER + 10;
        
        private void OnPostprocessModel(GameObject root) {
            if (CollidersImportModule.overrideLayer == false) {
                return;
            }
            
            root.layer = CollidersImportModule.layer;
            
            foreach (Transform child in root.transform.GetAllChildren()) {
                child.gameObject.layer = CollidersImportModule.layer;
            }
        }
        
        public override int GetPostprocessOrder() => ORDER;
    }
}