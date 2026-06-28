// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TMPro;

namespace TinyUtilities.Extensions {
    public static class TextMeshProUGUIExtension {
        public static void SetText<T>(this T[] objects, string sourceText) where T : TMP_Text {
            for (int i = 0; i < objects.Length; i++) {
                objects[i].SetText(sourceText);
            }
        }
    }
}