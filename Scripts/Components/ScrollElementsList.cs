// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections;
using Sirenix.OdinInspector;
using TinyUtilities.Extensions.Global;
using TinyUtilities.Extensions.Unity;
using TinyUtilities.Validation;
using UnityEngine;
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
        public int currentElement { get; private set; }
        public int elementsCount => _positions.Length;
        
        [SerializeField]
        private Orientation _orientation;
        
    #if DOTWEEN
        [SerializeField]
        private Ease _ease = Ease.OutBack;
    #endif
        
        [SerializeField, Min(1f)]
        private float _speed = 10f;
        
        [SerializeField, Required]
        private ScrollButtonMove[] _buttons;
        
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
            _calculateProcess = StartCoroutine(CalculateProcess());
        }
        
        protected override void OnDisable() {
            _calculateProcess = this.StopCoroutineResult(_calculateProcess);
            base.OnDisable();
        }
        
        public void OnBeginDrag(PointerEventData eventData) => HideButtons();
        
        public void OnEndDrag(PointerEventData eventData) => MoveToElement(currentElement);
        
        public void OnDrag(PointerEventData eventData) {
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
        
        public void Recalculate() {
            if (gameObject.activeInHierarchy == false) {
                return;
            }
            
            _calculateProcess = this.RestartCoroutine(_calculateProcess, CalculateProcess());
        }
        
        public void MoveToElement(int elementId) {
            if (elementId < 0 || elementId >= _positions.Length) {
                return;
            }
            
            currentElement = elementId;
            DisableScroll();
            
        #if DOTWEEN
            
            Tweener tween;
            
            if (_orientation == Orientation.Vertical) {
                float duration = Mathf.Abs(_thisScrollRect.content.anchoredPosition.y - _positions[elementId]) / _speed;
                tween = DOAnchorPosY(_thisScrollRect.content, _positions[elementId], Mathf.Min(duration, _MAX_DURATION));
            } else {
                float duration = Mathf.Abs(_thisScrollRect.content.anchoredPosition.x - _positions[elementId]) / _speed;
                tween = DOAnchorPosX(_thisScrollRect.content, _positions[elementId], Mathf.Min(duration, _MAX_DURATION));
            }
            
            tween.SetEase(_ease).OnComplete(EnableScroll).SetUpdate(true);
            
        #else
        
            MoveToElementForce(elementId);
            
        #endif
            
        }
        
        public void MoveToElementForce(int elementId) {
            if (elementId < 0 || elementId >= _positions.Length) {
                return;
            }
            
            currentElement = elementId;
            
            if (_orientation == Orientation.Vertical) {
                _thisScrollRect.content.anchoredPosition = new Vector2(_thisScrollRect.content.anchoredPosition.x, _positions[elementId]);
            } else {
                _thisScrollRect.content.anchoredPosition = new Vector2(_positions[elementId], _thisScrollRect.content.anchoredPosition.y);
            }
            
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
            int childCount = content.childCount;
            _positions = new float[Mathf.Max(1, content.childCount)];
            
            float spacing = _contentLayoutGroup.spacing;
            float offset = 0;
            _positions[0] = offset;
            
            if (_orientation == Orientation.Vertical) {
                for (int childId = 0, positionId = 1; childId < childCount && positionId < _positions.Length; childId++, positionId++) {
                    if (content.GetChild(childId) is RectTransform elementRect) {
                        offset -= elementRect.sizeDelta.y + spacing;
                    }
                    
                    _positions[positionId] = offset;
                }
            } else {
                for (int childId = 0, positionId = 1; childId < childCount && positionId < _positions.Length; childId++, positionId++) {
                    if (content.GetChild(childId) is RectTransform elementRect) {
                        offset -= elementRect.sizeDelta.x + spacing;
                    }
                    
                    _positions[positionId] = offset;
                }
            }
            
            currentElement = Mathf.Clamp(currentElement, 0, _positions.Length - 1);
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
        
        private IEnumerator CalculateProcess() {
            yield return null;
            CalculateOffsets();
            UpdateButtons();
        }
        
        public void Validate(SelfValidationResult result) {
        #if UNITY_EDITOR
            if (this.ValidateNotCurrent(_thisScrollRect)) {
                result.AddErrorNotCurrent<ScrollRect>().WithFix(() => this.FixComponent(out _thisScrollRect));
            }
            
            if (_thisScrollRect != null) {
                if (_thisScrollRect.content == null) {
                    result.AddError($"{nameof(ScrollRect)} content is required!");
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
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    #endif
    }
}