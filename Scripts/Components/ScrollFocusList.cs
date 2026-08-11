// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections;
using System.Collections.Generic;
using TinyUtilities.Extensions;
using TinyUtilities.Validation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.Components {
    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect), typeof(RectTransform))]
    public sealed class ScrollFocusList : UIBehaviour, ISelfValidator, ICoroutineRunner {
        public ScrollRect scroll => _thisScrollRect;
        public int elementsCount => _positions.Length;
        public Orientation orientation => _orientation;
        
        public int currentElement { get; private set; }
        
        [SerializeField, OnValueChanged("ApplyOrientation")]
        private Orientation _orientation;
        
        [field: SerializeField]
        public bool activeOnly { get; private set; }
        
        [field: SerializeField, OnValueChanged("ApplyOrientation")]
        public bool isInverted { get; private set; }
        
        [field: SerializeField]
        public float spacing { get; set; }
        
        [field: SerializeField]
        public RectOffset padding { get; set; }
        
        [SerializeField, BoxGroup(InspectorNames.GENERATED), Required, ReadOnly]
        private ScrollRect _thisScrollRect;
        
        private Coroutine _calculateProcess;
        private float[] _positions;
        
        public enum Orientation {
            Vertical = 0,
            Horizontal = 1
        }
        
        protected override void Awake() {
            base.Awake();
            _positions = new float[1];
        }
        
        protected override void OnEnable() {
            base.OnEnable();
            _calculateProcess = StartCoroutine(CalculateAfterFrameProcess(() => { }));
        }
        
        protected override void OnDisable() {
            _calculateProcess = this.StopCoroutineResult(_calculateProcess);
            base.OnDisable();
        }
        
        public void SetInvertedState(bool value) {
            isInverted = value;
            CalculateOffsets();
            UpdateCurrentElement();
        }
        
        public void SetOrientation(Orientation value) {
            _orientation = value;
            CalculateOffsets();
            UpdateCurrentElement();
        }
        
        public void FixContent() {
            RectTransform content = _thisScrollRect.content;
            
            if (content != null) {
                float offset = isInverted ? 0f : 1f;
                Vector2 anchors = _orientation == Orientation.Vertical ? new Vector2(0.5f, offset) : new Vector2(offset, 0.5f);
                
                content.anchorMin = anchors;
                content.anchorMax = anchors;
                content.pivot = anchors;
            }
        }
        
        public void Recalculate() => Recalculate(() => { });
        
        public void Recalculate(Action onComplete) {
            if (gameObject.activeInHierarchy == false) {
                return;
            }
            
            _calculateProcess = this.RestartCoroutine(_calculateProcess, CalculateAfterFrameProcess(onComplete));
        }
        
        public void MoveToElement(int elementId) {
            if (IsCanMove(elementId) == false) {
                return;
            }
            
            currentElement = elementId;
            
            if (_orientation == Orientation.Vertical) {
                _thisScrollRect.content.anchoredPosition = new Vector2(_thisScrollRect.content.anchoredPosition.x, _positions[elementId]);
            } else {
                _thisScrollRect.content.anchoredPosition = new Vector2(_positions[elementId], _thisScrollRect.content.anchoredPosition.y);
            }
        }
        
        private bool IsCanMove(int elementId) {
            if (elementId < 0 || elementId >= _positions.Length) {
                return false;
            }
            
            if (_positions.Length == 1) {
                return false;
            }
            
            return true;
        }
        
        private void UpdateCurrentElement() {
            int nextElement;
            
            if (_orientation == Orientation.Vertical) {
                nextElement = _positions.FindClosestIndex(_thisScrollRect.content.anchoredPosition.y);
            } else {
                nextElement = _positions.FindClosestIndex(_thisScrollRect.content.anchoredPosition.x);
            }
            
            if (nextElement == currentElement) {
                return;
            }
            
            currentElement = nextElement;
        }
        
        private void CalculateOffsets() {
            RectTransform content = _thisScrollRect.content;
            int childCount = CalculateChildCount(content);
            List<float> positions = new List<float>(Mathf.Max(1, childCount));
            
            float position = 0;
            
            if (_orientation == Orientation.Vertical) {
                if (isInverted) {
                    position -= padding.bottom;
                } else {
                    position += padding.top;
                }
            } else {
                if (isInverted) {
                    position -= padding.left;
                } else {
                    position += padding.right;
                }
            }
            
            positions.Add(position);
            childCount -= 1;
            
            if (_orientation == Orientation.Vertical) {
                for (int childId = 0; childId < childCount; childId++) {
                    if (content.GetChild(childId) is RectTransform elementRect) {
                        if (activeOnly && elementRect.gameObject.activeSelf == false) {
                            continue;
                        }
                        
                        if (isInverted) {
                            position -= elementRect.sizeDelta.y + spacing;
                        } else {
                            position += elementRect.sizeDelta.y + spacing;
                        }
                    }
                    
                    positions.Add(position);
                }
            } else {
                for (int childId = 0; childId < childCount; childId++) {
                    if (content.GetChild(childId) is RectTransform elementRect) {
                        if (activeOnly && elementRect.gameObject.activeSelf == false) {
                            continue;
                        }
                        
                        if (isInverted) {
                            position -= elementRect.sizeDelta.x + spacing;
                        } else {
                            position += elementRect.sizeDelta.x + spacing;
                        }
                    }
                    
                    positions.Add(position);
                }
            }
            
            _positions = positions.ToArray();
            
            int elementId = Mathf.Clamp(currentElement, 0, _positions.Length - 1);
            currentElement = elementId;
        }
        
        private int CalculateChildCount(RectTransform parent) {
            int childCount = parent.childCount;
            
            if (activeOnly) {
                for (int childId = childCount - 1; childId >= 0; childId--) {
                    if (parent.GetChild(childId).gameObject.activeSelf == false) {
                        childCount--;
                    }
                }
            }
            
            return childCount;
        }
        
        private IEnumerator CalculateAfterFrameProcess(Action onComplete) {
            yield return new WaitForEndOfFrame();
            CalculateOffsets();
            onComplete.Invoke();
        }
        
        public void Validate(SelfValidationResult result) {
        #if UNITY_EDITOR && ODIN_INSPECTOR
            if (this.ValidateNotCurrent(_thisScrollRect)) {
                result.AddErrorNotCurrent<ScrollRect>().WithFix(() => this.FixComponent(out _thisScrollRect));
            }
            
            if (_thisScrollRect != null) {
                if (_thisScrollRect.content == null) {
                    result.AddError($"{nameof(ScrollRect)} content is required!");
                }
            }
        #endif
        }
        
    #if UNITY_EDITOR
        
        [ContextMenu(InspectorNames.SOFT_RESET)]
        protected override void Reset() {
            _thisScrollRect = GetComponent<ScrollRect>();
            ApplyOrientation();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        private void ApplyOrientation() {
            RectTransform content = _thisScrollRect.content;
            
            if (content == null) {
                return;
            }
            
            float offset = isInverted ? 0f : 1f;
            Vector2 anchors = _orientation == Orientation.Vertical ? new Vector2(0.5f, offset) : new Vector2(offset, 0.5f);
            
            content.anchorMin = anchors;
            content.anchorMax = anchors;
            
            content.pivot = anchors;
        }
        
    #endif
    }
}