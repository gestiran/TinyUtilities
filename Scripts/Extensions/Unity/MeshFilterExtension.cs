// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class MeshFilterExtension {
        public static void UploadMeshData(this MeshFilter[] meshFilters) {
            for (int meshId = 0; meshId < meshFilters.Length; meshId++) {
                Mesh mesh = meshFilters[meshId].sharedMesh;
                
                if (mesh.isReadable == false) {
                    continue;
                }
                
                mesh.Optimize();
                mesh.UploadMeshData(true);
            }
        }
    }
}