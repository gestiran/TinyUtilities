// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TinyUtilities.Extensions.Global;
using TinyUtilities.Extensions.Unity;
using TinyUtilities.Validation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if DOTWEEN
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
#endif

namespace TinyUtilities.Components {
    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect))]
    [RequireComponent(typeof(RectTransform))]
    public sealed class ScrollElementsList : UIBehaviour, ISelfValidator, ICoroutineRunner, IBeginDragHandler, IDragHandler, IEndDragHandler {
        public ScrollRect scroll => _thisScrollRect;
        
        public int elementsCount => _positions.Length;
        
        public int currentElement { get; private set; }
        
        [SerializeField, OnValueChanged("ApplyOrientation")]
        private Orientation _orientation;
        
        [field: SerializeField]
        public bool activeOnly { get; private set; }
        
        [field: SerializeField, OnValueChanged("ApplyOrientation")]
        public bool isInverted { get; private set; }
        
        [field: SerializeField]
        public bool isSkippedPadding { get; private set; }
        
    #if DOTWEEN
        [field: SerializeField]
        public Ease ease { get; private set; } = Ease.OutBack;
    #endif
        
        [field: SerializeField]
        public float speed { get; private set; } = 100f;
        
        [SerializeField, Required]
        private ScrollButtonMove[] _buttons;
        
        [SerializeField]
        public UnityEvent<int> onCurrentElementChanged;
        
        [SerializeField, FoldoutGroup(InspectorNames.GENERATED), Required, ReadOnly]
        private ScrollRect _thisScrollRect;
        
        [SerializeField, FoldoutGroup(InspectorNames.GENERATED), Required, ReadOnly]
        private HorizontalOrVerticalLayoutGroup _contentLayoutGroup;
        
        private Coroutine _calculateProcess;
        private float[] _positions;
        
        private enum Orientation {
            Vertical = 0,
            Horizontal = 1
        }
        
        private const float _MAX_DURATION = 4f;
        
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
        
        public void OnBeginDrag(PointerEventData eventData) => HideButtons();
        
        public void OnEndDrag(PointerEventData eventData) => MoveToElement(currentElement);
        
        public void OnDrag(PointerEventData eventData) => UpdateCurrentElement();
        
        public void SetInvertedState(bool value) {
            isInverted = value;
            CalculateOffsets();
            UpdateCurrentElement();
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
            
            if (elementId != currentElement) {
                onCurrentElementChanged.Invoke(elementId);
            }
            
            currentElement = elementId;
            DisableScroll();
            
        #if DOTWEEN
            
            Tweener tween;
            
            if (_orientation == Orientation.Vertical) {
                float duration = Mathf.Abs(_thisScrollRect.content.anchoredPosition.y - _positions[elementId]) / speed;
                tween = DOAnchorPosY(_thisScrollRect.content, _positions[elementId], Mathf.Min(duration, _MAX_DURATION));
            } else {
                float duration = Mathf.Abs(_thisScrollRect.content.anchoredPosition.x - _positions[elementId]) / speed;
                tween = DOAnchorPosX(_thisScrollRect.content, _positions[elementId], Mathf.Min(duration, _MAX_DURATION));
            }
            
            tween.SetEase(ease).OnComplete(EnableScroll).SetUpdate(true);
            
        #else
            MoveToElementForce(elementId);
        #endif
        }
        
        public void MoveToElementForce(int elementId) {
            if (IsCanMove(elementId) == false) {
                return;
            }
            
            if (elementId != currentElement) {
                onCurrentElementChanged.Invoke(elementId);
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
            
            onCurrentElementChanged.Invoke(nextElement);
            currentElement = nextElement;
        }
        
        private void EnableScroll() {
            _thisScrollRect.enabled = true;
            UpdateButtons();
        }
        
        private void DisableScroll() {
            _thisScrollRect.enabled = false;
            HideButtons();
        }
        
        private void HideButtons() {
            for (int i = 0; i < _buttons.Length; i++) {
                _buttons[i].Hide();
            }
        }
        
        private void UpdateButtons() {
            for (int i = 0; i < _buttons.Length; i++) {
                _buttons[i].UpdateActiveState();
            }
        }
        
        private void CalculateOffsets() {
            RectTransform content = _thisScrollRect.content;
            int childCount = CalculateChildCount(content);
            List<float> positions = new List<float>(Mathf.Max(1, childCount));
            
            float spacing = _contentLayoutGroup.spacing;
            float position = 0;
            
            if (isSkippedPadding == false) {
                if (_orientation == Orientation.Vertical) {
                    if (isInverted) {
                        position -= _contentLayoutGroup.padding.bottom;
                    } else {
                        position += _contentLayoutGroup.padding.top;
                    }
                } else {
                    if (isInverted) {
                        position -= _contentLayoutGroup.padding.left;
                    } else {
                        position += _contentLayoutGroup.padding.right;
                    }
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
            
            if (elementId != currentElement) {
                onCurrentElementChanged.Invoke(elementId);
            }
            
            currentElement = elementId;
        }
        
        private int CalculateChildCount(RectTransform parent) {
            int childCount = parent.childCount;
            
            if (activeOnly) {
                for (int childId = 0; childId < childCount; childId++) {
                    if (parent.GetChild(childId).gameObject.activeSelf == false) {
                        childCount--;
                    }
                }   
            }
            
            return childCount;
        }
        
    #if DOTWEEN
        
        private static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorPosY(RectTransform target, float endValue, float duration, bool snapping = false) {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.anchoredPosition, x => target.anchoredPosition = x, new Vector2(0, endValue), duration);
            t.SetOptions(AxisConstraint.Y, snapping).SetTarget(target);
            return t;
        }
        
        private static TweenerCore<Vector2, Vector2, VectorOptions> DOAnchorPosX(RectTransform target, float endValue, float duration, bool snapping = false) {
            TweenerCore<Vector2, Vector2, VectorOptions> t = DOTween.To(() => target.anchoredPosition, x => target.anchoredPosition = x, new Vector2(endValue, 0), duration);
            t.SetOptions(AxisConstraint.X, snapping).SetTarget(target);
            return t;
        }
        
    #endif
        
        private IEnumerator CalculateAfterFrameProcess(Action onComplete) {
            yield return new WaitForEndOfFrame();
            CalculateOffsets();
            UpdateButtons();
            onComplete.Invoke();
        }
        
        public void Validate(SelfValidationResult result) {
        #if UNITY_EDITOR
            if (this.ValidateNotCurrent(_thisScrollRect)) {
                result.AddErrorNotCurrent<ScrollRect>().WithFix(() => this.FixComponent(out _thisScrollRect));
            }
            
            if (_thisScrollRect != null) {
                if (_thisScrollRect.content == null) {
                    result.AddError($"{nameof(ScrollRect)} content is required!");
                } else {
                    RectTransform content = _thisScrollRect.content;
                    
                    float offset = isInverted ? 0f : 1f;
                    Vector2 anchors = _orientation == Orientation.Vertical ? new Vector2(0.5f, offset) : new Vector2(offset, 0.5f);
                    
                    if (content.anchorMin != anchors) {
                        result.AddError("Invalid content anchors!").WithFix(() => content.anchorMin = anchors);
                    }
                    
                    if (content.anchorMax != anchors) {
                        result.AddError("Invalid content anchors!").WithFix(() => content.anchorMax = anchors);
                    }
                    
                    if (content.pivot != anchors) {
                        result.AddError("Invalid content pivot!").WithFix(() => content.pivot = anchors);
                    }
                }
                
                if (_thisScrollRect.content.ValidateNotCurrent(_contentLayoutGroup)) {
                    result.AddErrorNotCurrent<HorizontalOrVerticalLayoutGroup>().WithFix(() => _thisScrollRect.content.FixComponent(out _contentLayoutGroup));
                }
            }
        #endif
        }
        
    #if UNITY_EDITOR
        
        [ContextMenu(InspectorNames.SOFT_RESET)]
        protected override void Reset() {
            _thisScrollRect = GetComponent<ScrollRect>();
            
            if (_thisScrollRect != null && _thisScrollRect.content != null) {
                _contentLayoutGroup = _thisScrollRect.content.GetComponent<HorizontalOrVerticalLayoutGroup>();
            }
            
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