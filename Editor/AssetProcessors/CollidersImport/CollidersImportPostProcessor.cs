// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;
using TinyUtilities.Extensions.Unity;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.AssetProcessors.CollidersImport {
    public sealed class CollidersImportPostProcessor : AssetPostprocessor {
        public const int ORDER = 10;
        
        private void OnPostprocessModel(GameObject root) {
            if (CollidersImportModule.isEnable == false) {
                return;
            }
            
            foreach (Transform child in root.transform.GetAllChildren()) {
                if (TryGenerate(child)) {
                    Object.DestroyImmediate(child.gameObject);
                }
            }
        }
        
        public override int GetPostprocessOrder() => ORDER;
        
        private bool TryGenerate(Transform target) {
            if (target.IsHavePrefix(ImportPrefixes.BOX_COLLIDER)) {
                AddBoxCollider(target);
            } else if (target.IsHavePrefix(ImportPrefixes.CAPSULE_COLLIDER)) {
                AddCapsuleCollider(target);
            } else if (target.IsHavePrefix(ImportPrefixes.SPHERE_COLLIDER)) {
                AddCollider<SphereCollider>(target);
            } else if (target.IsHavePrefix(ImportPrefixes.MESH_CONVEX_COLLIDER)) {
                TransformSharedMesh(target.GetComponent<MeshFilter>());
                MeshCollider collider = AddCollider<MeshCollider>(target);
                collider.convex = true;
            } else if (target.IsHavePrefix(ImportPrefixes.MESH_COLLIDER)) {
                TransformSharedMesh(target.GetComponent<MeshFilter>());
                AddCollider<MeshCollider>(target);
            } else {
                return false;
            }
            
            return true;
        }
        
        private void AddBoxCollider(Transform target) {
            BoxCollider collider = target.parent.gameObject.AddComponent<BoxCollider>();
            Bounds bounds = RotateBounds(target);
            
            collider.center = bounds.center;
            collider.size = bounds.size;
        }
        
        private void AddCapsuleCollider(Transform target) {
            CapsuleCollider collider = target.parent.gameObject.AddComponent<CapsuleCollider>();
            Bounds bounds = RotateBounds(target);
            
            float x = Mathf.Abs(bounds.size.x);
            float y = Mathf.Abs(bounds.size.y);
            float z = Mathf.Abs(bounds.size.z);
            
            float height = Mathf.Max(x, y, z);
            
            collider.center = bounds.center;
            collider.direction = height.Equals(x) ? 0 : height.Equals(y) ? 1 : 2;
            collider.height = height;
            collider.radius = Mathf.Min(x, y, z);
        }
        
        private T AddCollider<T>(Transform target) where T : Collider {
            if (1 - Mathf.Abs(Quaternion.Dot(target.parent.rotation, target.rotation)) > 0.1f) {
                Debug.LogWarning("Collision mesh transform doesn't match the parent transform rotation, Colliders may not have translated correctly.");
            }
            
            T collider = target.gameObject.AddComponent<T>();
            T parentCollider = target.parent.gameObject.AddComponent<T>();
            
            EditorUtility.CopySerialized(collider, parentCollider);
            
            SerializedObject parentColliderSO = new SerializedObject(parentCollider);
            SerializedProperty parentCenterProperty = parentColliderSO.FindProperty("m_Center");
            
            if (parentCenterProperty != null) {
                SerializedObject colliderSO = new SerializedObject(collider);
                SerializedProperty colliderCenterProperty = colliderSO.FindProperty("m_Center");
                Vector3 worldSpaceColliderCenter = target.TransformPoint(colliderCenterProperty.vector3Value);
                
                parentCenterProperty.vector3Value = target.parent.InverseTransformPoint(worldSpaceColliderCenter);
                parentColliderSO.ApplyModifiedPropertiesWithoutUndo();
            }
            
            return parentCollider;
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
        
        private Bounds RotateBounds(Transform target) {
            Bounds bounds = target.GetComponent<MeshFilter>().sharedMesh.bounds;
            Vector3 center = (target.rotation * (bounds.center - target.position)) + target.position;
            Vector3 extents = (target.rotation * (bounds.extents - bounds.center)) + center;
            
            bounds.extents = extents;
            bounds.center = center;
            return bounds;
        }
    }
}