// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEditor;
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
    }
}