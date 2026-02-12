// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TinyUtilities.CustomTypes;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class MeshExtension {
        [Pure]
        public static MeshCorners CalculateMeshCorners<T>(this T meshes) where T : IEnumerable<Mesh> {
            MeshCorners corners = new MeshCorners();
            List<Vector3> vertices = new List<Vector3>();
            
            foreach (Mesh mesh in meshes) {
                corners = mesh.CalculateMeshCorners(vertices, corners);
            }
            
            return corners;
        }
        
        [Pure]
        public static MeshCorners CalculateMeshCorners(this Mesh mesh) {
            List<Vector3> vertices = new List<Vector3>();
            MeshCorners corners = new MeshCorners();
            return mesh.CalculateMeshCorners(vertices, corners);
        }
        
        [Pure]
        public static MeshCorners CalculateMeshCorners(this Mesh mesh, List<Vector3> vertices, MeshCorners compare) {
            if (mesh != null) {
                mesh.GetVertices(vertices);
                
                foreach (Vector3 vertice in vertices) {
                    if (vertice.x > compare.xMax) {
                        compare.xMax = vertice.x;
                    } else if (vertice.x < compare.xMin) {
                        compare.xMin = vertice.x;
                    }
                    
                    if (vertice.y > compare.yMax) {
                        compare.yMax = vertice.y;
                    } else if (vertice.y < compare.yMin) {
                        compare.yMin = vertice.y;
                    }
                    
                    if (vertice.z > compare.zMax) {
                        compare.zMax = vertice.z;
                    } else if (vertice.z < compare.zMin) {
                        compare.zMin = vertice.z;
                    }
                }
            }
            
            return compare;
        }
        
        [Pure]
        public static Vector3 CalculateMeshCenter<T>(this T meshes) where T : IEnumerable<Mesh> => meshes.CalculateMeshCorners().Center();
        
        [Pure]
        public static Vector3 CalculateMeshCenter(this Mesh mesh) {
            if (mesh != null) {
                return mesh.CalculateMeshCorners().Center();
            }
            
            return Vector3.zero;
        }
    }
}