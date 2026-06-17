// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using TinyUtilities.Extensions.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TinyUtilities.Unity {
    public static class RectTransformBakeUtility {
        public static void BakeScale(this RectTransform rectTransform) {
            Vector3 localScale = rectTransform.localScale;
            
            int childCount = rectTransform.childCount;
            List<RectTransform> children = new List<RectTransform>(childCount);
            
            for (int childId = 0; childId < childCount; childId++) {
                if (rectTransform.GetChild(childId) is RectTransform child) {
                    children.Add(child);
                }
            }
            
            if (localScale != Vector3.one) {
                children.SetParent(rectTransform.parent, true);
                
                BakeRectTransformScale(rectTransform, localScale);
                BakeImageScale(rectTransform, localScale);
                BakeTextScale(rectTransform, localScale);
                BakeTextMeshProScale(rectTransform, localScale);
                BakeHorizontalLayoutGroupScale(rectTransform, localScale);
                BakeVerticalLayoutGroupScale(rectTransform, localScale);
                BakeGridLayoutGroupScale(rectTransform, localScale);
                BakeLayoutElementScale(rectTransform, localScale);
                
                children.SetParent(rectTransform, true);
            }
            
            for (int childId = 0; childId < children.Count; childId++) {
                BakeScaleNR(children[childId]);
            }
        }
        
        private static void BakeScaleNR(this RectTransform rectTransform) => BakeScale(rectTransform);
        
        private static void BakeRectTransformScale(RectTransform rectTransform, Vector3 scale) {
            Rect rect = rectTransform.rect;
            
            Vector2 offsetMin = rectTransform.offsetMin;
            Vector2 offsetMax = rectTransform.offsetMax;
            
            offsetMin.x += rect.xMin * (scale.x - 1f);
            offsetMin.y += rect.yMin * (scale.y - 1f);
            
            offsetMax.x += rect.xMax * (scale.x - 1f);
            offsetMax.y += rect.yMax * (scale.y - 1f);
            
            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
            
            rectTransform.localScale = Vector3.one;
            
        #if UNITY_EDITOR
            SetDirty(rectTransform);
        #endif
        }
        
        private static void BakeImageScale(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out Image image)) {
                image.pixelsPerUnitMultiplier /= Mathf.Max(scale.x, scale.y);
                
            #if UNITY_EDITOR
                SetDirty(image);
            #endif
            }
        }
        
        private static void BakeTextScale(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out Text text)) {
                text.fontSize = Mathf.RoundToInt(text.fontSize * scale.y);
                
            #if UNITY_EDITOR
                SetDirty(text);
            #endif
            }
        }
        
        private static void BakeTextMeshProScale(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out TextMeshProUGUI textMeshPro)) {
                textMeshPro.fontSize *= scale.y;
                textMeshPro.fontSizeMin *= scale.y;
                textMeshPro.fontSizeMax *= scale.y;
                
            #if UNITY_EDITOR
                SetDirty(textMeshPro);
            #endif
            }
        }
        
        private static void BakeHorizontalLayoutGroupScale(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out HorizontalLayoutGroup horizontalLayoutGroup)) {
                horizontalLayoutGroup.spacing *= scale.x;
                ApplyScaleToPadding(horizontalLayoutGroup.padding, scale);
                horizontalLayoutGroup.childForceExpandWidth = horizontalLayoutGroup.childForceExpandWidth;
                horizontalLayoutGroup.childForceExpandHeight = horizontalLayoutGroup.childForceExpandHeight;
                
            #if UNITY_EDITOR
                SetDirty(horizontalLayoutGroup);
            #endif
            }
        }
        
        private static void BakeVerticalLayoutGroupScale(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out VerticalLayoutGroup verticalLayoutGroup)) {
                verticalLayoutGroup.spacing *= scale.y;
                ApplyScaleToPadding(verticalLayoutGroup.padding, scale);
                verticalLayoutGroup.childForceExpandWidth = verticalLayoutGroup.childForceExpandWidth;
                verticalLayoutGroup.childForceExpandHeight = verticalLayoutGroup.childForceExpandHeight;
                
            #if UNITY_EDITOR
                SetDirty(verticalLayoutGroup);
            #endif
            }
        }
        
        private static void BakeGridLayoutGroupScale(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out GridLayoutGroup gridLayoutGroup)) {
                gridLayoutGroup.cellSize = new Vector2(gridLayoutGroup.cellSize.x * scale.x, gridLayoutGroup.cellSize.y * scale.y);
                gridLayoutGroup.spacing = new Vector2(gridLayoutGroup.spacing.x * scale.x, gridLayoutGroup.spacing.y * scale.y);
                ApplyScaleToPadding(gridLayoutGroup.padding, scale);
                
            #if UNITY_EDITOR
                SetDirty(gridLayoutGroup);
            #endif
            }
        }
        
        private static void BakeLayoutElementScale(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out LayoutElement layoutElement)) {
                if (layoutElement.preferredWidth > 0) {
                    layoutElement.preferredWidth *= scale.x;
                }
                
                if (layoutElement.preferredHeight > 0) {
                    layoutElement.preferredHeight *= scale.y;
                }
                
                if (layoutElement.minWidth > 0) {
                    layoutElement.minWidth *= scale.x;
                }
                
                if (layoutElement.minHeight > 0) {
                    layoutElement.minHeight *= scale.y;
                }
                
            #if UNITY_EDITOR
                SetDirty(layoutElement);
            #endif
            }
        }
        
        private static void ApplyScaleToPadding(RectOffset padding, Vector3 scale) {
            padding.left = Mathf.RoundToInt(padding.left * scale.x);
            padding.right = Mathf.RoundToInt(padding.right * scale.x);
            padding.top = Mathf.RoundToInt(padding.top * scale.y);
            padding.bottom = Mathf.RoundToInt(padding.bottom * scale.y);
        }
        
    #if UNITY_EDITOR
        
        private static void SetDirty(Object target) {
            if (Application.isPlaying == false) {
                UnityEditor.EditorUtility.SetDirty(target);
            }
        }
        
    #endif
    }
}