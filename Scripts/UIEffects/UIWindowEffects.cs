// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace TinyUtilities.UIEffects {
    public static class UIWindowEffects {
        public static Tweener DoEffectWindowShow(this CanvasGroup canvasGroup) {
            return DOFade(canvasGroup, 1f, 0.25f).SetEase(Ease.Linear);
        }
        
        public static Tweener DoEffectWindowHide(this CanvasGroup canvasGroup) {
            return DOFade(canvasGroup, 0f, 0.25f).SetEase(Ease.Linear);
        }
        
        public static void DoEffectWindowShowForce(this CanvasGroup canvasGroup) {
            canvasGroup.alpha = 1f;
        }
        
        public static void DoEffectWindowHideForce(this CanvasGroup canvasGroup) {
            canvasGroup.alpha = 0f;
        }
        
        private static TweenerCore<float, float, FloatOptions> DOFade(CanvasGroup target, float endValue, float duration) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.alpha, x => target.alpha = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
    }
}