// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TinyUtilities.Extensions.Unity {
    public static class ImageExtension {
        public static T SetSprite<T>(this T images, Sprite sprite) where T : IEnumerable<Image> {
            foreach (Image image in images) {
                image.sprite = sprite;
            }
            
            return images;
        }
    }
}