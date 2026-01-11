// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using TinyUtilities.Editor.Utilities;
using TinyUtilities.Extensions.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace TinyUtilities.Editor.AssetProcessors.CollidersImport {
    public sealed class ShadowImportPostProcessor : AssetPostprocessor {
        public const int ORDER = CollidersImportPostProcessor.ORDER + 10;
        
        private const string _SHADOW = "USO";
        
        private void OnPostprocessModel(GameObject root) {
            if (CollidersImportModule.isEnableShadow == false) {
                return;
            }
            
            List<Transform> children = root.transform.GetAllChildren();
            
            foreach (Transform child in children) {
                GenerateShadow(child, children);
            }
        }
        
        public override int GetPostprocessOrder() => ORDER;
        
        private void GenerateShadow(Transform target, List<Transform> all) {
            if (target.IsHavePrefix(_SHADOW) == false) {
                return;
            }
            
            if (target.TryGetComponent(out MeshRenderer meshRenderer)) {
                meshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            }
            
            Transform parent = target.parent;
            
            if (parent != null && parent.TryGetComponent(out meshRenderer)) {
                meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            } else {
                string targetName = target.StripName(_SHADOW);
                
                foreach (Transform obj in all) {
                    if (obj.StripName().Equals(targetName) && obj.TryGetComponent(out meshRenderer)) {
                        meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
                    }
                }
            }
        }
    }
}