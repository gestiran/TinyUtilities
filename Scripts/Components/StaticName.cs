// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.Components {
    public sealed class StaticName : MonoBehaviour, ISelfValidator {
        [SerializeField, HideInInspector]
        private string _name;
        
        public void Validate(SelfValidationResult result) {
        #if UNITY_EDITOR
            
            if (gameObject.name != _name) {
                result.AddError($"Invalid name, required: {_name}").WithFix(FixName);
            }
            
        #endif
        }
        
    #if UNITY_EDITOR
        
        private void FixName() {
            gameObject.name = _name;
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
        
        [ContextMenu(InspectorNames.SOFT_RESET)]
        private void Reset() {
            _name = gameObject.name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    #endif
    }
}