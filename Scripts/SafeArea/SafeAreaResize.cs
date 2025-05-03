using System;
using Sirenix.OdinInspector;
using TinyUtilities.ScreenOrientation;
using UnityEngine;

namespace TinyUtilities.SafeArea {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeAreaResize : MonoBehaviour, ISelfValidator {
        private string _anchorsLandscapeTitle => separate ? "Landscape" : "Anchors";
        
        [field: SerializeField]
        public bool onEnable { get; private set; }
        
        [field: SerializeField]
        public bool separate { get; private set; }
        
        [field: SerializeField, HideLabel, Title("@" + nameof(_anchorsLandscapeTitle), horizontalLine: false), EnumToggleButtons]
        public SafeAreaUtility.Anchors anchorsLandscape { get; private set; }
        
        [field: SerializeField, HideLabel, Title("Portrait", horizontalLine: false), ShowIf("separate"), EnumToggleButtons]
        public SafeAreaUtility.Anchors anchorsPortrait { get; private set; }
        
        [field: SerializeField, FoldoutGroup("Generated"), Required]
        public RectTransform thisRectTransform { get; private set; }
        
        private Vector2 _sizeDelta;
        
        private void Awake() {
            _sizeDelta = thisRectTransform.sizeDelta;
            
            ScreenOrientationUtility.Apply(ResizePortrait, ResizeLandscape, Resize);
            
            ScreenOrientationUtility.onPortrait += ResizePortrait;
            ScreenOrientationUtility.onLandscape += ResizeLandscape;
        }
        
        private void OnEnable() {
            if (onEnable) {
                ScreenOrientationUtility.Apply(ResizePortrait, ResizeLandscape, Resize);
            }
        }
        
        private void OnDestroy() {
            ScreenOrientationUtility.onPortrait -= ResizePortrait;
            ScreenOrientationUtility.onLandscape -= ResizeLandscape;
        }
        
        private void Resize() {
            if (ScreenOrientationUtility.IsPortrait()) {
                ResizePortrait();
            } else {
                ResizeLandscape();
            }
        }
        
        private void ResizeLandscape() {
            if (!SafeAreaUtility.isHave) {
                return;
            }
            
            try {
                thisRectTransform.sizeDelta = SafeAreaUtility.CalculateSize(_sizeDelta, anchorsLandscape);
            } catch (MissingReferenceException exception) {
                Debug.LogWarning(new Exception("SafeAreaResize.ResizeLandscape - Scene lost.", exception));
            }
        }
        
        private void ResizePortrait() {
            if (!SafeAreaUtility.isHave) {
                return;
            }
            
            if (separate) {
                try {
                    thisRectTransform.sizeDelta = SafeAreaUtility.CalculateSize(_sizeDelta, anchorsPortrait);
                } catch (MissingReferenceException exception) {
                    Debug.LogWarning(new Exception("SafeAreaResize.ResizePortrait - Scene lost.", exception));
                }
            } else {
                ResizeLandscape();
            }
        }
        
        public void Validate(SelfValidationResult result) {
        #if UNITY_EDITOR
            if (anchorsLandscape == SafeAreaUtility.Anchors.None) {
                result.AddWarning("Invalid anchors - None!");
            }
            
            if (separate && anchorsPortrait == SafeAreaUtility.Anchors.None) {
                result.AddWarning("Invalid anchors - None!");
            }
        #endif
        }
        
    #if UNITY_EDITOR
        [ContextMenu(InspectorNames.SOFT_RESET)]
        private void Reset() {
            thisRectTransform = GetComponent<RectTransform>();
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
    #endif
    }
}