// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
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
    }
}