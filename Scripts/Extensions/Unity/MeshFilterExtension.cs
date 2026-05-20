// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TinyUtilities.CustomTypes;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class MeshFilterExtension {
        private readonly struct TriangleHit {
            public readonly float distance;
            public readonly float barycentricU;
            public readonly float barycentricV;
            
            public TriangleHit(float distance, float barycentricU, float barycentricV) {
                this.distance = distance;
                this.barycentricU = barycentricU;
                this.barycentricV = barycentricV;
            }
        }
        
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
        
        public static bool RaycastMesh(this MeshFilter meshFilter, Ray ray, out MeshHit hit) {
            Mesh mesh = meshFilter.sharedMesh;
            
            if (mesh == null) {
                hit = default;
                return false;
            }
            
            Matrix4x4 localToWorld = meshFilter.transform.localToWorldMatrix;
            Matrix4x4 worldToLocal = localToWorld.inverse;
            
            Ray localRay = new Ray(worldToLocal.MultiplyPoint(ray.origin), worldToLocal.MultiplyVector(ray.direction));
            
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            Vector2[] uvs = mesh.uv;
            
            float minDistance = float.MaxValue;
            bool isHit = false;
            Vector3 hitPosition = Vector3.zero;
            Vector2 hitUV = Vector2.zero;
            
            for (int triangleIndex = 0; triangleIndex < triangles.Length; triangleIndex += 3) {
                Vector3 vertexA = vertices[triangles[triangleIndex]];
                Vector3 vertexB = vertices[triangles[triangleIndex + 1]];
                Vector3 vertexC = vertices[triangles[triangleIndex + 2]];
                
                bool isHitTriangle = RaycastTriangle(localRay, vertexA, vertexB, vertexC, out TriangleHit triangleHit);
                
                if (isHitTriangle && triangleHit.distance < minDistance) {
                    minDistance = triangleHit.distance;
                    hitPosition = localToWorld.MultiplyPoint(localRay.origin + localRay.direction * triangleHit.distance);
                    
                    float barycentricW = 1f - triangleHit.barycentricU - triangleHit.barycentricV;
                    
                    Vector2 uvA = uvs[triangles[triangleIndex]];
                    Vector2 uvB = uvs[triangles[triangleIndex + 1]];
                    Vector2 uvC = uvs[triangles[triangleIndex + 2]];
                    
                    hitUV = uvA * barycentricW + uvB * triangleHit.barycentricU + uvC * triangleHit.barycentricV;
                    
                    isHit = true;
                }
            }
            
            if (isHit == false) {
                hit = default;
                return false;
            }
            
            Renderer renderer = meshFilter.GetComponent<Renderer>();
            Color color = Color.white;
            
            if (renderer != null && renderer.sharedMaterial != null && renderer.sharedMaterial.mainTexture is Texture2D texture) {
                if (texture.isReadable) {
                    color = texture.GetPixelBilinear(hitUV.x, hitUV.y);
                } else {
                    Debug.LogError($"Texture {texture.name} is not readable!", texture);
                    color = Color.white;
                }
            }
            
            hit = new MeshHit(hitPosition, color);
            return true;
        }
        
        private static bool RaycastTriangle(Ray ray, Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, out TriangleHit hit) {
            Vector3 edgeAb = vertexB - vertexA;
            Vector3 edgeAc = vertexC - vertexA;
            
            Vector3 rayCrossEdgeAc = Vector3.Cross(ray.direction, edgeAc);
            float determinant = Vector3.Dot(edgeAb, rayCrossEdgeAc);
            
            if (Mathf.Abs(determinant) < Mathf.Epsilon) {
                hit = default;
                return false;
            }
            
            float inverseDeterminant = 1f / determinant;
            Vector3 originToVertexA = ray.origin - vertexA;
            
            float barycentricU = Vector3.Dot(originToVertexA, rayCrossEdgeAc) * inverseDeterminant;
            
            if (barycentricU < 0f || barycentricU > 1f) {
                hit = default;
                return false;
            }
            
            Vector3 originToVertexACross = Vector3.Cross(originToVertexA, edgeAb);
            float barycentricV = Vector3.Dot(ray.direction, originToVertexACross) * inverseDeterminant;
            
            if (barycentricV < 0f || barycentricU + barycentricV > 1f) {
                hit = default;
                return false;
            }
            
            float hitDistance = Vector3.Dot(edgeAc, originToVertexACross) * inverseDeterminant;
            
            if (hitDistance < Mathf.Epsilon) {
                hit = default;
                return false;
            }
            
            hit = new TriangleHit(hitDistance, barycentricU, barycentricV);
            return true;
        }
    }
}