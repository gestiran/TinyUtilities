using System.Collections.Generic;
using TinyUtilities.Extensions.Unity;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TinyUtilities.Editor.Utilities {
    public static class RectTransformUtility {
        private const string _NAME = "Bake Size";
        
        [MenuItem("CONTEXT/RectTransform/" + _NAME)]
        private static void BakeSize(MenuCommand command) {
            RectTransform rectTransform = (RectTransform)command.context;
            BakeScale(rectTransform);
        }
        
        public static void BakeScale(this RectTransform rectTransform) {
            Vector3 localScale = rectTransform.localScale;
            
            if (localScale == Vector3.one) {
                return;
            }
            
            Undo.RecordObject(rectTransform, _NAME);
            
            int childCount = rectTransform.childCount;
            List<RectTransform> children = new List<RectTransform>(childCount);
            
            for (int childId = 0; childId < childCount; childId++) {
                if (rectTransform.GetChild(childId) is RectTransform child) {
                    children.Add(child);
                }
            }
            
            children.SetParent(rectTransform.parent, true);
            
            ApplyScaleToRectTransform(rectTransform, localScale);
            ApplyScaleToImage(rectTransform, localScale);
            ApplyScaleToText(rectTransform, localScale);
            ApplyScaleToTextMeshPro(rectTransform, localScale);
            ApplyScaleToHorizontalLayoutGroup(rectTransform, localScale);
            ApplyScaleToVerticalLayoutGroup(rectTransform, localScale);
            ApplyScaleToGridLayoutGroup(rectTransform, localScale);
            ApplyScaleToLayoutElement(rectTransform, localScale);
            
            children.SetParent(rectTransform, true);
            
            for (int childId = 0; childId < children.Count; childId++) {
                BakeScaleNR(children[childId]);
            }
        }
        
        private static void BakeScaleNR(this RectTransform rectTransform) => BakeScale(rectTransform);
        
        private static void ApplyScaleToRectTransform(RectTransform rectTransform, Vector3 scale) {
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
        }
        
        private static void ApplyScaleToImage(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out Image image)) {
                image.pixelsPerUnitMultiplier /= Mathf.Max(scale.x, scale.y);
            }
        }
        
        private static void ApplyScaleToText(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out Text text)) {
                text.fontSize = Mathf.RoundToInt(text.fontSize * scale.y);
            }
        }
        
        private static void ApplyScaleToTextMeshPro(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out TextMeshProUGUI textMeshPro)) {
                textMeshPro.fontSize *= scale.y;
                textMeshPro.fontSizeMin *= scale.y;
                textMeshPro.fontSizeMax *= scale.y;
            }
        }
        
        private static void ApplyScaleToHorizontalLayoutGroup(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out HorizontalLayoutGroup horizontalLayoutGroup)) {
                horizontalLayoutGroup.spacing *= scale.x;
                ApplyScaleToPadding(horizontalLayoutGroup.padding, scale);
                horizontalLayoutGroup.childForceExpandWidth = horizontalLayoutGroup.childForceExpandWidth;
                horizontalLayoutGroup.childForceExpandHeight = horizontalLayoutGroup.childForceExpandHeight;
            }
        }
        
        private static void ApplyScaleToVerticalLayoutGroup(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out VerticalLayoutGroup verticalLayoutGroup)) {
                verticalLayoutGroup.spacing *= scale.y;
                ApplyScaleToPadding(verticalLayoutGroup.padding, scale);
                verticalLayoutGroup.childForceExpandWidth = verticalLayoutGroup.childForceExpandWidth;
                verticalLayoutGroup.childForceExpandHeight = verticalLayoutGroup.childForceExpandHeight;
            }
        }
        
        private static void ApplyScaleToGridLayoutGroup(RectTransform rectTransform, Vector3 scale) {
            if (rectTransform.TryGetComponent(out GridLayoutGroup gridLayoutGroup)) {
                gridLayoutGroup.cellSize = new Vector2(gridLayoutGroup.cellSize.x * scale.x, gridLayoutGroup.cellSize.y * scale.y);
                gridLayoutGroup.spacing = new Vector2(gridLayoutGroup.spacing.x * scale.x, gridLayoutGroup.spacing.y * scale.y);
                ApplyScaleToPadding(gridLayoutGroup.padding, scale);
            }
        }
        
        private static void ApplyScaleToLayoutElement(RectTransform rectTransform, Vector3 scale) {
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
            }
        }
        
        private static void ApplyScaleToPadding(RectOffset padding, Vector3 scale) {
            padding.left = Mathf.RoundToInt(padding.left * scale.x);
            padding.right = Mathf.RoundToInt(padding.right * scale.x);
            padding.top = Mathf.RoundToInt(padding.top * scale.y);
            padding.bottom = Mathf.RoundToInt(padding.bottom * scale.y);
        }
    }
}