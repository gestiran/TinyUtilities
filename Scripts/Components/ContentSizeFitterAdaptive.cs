// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TinyUtilities.Components {
    [AddComponentMenu("Layout/Content Size Fitter Adaptive", 141)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class ContentSizeFitterAdaptive : UIBehaviour, ILayoutSelfController {
        public enum FitMode {
            Unconstrained,
            MinSize,
            MinPreferredSize,
            PreferredSize
        }
        
        [field: SerializeField]
        protected FitMode horizontalFit { get; private set; }
        
        [field: SerializeField]
        protected FitMode verticalFit { get; private set; }
        
        [System.NonSerialized]
        private RectTransform m_Rect;
        
        private RectTransform rectTransform {
            get {
                if (m_Rect == null) m_Rect = GetComponent<RectTransform>();
                
                return m_Rect;
            }
        }
        
        // field is never assigned warning
        #pragma warning disable 649
        private DrivenRectTransformTracker m_Tracker;
        #pragma warning restore 649
        
        protected override void OnEnable() {
            base.OnEnable();
            SetDirty();
        }
        
        protected override void OnDisable() {
            m_Tracker.Clear();
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            base.OnDisable();
        }
        
        protected override void OnRectTransformDimensionsChange() {
            SetDirty();
        }
        
        private void HandleSelfFittingAlongAxis(int axis) {
            FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
            
            if (fitting == FitMode.Unconstrained) {
                // Keep a reference to the tracked transform, but don't control its properties:
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.None);
                
                return;
            }
            
            m_Tracker.Add(this, rectTransform, (axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));
            
            // Set size to min or preferred size
            if (fitting == FitMode.MinSize) {
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetMinSize(m_Rect, axis));
            } else if (fitting == FitMode.PreferredSize) {
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetPreferredSize(m_Rect, axis));
            } else {
                float current = LayoutUtility.GetMinSize(m_Rect, axis);
                float preferred = LayoutUtility.GetPreferredSize(m_Rect, axis);
                
                rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, Mathf.Clamp(current, current, preferred));
            }
        }
        
        /// <summary>
        /// Calculate and apply the horizontal component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutHorizontal() {
            m_Tracker.Clear();
            HandleSelfFittingAlongAxis(0);
        }
        
        /// <summary>
        /// Calculate and apply the vertical component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutVertical() {
            HandleSelfFittingAlongAxis(1);
        }
        
        protected void SetDirty() {
            if (!IsActive()) {
                return;
            }
            
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
        
        #if UNITY_EDITOR
        protected override void OnValidate() {
            SetDirty();
        }
        
        #endif
    }
}