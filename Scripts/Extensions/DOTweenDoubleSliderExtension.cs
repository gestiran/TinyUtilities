using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TinyUtilities.Components;

namespace TinyUtilities.Extensions {
    public static class DoTweenDoubleSliderExtension {
        public static TweenerCore<float, float, FloatOptions> DoFirstValue(this DoubleSlider target, float endValue, float duration, bool snapping = false) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.firstValue, x => target.firstValue = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions> DoSecondValue(this DoubleSlider target, float endValue, float duration, bool snapping = false) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.secondValue, x => target.secondValue = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            return t;
        }
    }
}