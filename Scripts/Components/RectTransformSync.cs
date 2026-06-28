// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TinyUtilities.Components {
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public sealed class RectTransformSync : UIBehaviour {
        [SerializeField]
        private RectTransform _from;
        
        [SerializeField]
        private RectTransform _to;
        
        protected override void OnCanvasHierarchyChanged() {
            base.OnCanvasHierarchyChanged();
            
            if (gameObject.activeInHierarchy == false) {
                return;
            }
            
            if (_to == null || _from == null) {
                return;
            }
            
            StartCoroutine(SyncProcess(_from, _to));
        }
        
        private IEnumerator SyncProcess(RectTransform from, RectTransform to) {
            yield return new WaitForEndOfFrame();
            
            Sync(from, to);
        }
        
        private void Sync(RectTransform from, RectTransform to) {
            to.anchoredPosition = from.anchoredPosition;
            to.anchorMax = from.anchorMax;
            to.anchorMin = from.anchorMin;
            to.offsetMax = from.offsetMax;
            to.offsetMin = from.offsetMin;
            to.sizeDelta = from.sizeDelta;
        }
        
    #if UNITY_EDITOR
        
        protected override void Start() {
            base.Start();
            
            if (_from != null && _to != null) {
                Sync(_to, _from);
            }
        }
        
        private void Update() {
            if (Application.isPlaying) {
                return;
            }
            
            if (_from != null && _to != null) {
                Sync(_to, _from);
            }
        }
        
        [ContextMenu(InspectorNames.SOFT_RESET)]
        protected override void Reset() {
            base.Reset();
            
            _to = GetComponent<RectTransform>();
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    #endif
    }
}