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
        
        private void OnPostprocessModel(GameObject root) {
            if (CollidersImportModule.isEnable == false) {
                return;
            }
            
            List<Transform> children = new List<Transform>();
            
            foreach (Transform child in root.transform) {
                children.Add(child);
            }
            
            if (CollidersImportModule.overrideLayer) {
                root.layer = CollidersImportModule.layer;
                
                foreach (Transform child in children) {
                    child.gameObject.layer = CollidersImportModule.layer;
                }
            }
            
            bool isCustomShadow = false;
            
            for (int i = 0; i < children.Count; i++) {
                if (TryGenerateShadow(children[i])) {
                    isCustomShadow = true;
                    children.RemoveAt(i);
                    break;
                }
            }
            
            List<Transform> destroyList = new List<Transform>();
            
            foreach (Transform child in children) {
                Generate(child, destroyList, root, isCustomShadow);
            }
            
            foreach (Transform obj in destroyList) {
                if (obj != null) {
                    Object.DestroyImmediate(obj.gameObject);
                }
            }
        }
        
        private bool TryGenerateShadow(Transform target) {
            if (IsHavePrefix(target, _SHADOW)) {
                MeshRenderer renderer = target.GetComponent<MeshRenderer>();
                
                if (renderer == null) {
                    return false;
                }
                
                renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                renderer.enabled = true;
                
                return true;
            }
            
            return false;
        }
        
        private void GenerateNR(Transform target, List<Transform> destroyList, GameObject root, bool isCustomShadow) {
            Generate(target, destroyList, root, isCustomShadow);
        }
        
        private void Generate(Transform target, List<Transform> destroyList, GameObject root, bool isCustomShadow) {
            foreach (Transform child in target.transform) {
                GenerateNR(child, destroyList, root, isCustomShadow);
            }
            
            if (IsHavePrefix(target, _BOX_COLLIDER)) {
                AddBoxCollider(target);
                destroyList.Add(target);
            } else if (IsHavePrefix(target, _CAPSULE_COLLIDER)) {
                AddCapsuleCollider(target);
                destroyList.Add(target);
            } else if (IsHavePrefix(target, _SPHERE_COLLIDER)) {
                AddCollider<SphereCollider>(target, root);
                destroyList.Add(target);
            } else if (IsHavePrefix(target, _MESH_CONVEX_COLLIDER)) {
                TransformSharedMesh(target.GetComponent<MeshFilter>());
                MeshCollider collider = AddCollider<MeshCollider>(target, root);
                collider.convex = true;
                destroyList.Add(target);
            } else if (IsHavePrefix(target, _MESH_COLLIDER)) {
                TransformSharedMesh(target.GetComponent<MeshFilter>());
                AddCollider<MeshCollider>(target, root);
                destroyList.Add(target);
            } else if (IsHavePrefix(target, _SHADOW)) {
                // Ignore
            } else if (isCustomShadow) {
                MeshRenderer renderer = target.GetComponent<MeshRenderer>();
                
                if (renderer != null) {
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                }
            }
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
        
        private T AddCollider<T>(Transform target, GameObject root) where T : Collider {
            if (1 - Mathf.Abs(Quaternion.Dot(root.transform.rotation, target.rotation)) > 0.1f) {
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
        
        private bool IsHavePrefix(Transform target, string prefix) {
            if (target.TryGetComponent(out MeshFilter meshFilter)) {
                return meshFilter.sharedMesh.name.StartsWith($"{prefix}_");
            }
            
            return target.name.StartsWith($"{prefix}_");
        }
    }
}