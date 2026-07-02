// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TinyUtilities {
    public static class AsyncUtility {
        public static CancellationToken token => _cancellation.Token;
        
        private static CancellationTokenSource _cancellation;
        
        private static readonly float _aspect;
        
        private const float _MOVE_DISTANCE = 0.1f;
        
        static AsyncUtility() {
            _aspect = (Screen.width + Screen.height) / 2f;
            _cancellation = new CancellationTokenSource();
        }
        
        public static UniTask Run<T>(List<T> arr, Action<T> action, int iterations, int delay, DelayType delayType) {
            return Run(arr, action, iterations, delay, delayType, CancellationToken.None);
        }
        
        public static async UniTask Run<T>(List<T> arr, Action<T> action, int iterations, int delay, DelayType delayType, CancellationToken cancellation) {
            for (int i = 0; i < iterations; i++) {
                Run(arr, iterations, i, action);
                await UniTask.Delay(delay, delayType, PlayerLoopTiming.Update, cancellation);
            }
        }
        
        public static UniTask CallAfterFrame(Action callback) => CallAfterFrame(callback, CancellationToken.None);
        
        public static async UniTask CallAfterFrame(Action callback, CancellationToken cancellation) {
            await UniTask.Yield(cancellation);
            callback();
        }
        
        public static UniTask WaitFrames(int framesCount) => WaitFrames(framesCount, CancellationToken.None);
        
        public static async UniTask WaitFrames(int framesCount, CancellationToken cancellation) {
            while (framesCount > 0) {
                await UniTask.Yield(cancellation);
                framesCount--;
            }
        }
        
        public static UniTask Delay(int millisecondsDelay) => Delay(millisecondsDelay, CancellationToken.None);
        
        public static async UniTask Delay(int millisecondsDelay, CancellationToken cancellationToken) {
            await UniTask.Delay(millisecondsDelay, DelayType.DeltaTime, PlayerLoopTiming.Update, cancellationToken);
        }
        
        public static UniTask DelayUnscaled(int millisecondsDelay) => DelayUnscaled(millisecondsDelay, CancellationToken.None);
        
        public static async UniTask DelayUnscaled(int millisecondsDelay, CancellationToken cancellationToken) {
            await UniTask.Delay(millisecondsDelay, DelayType.UnscaledDeltaTime, PlayerLoopTiming.Update, cancellationToken);
        }
        
        public static UniTask DelayRealtime(int millisecondsDelay) => DelayRealtime(millisecondsDelay, CancellationToken.None);
        
        public static async UniTask DelayRealtime(int millisecondsDelay, CancellationToken cancellationToken) {
            await UniTask.Delay(millisecondsDelay, DelayType.Realtime, PlayerLoopTiming.Update, cancellationToken);
        }
        
        public static UniTask WaitAnyInput(Action action) => WaitAnyInput(action, CancellationToken.None);
        
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
        
        public static UniTask WaitMove(Vector3 pressPosition) => WaitMove(pressPosition, CancellationToken.None);
        
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
        
        public static void Run<T>(List<T> arr, int count, int id, Action<T> action) {
            int total = arr.Count;
            int baseSize = total / count;
            int remainder = total % count;
            
            int start = id * baseSize + Mathf.Min(id, remainder);
            int end = start + baseSize + (id < remainder ? 1 : 0);
            
            for (int i = start; i < end; i++) {
                action(arr[i]);
            }
        }
        
    #if UNITY_EDITOR
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset() => _cancellation = new CancellationTokenSource();
        
    #endif
    }
}