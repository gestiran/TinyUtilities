// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;
using UnityEngine.UI;

namespace TinyUtilities.Extensions.Unity {
    public static class ImageExtension {
        public static void SetSprite(this Image[] images, Sprite sprite) {
            for (int i = 0; i < images.Length; i++) {
                images[i].sprite = sprite;
            }
        }
    }
}