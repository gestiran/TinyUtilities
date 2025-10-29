// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using UnityEditor;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
#endif

namespace TinyUtilities.Editor.MergeObjects {
    public sealed class MergeObjectsWindow : OdinEditorWindow {
        [ShowInInspector]
        private Type[] _components;
        
        [ShowInInspector, HorizontalGroup("Objects"), HideLabel]
        private GameObject _source;
        
        [ShowInInspector, HorizontalGroup("Objects"), HideLabel]
        private GameObject _destination;
        
        [MenuItem("Window/TinyUtilities/Merge Objects", priority = 0)]
        private static void OpenWindow() => GetWindow<MergeObjectsWindow>("Merge Objects").Show();
        
        [OnInspectorInit]
        private void InspectorInit() {
            _components = Type.EmptyTypes;
            Selection.selectionChanged += UpdateObjects;
        }
        
        [OnInspectorDispose]
        private void InspectorDispose() {
            _components = null;
            Selection.selectionChanged -= UpdateObjects;
        }
        
        private void UpdateObjects() {
            GameObject active = Selection.activeGameObject;
            
            if (active == null) {
                ResetSelected();
                return;
            }
            
            if (_source == null) {
                _source = active;
            } else if (_destination == null && active.GetInstanceID() != _source.GetInstanceID()) {
                _destination = active;
            }
        }
        
        [Button, HorizontalGroup("Buttons"), EnableIf("IsEnableMerge")]
        public void Merge() {
            foreach (Type type in _components) {
                Copy(_source, _destination, type);
            }
            
            EditorUtility.SetDirty(_destination);
            ResetSelected();
        }
        
        [Button, HorizontalGroup("Buttons"), EnableIf("IsEnableMerge")]
        public void MergeAndRemove() {
            foreach (Type type in _components) {
                Copy(_source, _destination, type);
            }
            
            EditorUtility.SetDirty(_destination);
            DestroyImmediate(_source);
            ResetSelected();
        }
        
        private void ResetSelected() {
            _source = null;
            _destination = null;
        }
        
        private void Copy(GameObject from, GameObject to, Type componentType) {
            Component sourceValue = from.GetComponent(componentType);
            
            if (sourceValue == null) {
                return;
            }
            
            Component destinationValue = to.GetComponent(componentType);
            
            if (destinationValue == null) {
                destinationValue = to.AddComponent(componentType);
            }
            
            EditorUtility.CopySerialized(sourceValue, destinationValue);
        }
        
        private bool IsEnableMerge() {
            if (_components == null || _components.Length == 0) {
                return false;
            }
            
            if (_source == null || _destination == null) {
                return false;
            }
            
            return true;
        }
    }
}