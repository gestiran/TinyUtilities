using DG.Tweening;
using UnityEngine;

namespace TinyUtilities.UIEffects {
    public static class UIWindowEffects {
        public static Tweener DoEffectWindowShow(this CanvasGroup canvasGroup) {
            return canvasGroup.DOFade(1f, 0.25f).SetEase(Ease.Linear);
        }
        
        public static Tweener DoEffectWindowHide(this CanvasGroup canvasGroup) {
            return canvasGroup.DOFade(0f, 0.25f).SetEase(Ease.Linear);
        }
        
        public static void DoEffectWindowShowForce(this CanvasGroup canvasGroup) {
            canvasGroup.alpha = 1f;
        }
        
        public static void DoEffectWindowHideForce(this CanvasGroup canvasGroup) {
            canvasGroup.alpha = 0f;
        }
    }
}