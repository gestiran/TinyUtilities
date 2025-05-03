using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TinyUtilities.Components {
    [AddComponentMenu("UI/DoubleSlider", 34)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class DoubleSlider : UIBehaviour, ICanvasElement {
        public RectTransform firstRect {
            get => _firstRect;
            set {
                _firstRect = value;
                UpdateCachedReferences();
                UpdateVisuals();
            }
        }
        
        public RectTransform secondRect {
            get => _secondRect;
            set {
                _secondRect = value;
                UpdateCachedReferences();
                UpdateVisuals();
            }
        }
        
        public Direction direction {
            get => _direction;
            set {
                _direction = value;
                UpdateVisuals();
            }
        }
        
        public float size {
            get => _size;
            set {
                _size = value;
                SetFirstValue(_firstValue);
                SetSecondValue(_secondValue);
                UpdateVisuals();
            }
        }
        
        public bool wholeNumbers {
            get => _wholeNumbers;
            set {
                _wholeNumbers = value;
                SetFirstValue(_firstValue);
                SetSecondValue(_secondValue);
                UpdateVisuals();
            }
        }
        
        public float firstValue {
            get => wholeNumbers ? Mathf.Round(_firstValue) : _firstValue;
            set => SetFirstValue(value);
        }
        
        public float secondValue {
            get => wholeNumbers ? Mathf.Round(_secondValue) : _secondValue;
            set => SetSecondValue(value);
        }
        
        public float normalizedFirstValue {
            get {
                if (Mathf.Approximately(0, size)) {
                    return 0;
                }
                
                return Mathf.InverseLerp(0, size, _firstValue);
            }
            set => firstValue = Mathf.Lerp(0, size, value);
        }
        
        public float normalizedSecondValue {
            get {
                if (Mathf.Approximately(0, size)) {
                    return 0;
                }
                
                return Mathf.InverseLerp(0, size, _secondValue);
            }
            set => secondValue = Mathf.Lerp(0, size, value);
        }
        
        public DoubleSliderEvent onValueChanged {
            get => _onValueChanged;
            set => _onValueChanged = value;
        }
        
        private Axis axis => _direction is Direction.LeftToRight or Direction.RightToLeft ? Axis.Horizontal : Axis.Vertical;
        
        private bool reverseValue => _direction is Direction.RightToLeft or Direction.TopToBottom;
        
        [Serializable]
        public class DoubleSliderEvent : UnityEvent<float, float> { }
        
        [SerializeField]
        private RectTransform _firstRect;
        
        [SerializeField]
        private RectTransform _secondRect;
        
        [Space, SerializeField]
        private Direction _direction = Direction.LeftToRight;
        
        [SerializeField]
        private float _size = 1;
        
        [SerializeField]
        private bool _wholeNumbers = false;
        
        [SerializeField]
        private float _firstValue;
        
        [SerializeField]
        private float _secondValue;
        
        [Space, SerializeField]
        private DoubleSliderEvent _onValueChanged = new DoubleSliderEvent();
        
        private Transform _fillTransform;
        private RectTransform _fillContainerRect;
        private bool _delayedUpdateVisuals = false;
        
        public enum Direction {
            LeftToRight, RightToLeft, BottomToTop, TopToBottom,
        }
        
        private enum Axis { Horizontal = 0, Vertical = 1 }
        
        protected DoubleSlider() { }
        
        public void Rebuild(CanvasUpdate executing) {
            #if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout) {
                onValueChanged.Invoke(_firstValue, _secondValue);
            }
            #endif
        }
        
        public void LayoutComplete() { }
        
        public void GraphicUpdateComplete() { }
        
        protected override void OnEnable() {
            UpdateCachedReferences();
            SetFirstValue(_firstValue, false);
            SetSecondValue(_secondValue, false);
            
            UpdateVisuals();
            
            base.OnEnable();
        }
        
        private void Update() {
            if (_delayedUpdateVisuals) {
                _delayedUpdateVisuals = false;
                SetFirstValue(_firstValue, false);
                SetSecondValue(_secondValue, false);
                UpdateVisuals();
            }
        }
        
        protected override void OnDidApplyAnimationProperties() {
            _firstValue = ClampFirstValue(_firstValue);
            _secondValue = ClampSecondValue(_secondValue);
            
            float oldNormalizedFirstValue = normalizedFirstValue;
            float oldNormalizedSecondValue = normalizedSecondValue;
            
            if (_fillContainerRect != null) {
                oldNormalizedFirstValue = (reverseValue ? 1 - _firstRect.anchorMin[(int)axis] : _firstRect.anchorMax[(int)axis]);
                oldNormalizedSecondValue = (reverseValue ? 1 - _secondRect.anchorMin[(int)axis] : _secondRect.anchorMax[(int)axis]);
            }
            
            UpdateVisuals();
            
            if (Mathf.Approximately(oldNormalizedFirstValue, normalizedFirstValue)) {
                UISystemProfilerApi.AddMarker("DoubleSlider.firstValue", this);
                onValueChanged.Invoke(_firstValue, _secondValue);
            } else if (Mathf.Approximately(oldNormalizedSecondValue, normalizedSecondValue)) {
                UISystemProfilerApi.AddMarker("DoubleSlider.secondValue", this);
                onValueChanged.Invoke(_firstValue, _secondValue);
            }
            
            base.OnDidApplyAnimationProperties();
        }
        
        private void UpdateCachedReferences() {
            if (UpdateFirstReference() && UpdateSecondReference()) {
                if (_fillTransform.parent != null) {
                    _fillContainerRect = _fillTransform.parent.GetComponent<RectTransform>();
                }
            } else {
                _fillContainerRect = null;
            }
        }
        
        private bool UpdateFirstReference() {
            if (_firstRect == null) {
                return false;
            }
            
            if (_firstRect == (RectTransform)transform) {
                _firstRect = null;
                
                return false;
            }
            
            _fillTransform = _firstRect.transform;
            
            return true;
        }
        
        private bool UpdateSecondReference() {
            if (_secondRect == null) {
                return false;
            }
            
            if (_secondRect == (RectTransform)transform) {
                _secondRect = null;
                
                return false;
            }
            
            return true;
        }
        
        private float ClampFirstValue(float newValue) {
            newValue = Mathf.Clamp(newValue, 0, size - _secondValue);
            
            if (wholeNumbers) {
                newValue = Mathf.Round(newValue);
            }
            
            return newValue;
        }
        
        private float ClampSecondValue(float newValue) {
            newValue = Mathf.Clamp(newValue, 0, size - _firstValue);
            
            if (wholeNumbers) {
                newValue = Mathf.Round(newValue);
            }
            
            return newValue;
        }
        
        private void SetFirstValue(float newValue, bool sendCallback = true) {
            newValue = ClampFirstValue(newValue);
            
            if (Mathf.Approximately(_firstValue, newValue)) {
                return;
            }
            
            _firstValue = newValue;
            UpdateVisuals();
            
            if (sendCallback) {
                UISystemProfilerApi.AddMarker("DoubleSlider.firstValue", this);
                _onValueChanged.Invoke(newValue, _secondValue);
            }
        }
        
        private void SetSecondValue(float newValue, bool sendCallback = true) {
            newValue = ClampSecondValue(newValue);
            
            if (Mathf.Approximately(_secondValue, newValue)) {
                return;
            }
            
            _secondValue = newValue;
            UpdateVisuals();
            
            if (sendCallback) {
                UISystemProfilerApi.AddMarker("DoubleSlider.secondValue", this);
                _onValueChanged.Invoke(_firstValue, newValue);
            }
        }
        
        protected override void OnRectTransformDimensionsChange() {
            if (!isActiveAndEnabled) {
                return;
            }
            
            UpdateVisuals();
            base.OnRectTransformDimensionsChange();
        }
        
        private void UpdateVisuals() {
            #if UNITY_EDITOR
            if (!Application.isPlaying) {
                UpdateCachedReferences();
            }
            #endif
            
            if (_fillContainerRect != null) {
                Vector2 anchorMin = Vector2.zero;
                Vector2 anchorMax = Vector2.one;
                
                if (reverseValue) {
                    anchorMin[(int)axis] = 1 - normalizedFirstValue;
                } else {
                    anchorMax[(int)axis] = normalizedFirstValue;
                }
                
                _firstRect.anchorMin = anchorMin;
                _firstRect.anchorMax = anchorMax;
                
                anchorMin = anchorMax;
                anchorMin.y = 0;
                
                anchorMax = Vector2.one;
                
                if (reverseValue) {
                    anchorMin[(int)axis] = 1 - normalizedSecondValue;
                } else {
                    anchorMax[(int)axis] = anchorMin[(int)axis] + normalizedSecondValue;
                }
                
                _secondRect.anchorMin = anchorMin;
                _secondRect.anchorMax = anchorMax;
            }
        }
        
        public void SetDirection(Direction newDirection, bool includeRectLayouts) {
            Axis oldAxis = axis;
            bool oldReverse = reverseValue;
            this.direction = newDirection;
            
            if (!includeRectLayouts) {
                return;
            }
            
            if (axis != oldAxis) {
                RectTransformUtility.FlipLayoutAxes(transform as RectTransform, true, true);
            }
            
            if (reverseValue != oldReverse) {
                RectTransformUtility.FlipLayoutOnAxis(transform as RectTransform, (int)axis, true, true);
            }
        }
        
        #if UNITY_EDITOR
        protected override void OnValidate() {
            if (wholeNumbers) {
                _size = Mathf.Round(_size);
            }
            
            if (_size == 0) {
                if (wholeNumbers) {
                    _size = 1;
                } else {
                    _size = 0.0001f;
                }
            }
            
            if (_firstValue < 0) {
                _firstValue = 0;
            }
            
            if (_secondValue < 0) {
                _secondValue = 0;
            }
            
            if (_firstValue + _secondValue > _size) {
                float realSize = _firstValue + _secondValue;
                
                if (realSize > 0) {
                    _firstValue = _size * normalizedFirstValue;
                    _secondValue = _size * normalizedSecondValue;
                }
            }
            
            if (isActiveAndEnabled) {
                UpdateCachedReferences();
                _delayedUpdateVisuals = true;
            }
            
            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying) {
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
            }
            
            base.OnValidate();
        }
        
        #endif
    }
}