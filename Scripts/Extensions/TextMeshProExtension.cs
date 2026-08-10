// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TinyUtilities.Extensions {
    public static class TextMeshProExtension {
        public static void SetText<T>(this T[] objects, string sourceText) where T : TMP_Text {
            for (int i = 0; i < objects.Length; i++) {
                objects[i].SetText(sourceText);
            }
        }
        
        public static void SetText<T>(this List<T> objects, string sourceText) where T : TMP_Text {
            for (int i = 0; i < objects.Count; i++) {
                objects[i].SetText(sourceText);
            }
        }
        
        public static void SetColor<T>(this T[] objects, Color color) where T : TMP_Text {
            for (int i = 0; i < objects.Length; i++) {
                objects[i].color = color;
            }
        }
        
        public static void SetColor<T>(this List<T> objects, Color color) where T : TMP_Text {
            for (int i = 0; i < objects.Count; i++) {
                objects[i].color = color;
            }
        }
    }
}