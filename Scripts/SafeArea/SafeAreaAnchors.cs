using System;
using Sirenix.OdinInspector;
using TinyUtilities.ScreenOrientation;
using UnityEngine;

namespace TinyUtilities.SafeArea {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeAreaAnchors : MonoBehaviour, ISelfValidator {
        private string _anchorsLandscapeTitle => separate ? "Landscape" : "Anchors";
        private string _linksLandscapeTitle => separate ? "Landscape" : "Links";
        
        [field: SerializeField]
        public bool onEnable { get; private set; }
        
        [field: SerializeField]
        public bool separate { get; private set; }
        
        [field: SerializeField, HideLabel, Title("@" + nameof(_anchorsLandscapeTitle), horizontalLine: false), EnumToggleButtons]
        public SafeAreaUtility.Anchors anchors { get; private set; }
        
        [field: SerializeField, HideLabel, Title("Portrait", horizontalLine: false), ShowIf("separate"), EnumToggleButtons]
        public SafeAreaUtility.Anchors anchorsPortrait { get; private set; }
        
        [field: SerializeField]
        public bool isSoftOffset { get; private set; }
        
        [field: SerializeField, LabelText("@" + nameof(_linksLandscapeTitle)), Required]
        public RectTransform[] landscape { get; private set; } = new RectTransform[0];
        
        [field: SerializeField, ShowIf("separate"), Required]
        public RectTransform[] portrait { get; private set; } = new RectTransform[0];
        
        [field: SerializeField, FoldoutGroup("Generated"), Required]
        public RectTransform thisRectTransform { get; private set; }
        
        private Vector2 _anchoredPosition;
        private Vector2[] _landscapePositions;
        private Vector2[] _portraitPositions;
        
        private void Awake() {
            _anchoredPosition = thisRectTransform.anchoredPosition;
            _landscapePositions = GetAnchoredPositions(landscape);
            _portraitPositions = GetAnchoredPositions(portrait);
            
            ScreenOrientationUtility.Apply(OffsetPortrait, OffsetLandscape, UpdateOffset);
            
            ScreenOrientationUtility.onPortrait += OffsetPortrait;
            ScreenOrientationUtility.onLandscape += OffsetLandscape;
        }
        
        private void OnEnable() {
            if (onEnable) {
                UpdateOffset();
            }
        }
        
        private void OnDisable() {
            if (onEnable) {
                thisRectTransform.anchoredPosition = _anchoredPosition;   
            }
        }
        
        private void OnDestroy() {
            ScreenOrientationUtility.onPortrait -= OffsetPortrait;
            ScreenOrientationUtility.onLandscape -= OffsetLandscape;
        }
        
        private void UpdateOffset() {
            if (ScreenOrientationUtility.IsPortrait()) {
                OffsetPortrait();
            } else {
                OffsetLandscape();
            }
        }
        
        private void OffsetLandscape() {
            if (!SafeAreaUtility.isHave) {
                return;
            }
            
            try {
                Vector2 offset;
                
                if (isSoftOffset) {
                    offset = SafeAreaUtility.CalculateSoftOffset(thisRectTransform, anchors);
                    thisRectTransform.anchoredPosition = _anchoredPosition + offset;
                } else {
                    offset = SafeAreaUtility.CalculateFullOffset(anchors);
                    thisRectTransform.anchoredPosition = _anchoredPosition + offset;
                }
                
                ApplyOffset(landscape, _landscapePositions, offset);
            } catch (MissingReferenceException exception) {
                Debug.LogWarning(new Exception("SafeAreaAnchors.OffsetLandscape - Scene lost.", exception));
            }
        }
        
        private void OffsetPortrait() {
            if (!SafeAreaUtility.isHave) {
                return;
            }
            
            if (separate) {
                try {
                    Vector2 offset;
                    
                    if (isSoftOffset) {
                        offset = SafeAreaUtility.CalculateSoftOffset(thisRectTransform, anchorsPortrait);
                        thisRectTransform.anchoredPosition = _anchoredPosition + offset;
                    } else {
                        offset = SafeAreaUtility.CalculateFullOffset(anchorsPortrait);
                        thisRectTransform.anchoredPosition = _anchoredPosition + offset;
                    }
                    
                    ApplyOffset(portrait, _portraitPositions, offset);
                } catch (MissingReferenceException exception) {
                    Debug.LogWarning(new Exception("SafeAreaAnchors.OffsetPortrait - Scene lost.", exception));
                }
            } else {
                OffsetLandscape();
            }
        }
        
        private Vector2[] GetAnchoredPositions(RectTransform[] transforms) {
            Vector2[] result = new Vector2[transforms.Length];
            
            for (int i = 0; i < transforms.Length; i++) {
                result[i] = transforms[i].anchoredPosition;
            }
            
            return result;
        }
        
        private void ApplyOffset(RectTransform[] transforms, Vector2[] start, Vector2 offset) {
            for (int i = 0; i < transforms.Length; i++) {
                transforms[i].anchoredPosition = start[i] + offset;
            }
        }
        
        public void Validate(SelfValidationResult result) {
        #if UNITY_EDITOR
            Validate(anchors, result);
            Validate(landscape, result);
            
            if (separate) {
                Validate(anchorsPortrait, result);
                Validate(portrait, result);
            }
            
        #endif
        }
        
    #if UNITY_EDITOR
        
        private void Validate(SafeAreaUtility.Anchors value, SelfValidationResult result) {
            if (value == SafeAreaUtility.Anchors.None) {
                result.AddWarning("Invalid anchors - None!");
            }
            
            if (value.HasFlag(SafeAreaUtility.Anchors.Top) && value.HasFlag(SafeAreaUtility.Anchors.Bottom)) {
                result.AddError("Invalid anchors - Top and Bottom!");
            }
            
            if (value.HasFlag(SafeAreaUtility.Anchors.Left) && value.HasFlag(SafeAreaUtility.Anchors.Right)) {
                result.AddError("Invalid anchors - Left and Right!");
            }
        }
        
        private void Validate(RectTransform[] transforms, SelfValidationResult result) {
            for (int i = 0; i < transforms.Length; i++) {
                RectTransform current = transforms[i];
                
                SafeAreaAnchors safeAreaAnchors = current.GetComponent<SafeAreaAnchors>();
                
                if (safeAreaAnchors != null) {
                    result.AddError($"Invalid {current.gameObject.name} offsets!").WithFix(() => RemoveSafeAreaAnchors(current, safeAreaAnchors));
                }
            }
        }
        
        private void RemoveSafeAreaAnchors(RectTransform original, SafeAreaAnchors component) {
            DestroyImmediate(component);
            UnityEditor.EditorUtility.SetDirty(original.gameObject);
        }
        
        [ContextMenu(InspectorNames.SOFT_RESET)]
        private void Reset() {
            thisRectTransform = GetComponent<RectTransform>();
            anchors = SafeAreaUtility.CalculateAnchors(thisRectTransform);
            anchorsPortrait = SafeAreaUtility.CalculateAnchors(thisRectTransform);
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    #endif
    }
}