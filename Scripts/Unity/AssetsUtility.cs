// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityObject = UnityEngine.Object;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.Unity {
    public static class AssetsUtility {
    #if UNITY_EDITOR
        public static T[] GetAssetsAtPath<T>(string path) {
            ArrayList al = new ArrayList();
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
            
            foreach (string fileName in fileEntries) {
                int index = fileName.LastIndexOf("/", StringComparison.Ordinal);
                string localPath = "Assets/" + path;
                
                if (index > 0)
                    localPath += fileName.Substring(index);
                
                UnityObject t = UnityEditor.AssetDatabase.LoadAssetAtPath(localPath, typeof(T));
                
                if (t != null)
                    al.Add(t);
            }
            
            T[] result = new T[al.Count];
            
            for (int i = 0; i < al.Count; i++)
                result[i] = (T)al[i];
            
            return result;
        }
    #endif
        
        public static ValueDropdownList<GameObject> GetPrefabs(string path, bool splitName = false) {
            ValueDropdownList<GameObject> result = new ValueDropdownList<GameObject>();
            
        #if UNITY_EDITOR
            string[] assets = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new[] { path });
            
            for (int assetId = 0; assetId < assets.Length; assetId++) {
                GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GUIDToAssetPath(assets[assetId]));
                string name = prefab.name;
                
                if (splitName) {
                    string[] parts = name.Split('_');
                    
                    if (parts.Length > 0) {
                        name = parts[0];
                    }
                }
                
                result.Add(name, prefab);
            }
        #endif
            
            return result;
        }
    }
}