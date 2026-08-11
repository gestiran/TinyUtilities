// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.Extensions {
    public static class RectTransformExtension {
        public static float CalculateHeight(this RectTransform rectTransform) {
            if (rectTransform == null) {
                return 1;
            }
            
            return rectTransform.sizeDelta.y;
        }
        
        public static float CalculateHeight<T>(this T obj) where T : MonoBehaviour => obj.GetComponent<RectTransform>().CalculateHeight();
        
        public static void ExpandFullscreen(this RectTransform rectTransform) {
            rectTransform.localScale = Vector3.one;
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            
            rectTransform.ForceUpdateRectTransforms();
        }
        
        public static RectTransform FromReference(this RectTransform rectTransform, RectTransform reference) {
            rectTransform.anchorMin = reference.anchorMin;
            rectTransform.anchorMax = reference.anchorMax;
            
            rectTransform.offsetMin = reference.offsetMin;
            rectTransform.offsetMax = reference.offsetMax;
            
            return rectTransform;
        }
        
        public static void LerpBetween(this RectTransform rectTransform, RectTransform from, RectTransform to, float t) {
            rectTransform.anchorMin = Vector2.Lerp(from.anchorMin, to.anchorMin, t);
            rectTransform.anchorMax = Vector2.Lerp(from.anchorMax, to.anchorMax, t);
            
            rectTransform.offsetMin = Vector2.Lerp(from.offsetMin, to.offsetMin, t);
            rectTransform.offsetMax = Vector2.Lerp(from.offsetMax, to.offsetMax, t);
        }
        
        public static void SetParent<T>(this T rectTransforms, Transform parent, bool worldPositionStays = true) where T : IEnumerable<RectTransform> {
            foreach (RectTransform rectTransform in rectTransforms) {
                rectTransform.SetParent(parent, worldPositionStays);
            }
        }
    }
}