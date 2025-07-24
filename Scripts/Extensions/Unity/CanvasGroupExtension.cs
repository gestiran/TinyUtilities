// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;
using UnityEngine;

namespace TinyUtilities.Extensions.Unity {
    public static class CanvasGroupExtension {
        public static void SetAlpha<T>(this T canvasGroups, float alpha) where T : IEnumerable<CanvasGroup> {
            foreach (CanvasGroup canvasGroup in canvasGroups) {
                canvasGroup.alpha = alpha;
            }
        }
    }
}