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
        private static Vector3 CalculateMeshCenter<T>(this T meshFilters) where T : IEnumerable<MeshFilter> {
            Vector3 max = Vector3.zero;
            Vector3 min = Vector3.zero;
            
            List<Vector3> vertices = new List<Vector3>();
            
            foreach (MeshFilter meshFilter in meshFilters) {
                Mesh sharedMesh = meshFilter.sharedMesh;
                
                if (sharedMesh != null) {
                    sharedMesh.GetVertices(vertices);
                    
                    foreach (Vector3 vertice in vertices) {
                        if (vertice.x > max.x) {
                            max.x = vertice.x;
                        } else if (vertice.x < min.x) {
                            min.x = vertice.x;
                        }
                        
                        if (vertice.y > max.y) {
                            max.y = vertice.y;
                        } else if (vertice.y < min.y) {
                            min.y = vertice.y;
                        }
                        
                        if (vertice.z > max.z) {
                            max.z = vertice.z;
                        } else if (vertice.z < min.z) {
                            min.z = vertice.z;
                        }
                    }
                }
            }
            
            return new Vector3((min.x + max.x) * 0.5f, (min.y + max.y) * 0.5f, (min.z + max.z) * 0.5f);
        }
        
        [Pure]
        public static Vector3 CalculateMeshCenter(this MeshFilter meshFilter) {
            Vector3 max = Vector3.zero;
            Vector3 min = Vector3.zero;
            
            Mesh sharedMesh = meshFilter.sharedMesh;
            
            if (sharedMesh != null) {
                List<Vector3> vertices = new List<Vector3>();
                
                sharedMesh.GetVertices(vertices);
                
                foreach (Vector3 vertice in vertices) {
                    if (vertice.x > max.x) {
                        max.x = vertice.x;
                    } else if (vertice.x < min.x) {
                        min.x = vertice.x;
                    }
                    
                    if (vertice.y > max.y) {
                        max.y = vertice.y;
                    } else if (vertice.y < min.y) {
                        min.y = vertice.y;
                    }
                    
                    if (vertice.z > max.z) {
                        max.z = vertice.z;
                    } else if (vertice.z < min.z) {
                        min.z = vertice.z;
                    }
                }
            }
            
            return new Vector3((min.x + max.x) * 0.5f, (min.y + max.y) * 0.5f, (min.z + max.z) * 0.5f);
        }
    }
}