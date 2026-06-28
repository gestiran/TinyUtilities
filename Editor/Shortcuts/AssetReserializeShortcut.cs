// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.Shortcuts {
    public static class AssetReserializeShortcut {
        [MenuItem("Assets/Reserialize Force", false, 18)]
        public static void Reserialize() {
            Object[] objects = Selection.objects;
            List<string> paths = new List<string>();
            
            foreach (Object obj in objects) {
                paths.Add(AssetDatabase.GetAssetPath(obj));
            }
            
            AssetDatabase.ForceReserializeAssets(paths, ForceReserializeAssetsOptions.ReserializeAssetsAndMetadata);
        }
    }
}