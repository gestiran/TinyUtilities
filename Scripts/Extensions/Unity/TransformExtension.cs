// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace TinyUtilities.Extensions.Unity {
    public static class TransformExtension {
        public static bool TryFindChildWithName(this Transform transform, string name, out Transform result) {
            return TryFindChildWithName(transform.GetChildTransforms(), name, out result);
        }
        
        public static void SetParent(this Transform[] transforms, Transform parent) {
            for (int transformId = 0; transformId < transforms.Length; transformId++) {
                transforms[transformId].SetParent(parent);
            }
        }
        
        public static void DestroyChildren(this Transform transform) {
            int childCount = transform.childCount;
            
            for (int childId = childCount - 1; childId >= 0; childId--) {
                UnityObject.Destroy(transform.GetChild(childId).gameObject);
            }
        }
        
        public static void DestroyChildren(this Transform transform, float delay) {
            int childCount = transform.childCount;
            
            for (int childId = childCount - 1; childId >= 0; childId--) {
                UnityObject.Destroy(transform.GetChild(childId).gameObject, delay);
            }
        }
        
        public static Transform[] GetChildTransforms(this Transform transform) {
            int childCount = transform.childCount;
            
            if (childCount <= 0) {
                return Array.Empty<Transform>();
            }
            
            Transform[] result = new Transform[childCount];
            
            for (int childId = 0; childId < result.Length; childId++) {
                result[childId] = transform.GetChild(childId);
            }
            
            return result;
        }
        
        public static bool IsDefault(this Transform transform) {
            if (!transform.position.Equals(Vector3.zero)) {
                return false;
            }
            
            if (!transform.rotation.Equals(Quaternion.identity)) {
                return false;
            }
            
            if (!transform.localScale.Equals(Vector3.one)) {
                return false;
            }
            
            return true;
        }
        
        public static void Reset(this Transform transform) {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            
        #if UNITY_EDITOR
            
            if (!UnityEditor.EditorApplication.isPlaying) {
                UnityEditor.EditorUtility.SetDirty(transform);
            }
            
        #endif
        }
        
        public static void Apply(this Transform transform, Vector3 position, Quaternion rotation, Vector3 localScale) {
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = localScale;
            
        #if UNITY_EDITOR
            
            if (!UnityEditor.EditorApplication.isPlaying) {
                UnityEditor.EditorUtility.SetDirty(transform);
            }
            
        #endif
        }
        
        public static GameObject[] ToGameObject(this Transform[] transforms) {
            GameObject[] gameObjects = new GameObject[transforms.Length];
            
            for (int objId = 0; objId < gameObjects.Length; objId++) {
                gameObjects[objId] = transforms[objId].gameObject;
            }
            
            return gameObjects;
        }
        
        public static void DestroyImmediateChildren(this Transform transform) {
            for (int childId = transform.childCount - 1; childId >= 0; childId--) {
                UnityObject.DestroyImmediate(transform.GetChild(childId).gameObject);
            }
            
        #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(transform);
        #endif
        }
        
        public static void DestroyImmediateChildren(this Transform transform, Func<GameObject, bool> filter) {
            for (int childId = transform.childCount - 1; childId >= 0; childId--) {
                GameObject child = transform.GetChild(childId).gameObject;
                
                if (filter(child)) {
                    continue;
                }
                
                UnityObject.DestroyImmediate(child);
            }
            
        #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(transform);
        #endif
        }
        
        public static Vector3[] GetChildrenPositions(this Transform transform) {
            Vector3[] positions = new Vector3[transform.childCount];
            
            for (int positionId = 0; positionId < positions.Length; positionId++) {
                positions[positionId] = transform.GetChild(positionId).position;
            }
            
            return positions;
        }
        
        public static Transform AddChild(this Transform transform, string name) {
            GameObject child = new GameObject(name);
            Transform childTransform = child.transform;
            childTransform.SetParent(transform);
            return childTransform;
        }
        
        public static void ScaleGlobal(this Transform transform, Vector3 scale) {
            Transform parent = transform.parent;
            
            while (parent) {
                scale = scale.DivideValues(parent.localScale);
                parent = parent.parent;
            }
            
            transform.localScale = scale;
        }
        
        private static bool TryFindChildWithName(Transform[] children, string name, out Transform result) {
            for (int childId = 0; childId < children.Length; childId++) {
                if (children[childId].name.Equals(name)) {
                    result = children[childId];
                    return true;
                }
                
                if (TryFindChildWithNameNR(children[childId].GetChildTransforms(), name, out result)) {
                    return true;
                }
            }
            
            result = null;
            return false;
        }
        
        [Pure]
        public static RectTransform RectTransform(this Transform transform) => transform as RectTransform;
        
        [Pure]
        public static Vector2 SizeDelta(this Transform transform) => transform.RectTransform().sizeDelta;
        
        private static bool TryFindChildWithNameNR(Transform[] children, string name, out Transform result) {
            return TryFindChildWithName(children, name, out result);
        }
    }
}