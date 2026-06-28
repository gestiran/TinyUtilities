// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    public abstract class ScrollButtonMove : MonoBehaviour {
        public bool isActive { get; private set; }
        
        [field: SerializeField, Required]
        protected ScrollElementsList _scroll { get; private set; }
        
        [field: SerializeField, Required]
        public UnityEvent onActive { get; private set; }
        
        [field: SerializeField, Required]
        public UnityEvent onInactive { get; private set; }
        
        [field: SerializeField, FoldoutGroup(InspectorNames.GENERATED, Order = 100), ReadOnly, Required]
        protected Button _thisButton { get; private set; }
        
        private bool _isInitialized;
        
        private void Awake() => _thisButton.onClick.AddListener(OnClick);
        
        public void UpdateActiveState() {
            if (_isInitialized) {
                if (_scroll.elementsCount == 1) {
                    Hide();
                } else if (IsActive()) {
                    ToActive();
                } else {
                    ToInactive();
                }
            } else {
                if (_scroll.elementsCount == 1) {
                    Hide();
                } else if (IsActive()) {
                    ToActiveForce();
                } else {
                    ToInactiveForce();
                }
            }
        }
        
        protected abstract void OnClick();
        
        protected abstract bool IsActive();
        
        public void Hide() {
            _thisButton.interactable = false;
            gameObject.SetActive(false);
        }
        
        private void ToActive() {
            if (isActive) {
                return;
            }
            
            ToActiveForce();
        }
        
        private void ToActiveForce() {
            _thisButton.interactable = true;
            gameObject.SetActive(true);
            
            isActive = true;
            onActive.Invoke();
        }
        
        private void ToInactive() {
            if (isActive == false) {
                return;
            }
            
            ToInactiveForce();
        }
        
        private void ToInactiveForce() {
            if (isActive == false) {
                return;
            }
            
            _thisButton.interactable = false;
            gameObject.SetActive(true);
            
            isActive = false;
            onInactive.Invoke();
        }
        
        
    #if UNITY_EDITOR
        
        [ContextMenu(InspectorNames.SOFT_RESET)]
        protected virtual void Reset() {
            _thisButton = GetComponent<Button>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    #endif
    }
}