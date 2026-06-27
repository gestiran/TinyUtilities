// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if UNITY_ENGINE
using System.Collections.Generic;
using TinyUtilities.Editor.EditorInputs;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TinyUtilities.Editor.GridSnapping {
    [InitializeOnLoad]
    public static class GridSnappingAdaptive {
        private static int _lastUndoGroup;
        
        private static readonly HashSet<GameObject> _processed;
        
        static GridSnappingAdaptive() {
            _lastUndoGroup = -1;
            _processed = new HashSet<GameObject>();
            ObjectChangeEvents.changesPublished += OnObjectChange;
        }
        
        private static void OnObjectChange(ref ObjectChangeEventStream stream) {
            if (GridSnappingProjectSettings.isEnable) {
                bool isMoveOrCreate = false;
                
                for (int eventId = 0; eventId < stream.length; eventId++) {
                    ObjectChangeKind type = stream.GetEventType(eventId);
                    
                    if (type is ObjectChangeKind.ChangeGameObjectOrComponentProperties) {
                        stream.GetChangeGameObjectOrComponentPropertiesEvent(eventId, out ChangeGameObjectOrComponentPropertiesEventArgs data);
                        
                    #if UNITY_6000_3
                        if (EditorUtility.EntityIdToObject(data.instanceId) is Transform)
                    #else
                        if (EditorUtility.InstanceIDToObject(data.instanceId) is Transform)
                    #endif
                        {
                            isMoveOrCreate = true;
                            break;
                        }
                    }
                    
                    if (type is ObjectChangeKind.CreateGameObjectHierarchy) {
                        isMoveOrCreate = true;
                        break;
                    }
                }
                
                if (isMoveOrCreate && IsActiveSnap()) {
                    int currentUndoGroup = Undo.GetCurrentGroup();
                    
                    if (currentUndoGroup == _lastUndoGroup) {
                        return;
                    }
                    
                    _lastUndoGroup = currentUndoGroup;
                    
                    SnapToGrid();
                }
            }
        }
        
        private static bool IsActiveSnap() {
            if (EditorInput.isActive) {
                if (EditorSnapSettings.incrementalSnapActive && EditorInput.control == false) {
                    return true;
                }
                
                if (EditorSnapSettings.incrementalSnapActive == false && EditorInput.control) {
                    return true;
                }
                
                return false;
            }
            
            return EditorSnapSettings.incrementalSnapActive;
        }
        
        private static void SnapToGrid() {
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
#endif