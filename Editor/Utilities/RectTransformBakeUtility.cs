// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using TinyUtilities.Unity;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TinyUtilities.Editor.Utilities {
    public static class RectTransformBakeUtility {
        private const string _NAME = "Bake Size";
        
        [MenuItem("CONTEXT/RectTransform/" + _NAME)]
        private static void BakeSize(MenuCommand command) {
            if (command.context is RectTransform rectTransform) {
                List<Object> result = new List<Object>();
                FillRecordObjects(rectTransform, result);
                
                Undo.RecordObjects(result.ToArray(), _NAME);
                
                rectTransform.BakeScale();
            }
        }
        
        private static void FillRecordObjects(RectTransform rectTransform, List<Object> records) {
            records.Add(rectTransform);
            
            if (rectTransform.TryGetComponent(out Image image)) {
                records.Add(image);
            }
            
            if (rectTransform.TryGetComponent(out Text text)) {
                records.Add(text);
            }
            
            if (rectTransform.TryGetComponent(out TextMeshProUGUI textMeshPro)) {
                records.Add(textMeshPro);
            }
            
            if (rectTransform.TryGetComponent(out HorizontalLayoutGroup horizontalLayoutGroup)) {
                records.Add(horizontalLayoutGroup);
            }
            
            if (rectTransform.TryGetComponent(out VerticalLayoutGroup verticalLayoutGroup)) {
                records.Add(verticalLayoutGroup);
            }
            
            if (rectTransform.TryGetComponent(out GridLayoutGroup gridLayoutGroup)) {
                records.Add(gridLayoutGroup);
            }
            
            if (rectTransform.TryGetComponent(out LayoutElement layoutElement)) {
                records.Add(layoutElement);
            }
            
            int childCount = rectTransform.childCount;
            
            for (int childId = 0; childId < childCount; childId++) {
                if (rectTransform.GetChild(childId) is RectTransform child) {
                    FillRecordObjectsNR(child, records);
                }
            }
        }
        
        private static void FillRecordObjectsNR(RectTransform rectTransform, List<Object> records) => FillRecordObjects(rectTransform, records);
    }
}