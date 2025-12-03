// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace TinyUtilities.Editor.AssetProcessors.CollidersImport {
    public sealed class CollidersImportPostProcessor : AssetPostprocessor {
        private const string _BOX_COLLIDER = "UBX";
        private const string _CAPSULE_COLLIDER = "UCP";
        private const string _SPHERE_COLLIDER = "USX";
        private const string _MESH_CONVEX_COLLIDER = "UCX";
        private const string _MESH_COLLIDER = "UMC";
        private const string _SHADOW = "USO";
        
        private void OnPostprocessModel(GameObject meshAsset) {
            if (CollidersImportModule.isEnable == false) {
                return;
            }
            
            List<Transform> destroyList = new List<Transform>();
            
            foreach (Transform child in meshAsset.transform) {
                Generate(child, destroyList, meshAsset);
            }
            
            for (int i = destroyList.Count - 1; i >= 0; i--) {
                if (destroyList[i] != null) {
                    Object.DestroyImmediate(destroyList[i].gameObject);
                }
            }
        }
        
        private void TransformSharedMesh(MeshFilter meshFilter) {
            if (meshFilter == null) {
                return;
            }
            
            Transform transform = meshFilter.transform;
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            
            for (int verticeId = 0; verticeId < vertices.Length; verticeId++) {
                vertices[verticeId] = transform.TransformPoint(vertices[verticeId]);
                vertices[verticeId] = transform.parent.InverseTransformPoint(vertices[verticeId]);
            }
            
            mesh.SetVertices(vertices);
        }
        
        private void GenerateNR(Transform transform, List<Transform> destroyList, GameObject meshAsset) {
            Generate(transform, destroyList, meshAsset);
        }
        
        private void Generate(Transform transform, List<Transform> destroyList, GameObject meshAsset) {
            foreach (Transform child in transform.transform) {
                GenerateNR(child, destroyList, meshAsset);
            }
            
            if (IsHavePrefix(transform, _BOX_COLLIDER)) {
                AddBoxCollider(transform);
                destroyList.Add(transform);
            } else if (IsHavePrefix(transform, _CAPSULE_COLLIDER)) {
                AddCapsuleCollider(transform);
                destroyList.Add(transform);
            } else if (IsHavePrefix(transform, _SPHERE_COLLIDER)) {
                AddCollider<SphereCollider>(transform, meshAsset);
                destroyList.Add(transform);
            } else if (IsHavePrefix(transform, _MESH_CONVEX_COLLIDER)) {
                TransformSharedMesh(transform.GetComponent<MeshFilter>());
                MeshCollider collider = AddCollider<MeshCollider>(transform, meshAsset);
                collider.convex = true;
                destroyList.Add(transform);
            } else if (IsHavePrefix(transform, _MESH_COLLIDER)) {
                TransformSharedMesh(transform.GetComponent<MeshFilter>());
                AddCollider<MeshCollider>(transform, meshAsset);
                destroyList.Add(transform);
            } else if (IsHavePrefix(transform, _SHADOW)) {
                ApplyShadow(transform);
            }
        }
        
        private void AddBoxCollider(Transform transform) {
            BoxCollider collider = transform.parent.gameObject.AddComponent<BoxCollider>();
            Bounds bounds = RotateBounds(transform);
            
            collider.center = bounds.center;
            collider.size = bounds.size;
        }
        
        private void AddCapsuleCollider(Transform transform) {
            CapsuleCollider collider = transform.parent.gameObject.AddComponent<CapsuleCollider>();
            Bounds bounds = RotateBounds(transform);
            
            float x = Mathf.Abs(bounds.size.x);
            float y = Mathf.Abs(bounds.size.y);
            float z = Mathf.Abs(bounds.size.z);
            
            float height = Mathf.Max(x, y, z);
            
            collider.center = bounds.center;
            collider.direction = height.Equals(x) ? 0 : height.Equals(y) ? 1 : 2;
            collider.height = height;
            collider.radius = Mathf.Min(x, y, z);
        }
        
        private T AddCollider<T>(Transform transform, GameObject meshAsset) where T : Collider {
            if (1 - Mathf.Abs(Quaternion.Dot(meshAsset.transform.rotation, transform.rotation)) > 0.1f) {
                Debug.LogWarning("Collision mesh transform doesn't match the parent transform rotation, Colliders may not have translated correctly.");
            }
            
            T collider = transform.gameObject.AddComponent<T>();
            T parentCollider = transform.parent.gameObject.AddComponent<T>();
            
            EditorUtility.CopySerialized(collider, parentCollider);
            
            SerializedObject parentColliderSO = new SerializedObject(parentCollider);
            SerializedProperty parentCenterProperty = parentColliderSO.FindProperty("m_Center");
            
            if (parentCenterProperty != null) {
                SerializedObject colliderSO = new SerializedObject(collider);
                SerializedProperty colliderCenterProperty = colliderSO.FindProperty("m_Center");
                Vector3 worldSpaceColliderCenter = transform.TransformPoint(colliderCenterProperty.vector3Value);
                
                parentCenterProperty.vector3Value = transform.parent.InverseTransformPoint(worldSpaceColliderCenter);
                parentColliderSO.ApplyModifiedPropertiesWithoutUndo();
            }
            
            return parentCollider;
        }
        
        private Bounds RotateBounds(Transform transform) {
            Bounds bounds = transform.GetComponent<MeshFilter>().sharedMesh.bounds;
            Vector3 center = (transform.rotation * (bounds.center - transform.position)) + transform.position;
            Vector3 extents = (transform.rotation * (bounds.extents - bounds.center)) + center;
            
            bounds.extents = extents;
            bounds.center = center;
            return bounds;
        }
        
        private void ApplyShadow(Transform transform) {
            Transform parentTransform = transform.parent;
            
            if (parentTransform == null) {
                return;
            }
            
            MeshRenderer parentRenderer = parentTransform.gameObject.GetComponent<MeshRenderer>();
            
            if (parentRenderer == null) {
                return;
            }
            
            parentRenderer.shadowCastingMode = ShadowCastingMode.Off;
            
            MeshRenderer renderer = transform.gameObject.GetComponent<MeshRenderer>();
            renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            renderer.enabled = true;
        }
        
        private bool IsHavePrefix(Transform transform, string prefix) {
            if (transform.gameObject.TryGetComponent(out MeshFilter meshFilter)) {
                return meshFilter.sharedMesh.name.StartsWith($"{prefix}_");
            }
            
            return transform.name.StartsWith($"{prefix}_");
        }
    }
}