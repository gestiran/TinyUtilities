// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Diagnostics.Contracts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.Components {
    [SelectionBase]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ScrollRect))]
    [RequireComponent(typeof(RectTransform))]
    public sealed class ScrollCardsGroup : UIBehaviour, ISelfValidator {
        public ScrollRect scroll => _thisScrollRect;
        public RectTransform content => _thisScrollRect.content;
        public bool isOpened { get; private set; }
        public int currentId { get; private set; }
        
        [field: SerializeField]
        public Vector2 cellSize { get; private set; } = Vector2.zero;
        
        [field: SerializeField]
        public Vector2 spacing { get; private set; } = Vector2.zero;
        
        [field: SerializeField]
        public Orientation orientation { get; private set; }
        
        [field: SerializeField, BoxGroup("Duration")]
        public bool ignoreTimeScale { get; private set; } = true;
        
        [field: SerializeField, BoxGroup("Duration"), LabelText("Open"), SuffixLabel("(sec)", true)]
        public float durationOpen { get; private set; } = 0.3f;
        
        [field: SerializeField, BoxGroup("Duration"), LabelText("Close"), SuffixLabel("(sec)", true)]
        public float durationClose { get; private set; } = 0.2f;
        
        [field: SerializeField, BoxGroup("Scale"), LabelText("Main")]
        public Vector3 scaleMain { get; private set; } = Vector3.one;
        
        [field: SerializeField, BoxGroup("Scale"), LabelText("Sub")]
        public Vector3 scaleSub { get; private set; } = new Vector3(0.8f, 0.8f, 0.8f);
        
        [field: SerializeField, BoxGroup("Offset"), LabelText("Center")]
        public Vector2 offsetCenter { get; private set; } = Vector2.zero;
        
        [field: SerializeField, BoxGroup("Offset"), LabelText("Sub")]
        public Vector2 offsetSubCards { get; private set; } = Vector2.zero;
        
        [SerializeField, FoldoutGroup(InspectorNames.GENERATED), ReadOnly]
        private ScrollRect _thisScrollRect;
        
        private Vector3 _contentLocalPosition;
        private Transform[] _cards;
        
        public enum Orientation : byte {
            Horizontal,
            Vertical,
        }
        
        protected override void Start() {
            _contentLocalPosition = _thisScrollRect.content.localPosition;
            
            if (_cards == null) {
                _cards = RecalculateCards();
            }
            
            _thisScrollRect.enabled = false;
            RecalculateContentLocalPosition();
        }
        
        public void ForceUpdate() => ForceUpdate(currentId);
        
        public void ForceUpdate(int current) {
            _cards = RecalculateCards();
            
            RecalculateContentSize();
            ChangeCurrent(current);
            GetCardIds(_cards.Length, out int leftId, out int rightId);
            
            for (int cardId = 0; cardId < _cards.Length; cardId++) {
                Transform card = _thisScrollRect.content.GetChild(cardId);
                
                if (cardId == currentId) {
                    card.localPosition = new Vector3(offsetCenter.x, offsetCenter.y);
                } else if (cardId == leftId) {
                    card.localPosition = new Vector3(offsetCenter.x - offsetSubCards.x, offsetCenter.y - offsetSubCards.y);
                    card.localScale = scaleSub;
                } else if (cardId == rightId) {
                    card.localPosition = new Vector3(offsetCenter.x + offsetSubCards.x, offsetCenter.y + offsetSubCards.y);
                    card.localScale = scaleSub;
                } else {
                    card.localPosition = new Vector3(offsetCenter.x, offsetCenter.y);
                    card.localScale = scaleSub * 0.5f;
                }
            }
            
            ReorderCards(leftId, rightId);
        }
        
        public void Open() {
            if (isOpened) {
                return;
            }
            
            StopAnimation();
            
            Vector3 localPosition;
            
            if (orientation == Orientation.Horizontal) {
                localPosition = new Vector3(_contentLocalPosition.x - (cellSize.x + spacing.x) * currentId, _contentLocalPosition.y);
            } else {
                localPosition = new Vector3(_contentLocalPosition.x, _contentLocalPosition.y - (cellSize.y + spacing.y) * currentId);
            }
            
            _thisScrollRect.content.DOLocalMove(localPosition, durationOpen).SetUpdate(ignoreTimeScale);
            
            for (int cardId = 0; cardId < _cards.Length; cardId++) {
                Transform card = _cards[cardId];
                float moveDuration = durationOpen + Mathf.Abs(currentId - cardId) * durationOpen;
                
                if (orientation == Orientation.Horizontal) {
                    localPosition = new Vector3(offsetCenter.x + (cellSize.x + spacing.x) * cardId, offsetCenter.y);
                } else {
                    localPosition = new Vector3(offsetCenter.x, offsetCenter.y + (cellSize.y + spacing.y) * cardId);
                }
                
                card.DOScale(scaleMain, durationOpen).SetUpdate(ignoreTimeScale);
                card.DOLocalMove(localPosition, moveDuration).SetUpdate(ignoreTimeScale);
            }
            
            _thisScrollRect.enabled = true;
            isOpened = true;
        }
        
        public void Close() {
            if (isOpened == false) {
                return;
            }
            
            StopAnimation();
            
            _thisScrollRect.content.DOLocalMove(_contentLocalPosition, durationClose).SetUpdate(ignoreTimeScale);
            
            GetCardIds(_cards.Length, out int leftId, out int rightId);
            
            for (int cardId = 0; cardId < _cards.Length; cardId++) {
                Transform card = _cards[cardId];
                float moveDuration = durationClose + Mathf.Abs(currentId - cardId) * durationClose;
                
                if (cardId == currentId) {
                    card.DOLocalMove(new Vector3(offsetCenter.x, offsetCenter.y), moveDuration).SetUpdate(ignoreTimeScale);
                } else if (cardId == leftId) {
                    Vector3 localPosition = new Vector3(offsetCenter.x - offsetSubCards.x, offsetCenter.y - offsetSubCards.y);
                    card.DOLocalMove(localPosition, moveDuration).SetUpdate(ignoreTimeScale);
                    card.DOScale(scaleSub, durationClose).SetUpdate(ignoreTimeScale);
                } else if (cardId == rightId) {
                    Vector3 localPosition = new Vector3(offsetCenter.x + offsetSubCards.x, offsetCenter.y + offsetSubCards.y);
                    card.DOLocalMove(localPosition, moveDuration).SetUpdate(ignoreTimeScale);
                    card.DOScale(scaleSub, durationClose).SetUpdate(ignoreTimeScale);
                } else {
                    card.DOLocalMove(new Vector3(offsetCenter.x, offsetCenter.y), moveDuration).SetUpdate(ignoreTimeScale);
                    card.DOScale(scaleSub * 0.5f, durationClose).SetUpdate(ignoreTimeScale);
                }
            }
            
            ReorderCards(leftId, rightId);
            
            _thisScrollRect.enabled = false;
            isOpened = false;
        }
        
        public void ChangeCurrent(int value) => currentId = Mathf.Clamp(value, 0, _cards.Length - 1);
        
        public void RecalculateContentSize() {
            if (orientation == Orientation.Horizontal) {
                _thisScrollRect.content.sizeDelta = new Vector2((cellSize.x + spacing.x) * _cards.Length, cellSize.y);
            } else {
                _thisScrollRect.content.sizeDelta = new Vector2(cellSize.y, (cellSize.y + spacing.y) * _cards.Length);
            }
        }
        
        public void RecalculateContentLocalPosition() => _thisScrollRect.content.localPosition = _contentLocalPosition;
        
        private void StopAnimation() {
            _thisScrollRect.content.DOKill();
            
            for (int childId = 0; childId < _cards.Length; childId++) {
                _cards[childId].DOKill();
            }
        }
        
        private void GetCardIds(int max, out int leftId, out int rightId) {
            if (currentId > 0) {
                leftId = currentId - 1;
                rightId = currentId + 1 < max ? currentId + 1 : currentId - 2;
            } else {
                leftId = currentId + 1;
                rightId = currentId + 2;
            }
        }
        
        private void ReorderCards(int leftId, int rightId) {
            if (currentId < _cards.Length) {
                _cards[currentId].SetAsFirstSibling();
            }
            
            if (rightId < _cards.Length) {
                _cards[rightId].SetAsFirstSibling();
            }
            
            if (leftId < _cards.Length) {
                _cards[leftId].SetAsFirstSibling();
            }
            
            for (int cardId = _cards.Length - 1; cardId >= 0; cardId--) {
                if (cardId == currentId || cardId == leftId || cardId == rightId) {
                    continue;
                }
                
                _cards[cardId].SetAsFirstSibling();
            }
        }
        
        [Pure]
        private Transform[] RecalculateCards() {
            Transform[] cards = new Transform[_thisScrollRect.content.childCount];
            
            for (int cardId = 0; cardId < cards.Length; cardId++) {
                cards[cardId] = _thisScrollRect.content.GetChild(cardId);
            }
            
            return cards;
        }
        
        public void Validate(SelfValidationResult result) {
        #if UNITY_EDITOR
            
            if (_thisScrollRect != null) {
                if (_thisScrollRect.content == null) {
                    result.AddError($"Required {nameof(ScrollRect)} content!");
                } else {
                    if (orientation == Orientation.Vertical) {
                        _thisScrollRect.content.pivot = new Vector2(0.5f, 0);
                    } else {
                        _thisScrollRect.content.pivot = new Vector2(0, 0.5f);
                    }
                    
                    _thisScrollRect.content.anchorMin = new Vector2(0.5f, 0.5f);
                    _thisScrollRect.content.anchorMax = new Vector2(0.5f, 0.5f);
                }
            } else {
                result.AddError($"Required {nameof(ScrollRect)}!");
            }
            
        #endif
        }
        
    #if UNITY_EDITOR
        
        [ContextMenu(InspectorNames.SOFT_RESET)]
        protected override void Reset() {
            _thisScrollRect = GetComponent<ScrollRect>();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    #endif
    }
}