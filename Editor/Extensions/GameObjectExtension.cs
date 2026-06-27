// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if UNITY_ENGINE
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace TinyUtilities.Editor.Extensions {
    public static class GameObjectExtension {
        [Pure]
        public static bool IsContain(this GameObject[] objects, string name) {
            for (int objectId = 0; objectId < objects.Length; objectId++) {
                if (objects[objectId].name != name) {
                    continue;
                }
                
                return true;
            }
            
            return false;
        }
        
        [Pure]
        public static bool IsContain(this GameObject[] objects, string name, out GameObject result) {
            for (int objectId = 0; objectId < objects.Length; objectId++) {
                if (objects[objectId].name != name) {
                    continue;
                }
                
                result = objects[objectId];
                return true;
            }
            
            result = default;
            return false;
        }
        
        [Pure]
        public static GameObject[] GetChildren(this GameObject obj) {
            GameObject[] children = new GameObject[obj.transform.childCount];
            
            for (int childId = 0; childId < children.Length; childId++) {
                children[childId] = obj.transform.GetChild(childId).gameObject;
            }
            
            return children;
        }
        
        [Pure]
        public static GameObject[] GetAllChildren(this GameObject[] objects) {
            List<GameObject> children = new List<GameObject>(objects.Length);
            
            for (int childId = 0; childId < objects.Length; childId++) {
                GameObject[] subChildren = GetAllChildren(objects[childId]);
                
                for (int subChildId = 0; subChildId < subChildren.Length; subChildId++) {
                    children.Add(subChildren[subChildId]);
                }
            }
            
            return children.ToArray();
        }
        
        [Pure]
        public static T[] GetComponent<T>(this GameObject[] objects) where T : MonoBehaviour {
            List<T> components = new List<T>(objects.Length);
            
            for (int objId = 0; objId < objects.Length; objId++) {
                T component = objects[objId].GetComponent<T>();
                
                if (component == null) {
                    continue;
                }
                
                components.Add(component);
            }
            
            return components.ToArray();
        }
        
        [Pure]
        public static GameObject[] GetAllChildren(this GameObject obj) {
            int childCount = obj.transform.childCount;
            List<GameObject> children = new List<GameObject>(childCount);
            
            children.Add(obj);
            
            for (int childId = 0; childId < childCount; childId++) {
                GameObject[] subChildren = GetAllChildrenNR(obj.transform.GetChild(childId).gameObject);
                
                for (int subChildId = 0; subChildId < subChildren.Length; subChildId++) {
                    children.Add(subChildren[subChildId]);
                }
            }
            
            return children.ToArray();
        }
        
        [Pure]
        public static T[] FilterComponent<T>(this GameObject[] objects) {
            List<T> result = new List<T>(objects.Length);
            
            for (int objId = 0; objId < objects.Length; objId++) {
                T component = objects[objId].GetComponent<T>();
                
                if (component == null) {
                    continue;
                }
                
                result.Add(component);
            }
            
            return result.ToArray();
        }
        
        public static void SetParent<T>(this T[] objects, Transform parent) where T : MonoBehaviour {
            for (int objId = 0; objId < objects.Length; objId++) {
                objects[objId].transform.SetParent(parent);
            }
        }
        
        private static GameObject[] GetAllChildrenNR(this GameObject obj) => GetAllChildren(obj);
    }
}
#endif