// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using UnityEngine;

#if DOTWEEN
using DG.Tweening;
#endif

namespace TinyUtilities.UIEffects {
    public static class UIButtonEffects {
    #if DOTWEEN
        
        public static Tweener DoEffectClick(this Transform transform) {
            return transform.DOScale(transform.localScale * 0.9f, 0.15f).SetEase(Ease.InQuart).SetLoops(2, LoopType.Yoyo);
        }
        
    #endif
    }
}