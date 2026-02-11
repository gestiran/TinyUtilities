// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class MeshFilterExtension {
        public static T UploadMeshData<T>(this T meshFilters, bool markNoLongerReadable = true) where T : IEnumerable<MeshFilter> {
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
        
        [Pure]
        public static Vector3 CalculateMeshCenter<T>(this T meshFilters) where T : IEnumerable<MeshFilter> {
            return meshFilters.SharedMeshes().CalculateMeshCenter();
        }
        
        [Pure]
        public static IEnumerable<Mesh> SharedMeshes<T>(this T meshFilters) where T : IEnumerable<MeshFilter> {
            foreach (MeshFilter meshFilter in meshFilters) {
                yield return meshFilter.sharedMesh;
            }
        }
        
        [Pure]
        public static IEnumerable<Mesh> Meshes<T>(this T meshFilters) where T : IEnumerable<MeshFilter> {
            foreach (MeshFilter meshFilter in meshFilters) {
                yield return meshFilter.mesh;
            }
        }
        
        [Pure]
        public static Vector3 CalculateMeshCenter(this MeshFilter meshFilter) {
            return meshFilter.sharedMesh.CalculateMeshCenter();
        }
    }
}