// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TinyUtilities.Components;

namespace TinyUtilities.Extensions {
    public static class DoTweenTripleSliderExtension {
        public static TweenerCore<float, float, FloatOptions> DoFirstValue(this TripleSlider target, float endValue, float duration, bool snapping = false) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.firstValue, x => target.firstValue = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions> DoSecondValue(this TripleSlider target, float endValue, float duration, bool snapping = false) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.secondValue, x => target.secondValue = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions> DoThirdValue(this TripleSlider target, float endValue, float duration, bool snapping = false) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.thirdValue, x => target.thirdValue = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
    }
}