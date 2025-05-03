using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TinyUtilities.Components;
using UnityEngine;
using UnityEngine.UI;

namespace TinyUtilities {
    public static class DoTweenExtension {
        public static TweenerCore<float, float, FloatOptions> DOMaxValue(this Slider target, float endValue, float duration, bool snapping = false) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.maxValue, x => target.maxValue = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions> DOVolume(this AudioSource target, float endValue, float duration, bool snapping = false) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.volume, x => target.volume = x, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions> DOSetFloat(this Animator target, int id, float endValue, float duration, bool snapping = false) {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.GetFloat(id), x => target.SetFloat(id, x), endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            
            return t;
        }
        
        public static TweenerCore<Color, Color, ColorOptions> DOColor(this ColorRendererAnimation target, Color endValue, float duration, bool snapping = false) {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.To(target.GetColor, target.SetColor, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            
            return t;
        }
    }
}