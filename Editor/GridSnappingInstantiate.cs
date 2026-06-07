// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TinyUtilities.Editor {
    public static class GridSnappingInstantiate {
        private static int _lastUndoGroup = -1;
        
        private static readonly HashSet<GameObject> _processed = new HashSet<GameObject>();
        
        [InitializeOnLoadMethod]
        private static void Initialize() {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }
        
        private static void OnHierarchyChanged() {
            int currentUndoGroup = Undo.GetCurrentGroup();
            
            if (currentUndoGroup == _lastUndoGroup) {
                return;
            }
            
            _lastUndoGroup = currentUndoGroup;
            _processed.Clear();
            
            Vector3 gridSize = EditorSnapSettings.gridSize;
            GameObject[] gameObjects = Selection.gameObjects;
            
            for (int objId = 0; objId < gameObjects.Length; objId++) {
                GameObject gameObject = gameObjects[objId];
                
                if (gameObject == null) {
                    continue;
                }
                
                if (_processed.Contains(gameObject)) {
                    continue;
                }
                
                SnapToGrid(gameObject, gridSize);
                _processed.Add(gameObject);
            }
        }
        
        private static void SnapToGrid(in GameObject gameObject, in Vector3 gridSize) {
            gameObject.transform.position = SnapPosition(gameObject.transform.position, gridSize);
            EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
        
        private static Vector3 SnapPosition(in Vector3 position, in Vector3 gridSize) {
            return new Vector3(RoundValue(position.x, gridSize.x), RoundValue(position.y, gridSize.y), RoundValue(position.z, gridSize.z));
        }
        
        private static float RoundValue(in float value, in float gridSize) {
            if (gridSize <= 0) {
                return value;
            }
            
            return Mathf.Round(value / gridSize) * gridSize;
        }
    }
}