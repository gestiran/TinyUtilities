// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityRandom = UnityEngine.Random;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.Components {
    [DisallowMultipleComponent]
    public sealed class ButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        public bool isStop { get; private set; }
        
        [field: SerializeField, BoxGroup(InspectorNames.PARAMETERS), HideIf(nameof(customStartScale))]
        public bool notScaleReset { get; private set; }
        
        [field: SerializeField, BoxGroup(InspectorNames.PARAMETERS)]
        public float koef { get; private set; } = 0.9f;
        
        [field: SerializeField, BoxGroup(InspectorNames.PARAMETERS)]
        public Ease easeDown { get; private set; } = Ease.InQuart;
        
        [field: SerializeField, BoxGroup(InspectorNames.PARAMETERS)]
        public Ease easeUp { get; private set; } = Ease.OutElastic;
        
        [field: SerializeField, BoxGroup(InspectorNames.PARAMETERS)]
        public bool bounce { get; private set; }
        
        [field: SerializeField, BoxGroup(_BOUNCE_GROUP), ShowIf(nameof(bounce))]
        public float bounceKoef { get; private set; } = 1.1f;
        
        [field: SerializeField, BoxGroup(_BOUNCE_GROUP), ShowIf(nameof(bounce))]
        public Ease loopEase { get; private set; } = Ease.OutExpo;
        
        [field: SerializeField, BoxGroup(_BOUNCE_GROUP), ShowIf(nameof(bounce))]
        public bool randomTime { get; private set; }
        
        [field: SerializeField, BoxGroup(_BOUNCE_GROUP), ShowIf(nameof(bounce))]
        public float bounceTime { get; private set; } = 1f;
        
        [field: SerializeField, BoxGroup(InspectorNames.PARAMETERS)]
        public bool customStartScale { get; private set; }
        
        [field: SerializeField, BoxGroup(InspectorNames.PARAMETERS + "/StartScale"), ShowIf(nameof(customStartScale))]
        public Vector3 startScale { get; private set; } = Vector3.one;
        
        private const string _BOUNCE_GROUP = InspectorNames.PARAMETERS + "/Bounce";
        
        private void Awake() {
            if (customStartScale) {
                return;
            }
            
            startScale = transform.localScale;
        }
        
        private void OnEnable() {
            if (notScaleReset && !customStartScale) {
                startScale = transform.localScale;
            }
            
            if (randomTime) {
                bounceTime *= UnityRandom.Range(0.9f, 1.1f);
            }
            
            Bounce();
            isStop = false;
        }
        
        public void SetStartScale(Vector3 scale) {
            if (customStartScale) {
                return;
            }
            
            startScale = scale;
        }
        
        public void OnPointerDown(PointerEventData eventData) {
            DOTween.Complete(transform);
            transform.DOScale(startScale * koef, 0.1f).SetEase(easeDown).SetUpdate(true).Play();
        }
        
        public void OnPointerUp(PointerEventData eventData) {
            DOTween.Complete(transform);
            transform.DOScale(startScale, 0.3f).SetEase(easeUp).SetUpdate(true).OnComplete(Bounce).Play();
        }
        
        public void StartBounce() {
            DOTween.Complete(transform);
            isStop = false;
            bounce = true;
            Bounce();
        }
        
        public void Bounce() {
            if (bounce && !isStop) {
                DOTween.Complete(transform);
                transform.localScale = startScale;
                transform.DOScale(startScale * bounceKoef, bounceTime).SetEase(loopEase).SetUpdate(true).SetLoops(9999, LoopType.Yoyo).Play();
            }
        }
        
        public void Stop() {
            if (bounce) {
                DOTween.Complete(transform);
                isStop = true;
            }
        }
    }
}