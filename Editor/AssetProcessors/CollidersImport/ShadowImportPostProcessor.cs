// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;
using TinyUtilities.Extensions.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace TinyUtilities.Editor.AssetProcessors.CollidersImport {
    public sealed class ShadowImportPostProcessor : AssetPostprocessor {
        public const int ORDER = CollidersImportPostProcessor.ORDER + 10;
        
        private void OnPostprocessModel(GameObject root) {
            if (CollidersImportModule.isEnableShadow == false) {
                return;
            }
            
            foreach (Transform child in root.transform.GetAllChildren()) {
                GenerateShadow(child);
            }
        }
        
        public override int GetPostprocessOrder() => ORDER;
        
        private void GenerateShadow(Transform target) {
            if (target.IsHavePrefix(ImportPrefixes.SHADOW_OBJECT)) {
                GenerateShadowObject(target);
            }
        }
        
        private static void GenerateShadowObject(Transform target) {
            if (target.TryGetComponent(out MeshRenderer meshRenderer)) {
                meshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            }
            
            Transform parent = target.parent;
            
            if (parent == null) {
                return;
            }
            
            if (parent.TryGetComponent(out meshRenderer)) {
                meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            } else {
                string targetName = target.StripName(ImportPrefixes.SHADOW_OBJECT);
                
                foreach (Transform obj in parent) {
                    if (obj.StripName(CollidersImportModule.stripPrefixes).Equals(targetName) && obj.TryGetComponent(out meshRenderer)) {
                        meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                    }
                }
            }
        }
    }
}