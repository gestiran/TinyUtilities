// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class MeshFilterExtension {
        public static T UploadMeshData<T>(T meshFilters, bool markNoLongerReadable = true) where T : IEnumerable<MeshFilter> {
            foreach (MeshFilter meshFilter in meshFilters) {
                Mesh mesh = meshFilter.sharedMesh;
                
                if (mesh.isReadable == false) {
                    continue;
                }
                
                mesh.Optimize();
                mesh.UploadMeshData(markNoLongerReadable);
            }
            
            return meshFilters;
        }
    }
}