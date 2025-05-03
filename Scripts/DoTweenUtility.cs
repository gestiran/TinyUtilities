using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace TinyUtilities {
    public static class DoTweenUtility {
        public static TweenerCore<float, float, FloatOptions> ChangeFloat(Func<float> get, Action<float> set, float endValue, float duration, bool snapping = false) {
            GameObject target = new GameObject("TweenTarget");
            TweenerCore<float, float, FloatOptions> t = DOTween.To(get.Invoke, set.Invoke, endValue, duration);
            t.SetOptions(snapping).SetTarget(target);
            
            return t;
        }
    }
}