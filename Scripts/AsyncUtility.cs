using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TinyUtilities {
    public static class AsyncUtility {
        private static readonly float _aspect;
        
        private const float _MOVE_DISTANCE = 0.1f;
        
        static AsyncUtility() {
            _aspect = (Screen.width + Screen.height) / 2f;
        }
        
        public static async UniTask CallAfterFrame(Action callback, CancellationToken cancellation) {
            await UniTask.Yield(cancellation);
            callback();
        }
        
        public static async UniTask WaitFrames(int framesCount) {
            while (framesCount > 0) {
                await UniTask.Yield();
                framesCount--;
            }
        }
        
        public static async UniTask Delay(int millisecondsDelay, CancellationToken cancellationToken = default) {
            await UniTask.Delay(millisecondsDelay, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
        }
        
        public static async UniTask DelayUnscaled(int millisecondsDelay, CancellationToken cancellationToken = default) {
            await UniTask.Delay(millisecondsDelay, DelayType.UnscaledDeltaTime, PlayerLoopTiming.Update, cancellationToken);
        }
        
        public static async UniTask DelayRealtime(int millisecondsDelay, CancellationToken cancellationToken = default) {
            await UniTask.Delay(millisecondsDelay, DelayType.Realtime, PlayerLoopTiming.Update, cancellationToken);
        }
        
        public static async UniTask WaitAnyInput(Action action, CancellationToken cancellation) {
            Vector3 pressPosition = Vector3.zero;
            
            while (Application.isPlaying) {
                if (Input.GetMouseButtonDown(0)) {
                    pressPosition = Input.mousePosition;
                    break;
                }
                
                await UniTask.Yield(cancellation);
            }
            
            await UniTask.WhenAny(WaitMove(pressPosition, cancellation), WaitClick(cancellation));
            action.Invoke();
        }
        
        private static async UniTask WaitMove(Vector3 pressPosition, CancellationToken cancellation) {
            float moveDistance = _aspect * _MOVE_DISTANCE;
            
            while (Application.isPlaying) {
                if (Vector3.Distance(pressPosition, Input.mousePosition) > moveDistance) {
                    return;
                }
                
                await UniTask.Yield(cancellation);
            }
        }
        
        private static async UniTask WaitClick(CancellationToken cancellation) {
            while (Application.isPlaying) {
                if (Input.GetMouseButtonUp(0)) {
                    await UniTask.Yield(cancellation);
                    return;
                }
                
                await UniTask.Yield(cancellation);
            }
        }
    }
}