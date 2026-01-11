// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace TinyUtilities.Editor.Utilities {
    public static class AssetsUtility {
        public static T GetAsset<T>(string path) where T : UnityObject {
            string[] assets = AssetDatabase.FindAssets("", new[] { path });

            for (int assetId = 0; assetId < assets.Length; assetId++) {
                T asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assets[assetId]));

                if (asset != null) {
                    return asset;
                }
            }

            return null;
        }
        
        public static bool IsHavePrefix(this Transform target, string prefix) {
            if (target.TryGetComponent(out MeshFilter meshFilter)) {
                return meshFilter.sharedMesh.name.StartsWith($"{prefix}_");
            }
            
            return target.name.StartsWith($"{prefix}_");
        }
        
        public static bool IsHavePrefix(this GameObject target, string prefix) {
            if (target.TryGetComponent(out MeshFilter meshFilter)) {
                return meshFilter.sharedMesh.name.StartsWith($"{prefix}_");
            }
            
            return target.name.StartsWith($"{prefix}_");
        }
        
        public static string StripName(this Transform target, params string[] prefixes) {
            string name;
            
            if (target.TryGetComponent(out MeshFilter meshFilter)) {
                name = meshFilter.sharedMesh.name;
            } else {
                name = target.name;
            }
            
            foreach (string postfix in LodPostfixes()) {
                name = name.Replace($"_{postfix}", "");
            }
            
            if (prefixes != null) {
                foreach (string prefix in prefixes) {
                    name = name.Replace($"{prefix}_","");
                }   
            }
            
            return name;
        }
        
        public static IEnumerable<string> LodPostfixes(int count = 5) {
            for (int lodId = 0; lodId < count; lodId++) {
                yield return $"LOD{lodId}";
            }
        }
    }
}