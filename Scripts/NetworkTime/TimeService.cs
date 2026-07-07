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
        public static bool isInitialized { get; private set; }
        
        private static DateTime _networkTime;
        private static float _startTime;
        private static bool _isProcess;
        
        private static readonly ITimeProvider[] _providers;
        
        static TimeService() {
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
            if (_isProcess || isInitialized) {
                return;
            }
            
            _isProcess = true;
            
            try {
                for (int providerId = 0; providerId < _providers.Length; providerId++) {
                    try {
                        TimeResult result = await _providers[providerId].GetTime(cancellation);
                        
                        if (result.isSuccess) {
                            Initialize(result.time);
                            break;
                        }
                    } catch (Exception exception) {
                        Debug.LogWarning(exception);
                    }
                }
            } finally {
                _isProcess = false;
            }
        }
        
        [Pure]
        public static async UniTask<DateTime> GetTime(CancellationToken cancellation) {
            DateTime networkTime;
            
            while (TryGetTime(out networkTime) == false) {
                await UniTask.Delay(1000, DelayType.UnscaledDeltaTime, PlayerLoopTiming.Update, cancellation);
            }
            
            return networkTime;
        }
        
        public static bool TryGetTime(out DateTime time) {
            if (isInitialized) {
                time = _networkTime.AddSeconds(Time.unscaledTime - _startTime);
                return true;
            }
            
            time = default;
            return false;
        }
        
        private static void Initialize(in DateTime time) {
            _networkTime = time.AddHours(LoadOffset(time));
            _startTime = Time.unscaledTime;
            isInitialized = true;
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