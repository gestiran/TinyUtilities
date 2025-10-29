// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using UnityEngine;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public sealed class ButtonToggle : MonoBehaviour {
        public event Action<bool> onClick;
        
        [SerializeField, BoxGroup, Required]
        private GameObject _activeState;
        
        [SerializeField, BoxGroup, Required]
        private GameObject _inactiveState;
        
        [SerializeField, FoldoutGroup(InspectorNames.GENERATED, 1000), Required, ReadOnly]
        private Button _thisButton;
        
        private bool _isActive;
        
        public void Awake() => _thisButton.onClick.AddListener(OnClick);
        
        public void Toggle(bool isActive) {
            _activeState.SetActive(isActive);
            _inactiveState.SetActive(!isActive);
            
            onClick?.Invoke(isActive);
            _isActive = isActive;
        }
        
        private void OnClick() => Toggle(!_isActive);
        
    #if UNITY_EDITOR
        
        [Button, DisableInPlayMode]
        private void Toggle() => Toggle(!_isActive);
        
        [ContextMenu(InspectorNames.SOFT_RESET)]
        public void Reset() {
            _thisButton = GetComponent<Button>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    #endif
    }
}