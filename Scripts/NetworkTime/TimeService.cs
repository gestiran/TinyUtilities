// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics.Contracts;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinyUtilities.NetworkTime.Providers;
using UnityEngine;

namespace TinyUtilities.NetworkTime {
    public static class TimeService {
        public static bool isInitialized => _status is State.Success;
        
    #if UNITY_EDITOR
        public static float debugTimeScale;
    #endif
        
        private static DateTime _networkTime;
        private static float _startTime;
        private static float _lastConnectionTime;
        private static bool _lastConnectionStatus;
        private static State _status;
        
        private static readonly ITimeProvider[] _providers;
        
        private const float _CONNECT_CHECK_DELAY = 60f;
        
        private enum State : byte {
            None,
            Process,
            Success,
            Failed
        }
        
        static TimeService() {
        #if UNITY_EDITOR
            debugTimeScale = 1f;
        #endif
            
            _providers = new ITimeProvider[] {
                new GoogleHeaderTimeProvider(),
                new DuckDuckGoHeaderTimeProvider(),
                new CloudflareHeaderTimeProvider(),
                new AwsHeaderTimeProvider(),
                new TimeAPIProvider()
            };
        }
        
        public static UniTask Sync() => Sync(CancellationToken.None);
        
        public static async UniTask Sync(CancellationToken cancellation) {
            if (_status == State.Success) {
                Debug.LogWarning("TimeService.Sync - Already initialized!");
                return;
            }
            
            if (_status == State.Process) {
                Debug.LogWarning("TimeService.Sync - Operation is started!");
                return;
            }
            
            _status = State.Process;
            
            try {
                TimeResult result = await TryGetNetworkTime(cancellation);
                
                if (result.isSuccess) {
                    Initialize(result.time);
                }
            } finally {
                _status = State.Failed;
            }
        }
        
        [Pure]
        public static UniTask<bool> IsConnected() => IsConnected(CancellationToken.None);
        
        [Pure]
        public static async UniTask<bool> IsConnected(CancellationToken cancellation) {
            if (_status == State.Success) {
                if (Time.unscaledTime - _lastConnectionTime > _CONNECT_CHECK_DELAY) {
                    TimeResult _ = await TryGetNetworkTime(cancellation);
                }
            } else if (_status == State.None) {
                Debug.LogError("TimeService.IsConnected - Isn't initialized, use TimeService.Sync to start initialization!");
            }
            
            return _lastConnectionStatus;
        }
        
        [Pure]
        public static async UniTask<DateTime> GetTime(CancellationToken cancellation) {
            if (_status == State.Success) {
                DateTime networkTime;
                
                while (TryGetTime(out networkTime) == false) {
                    await UniTask.Delay(1000, DelayType.UnscaledDeltaTime, PlayerLoopTiming.Update, cancellation);
                }
                
                return networkTime;
            }
            
            if (_status == State.None) {
                Debug.LogError("TimeService.GetTime - Isn't initialized, use TimeService.Sync to start initialization!");
            }
            
            return default;
        }
        
        public static bool TryGetTime(out DateTime time) {
            if (_status == State.Success) {
                float current = Time.unscaledTime;
            #if UNITY_EDITOR
                current *= debugTimeScale;
            #endif
                time = _networkTime.AddSeconds(current - _startTime);
                return true;
            }
            
            if (_status == State.None) {
                Debug.LogError("TimeService.TryGetTime - Isn't initialized, use TimeService.Sync to start initialization!");
            }
            
            time = default;
            return false;
        }
        
        private static void Initialize(in DateTime time) {
            _networkTime = time.AddHours(LoadOffset(time));
            _startTime = Time.unscaledTime;
            _status = State.Success;
        }
        
        [Pure]
        private static async UniTask<TimeResult> TryGetNetworkTime(CancellationToken cancellation) {
            _lastConnectionTime = Time.unscaledTime;
            
            for (int providerId = 0; providerId < _providers.Length; providerId++) {
                try {
                    TimeResult result = await _providers[providerId].GetTime(cancellation);
                    
                    if (result.isSuccess) {
                        _lastConnectionStatus = true;
                        return result;
                    }
                } catch (Exception exception) {
                    Debug.LogWarning(exception);
                }
            }
            
            _lastConnectionStatus = false;
            return new TimeResult(default, false);
        }
        
        private static int LoadOffset(in DateTime time) {
            TimeServicePrefs prefs = new TimeServicePrefs();
            
            if (prefs.HasHoursOffset()) {
                return prefs.LoadHoursOffset();
            }
            
            int offset = (int)Math.Round(DateTime.Now.Subtract(time).TotalHours);
            prefs.SaveHoursOffset(offset);
            return offset;
        }
    }
}