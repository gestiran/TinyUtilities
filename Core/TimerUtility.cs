// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if EXTERNAL_DEPENDENCIES
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TinyUtilities {
    public static class TimerUtility {
        private const int _SECOND = 1000;
        
        public static TimeSpan GetTimeToNextDay() {
            DateTime now = DateTime.Now;
            
            int hours = 23 - now.Hour;
            int minutes = 59 - now.Minute;
            int seconds = 59 - now.Second;
            
            return new TimeSpan(hours, minutes, seconds);
        }
        
        public static Task StartTimer(TimeSpan time, Action onComplete, int updateDelay) {
            return StartTimer(time, _ => { }, onComplete, updateDelay, CancellationToken.None);
        }
        
        public static Task StartTimer(TimeSpan time, Action onComplete) {
            return StartTimer(time, _ => { }, onComplete, _SECOND, CancellationToken.None);
        }
        
        public static Task StartTimer(TimeSpan time, Action<TimeSpan> setTime, int updateDelay) {
            return StartTimer(time, setTime, () => { }, updateDelay, CancellationToken.None);
        }
        
        public static Task StartTimer(TimeSpan time, Action<TimeSpan> setTime) {
            return StartTimer(time, setTime, () => { }, _SECOND, CancellationToken.None);
        }
        
        public static Task StartTimer(TimeSpan time, Action<TimeSpan> setTime, Action onComplete) {
            return StartTimer(time, setTime, onComplete, _SECOND, CancellationToken.None);
        }
        
        public static Task StartTimer(TimeSpan time, Action onComplete, int updateDelay, CancellationToken cancellation) {
            return StartTimer(time, _ => { }, onComplete, updateDelay, cancellation);
        }
        
        public static Task StartTimer(TimeSpan time, Action onComplete, CancellationToken cancellation) {
            return StartTimer(time, _ => { }, onComplete, _SECOND, cancellation);
        }
        
        public static Task StartTimer(TimeSpan time, Action<TimeSpan> setTime, int updateDelay, CancellationToken cancellation) {
            return StartTimer(time, setTime, () => { }, updateDelay, cancellation);
        }
        
        public static Task StartTimer(TimeSpan time, Action<TimeSpan> setTime, CancellationToken cancellation) {
            return StartTimer(time, setTime, () => { }, _SECOND, cancellation);
        }
        
        public static Task StartTimer(TimeSpan time, Action<TimeSpan> setTime, Action onComplete, CancellationToken cancellation) {
            return StartTimer(time, setTime, onComplete, _SECOND, cancellation);
        }
        
        public static async Task StartTimer(TimeSpan time, Action<TimeSpan> setTime, Action onComplete, int updateDelay, CancellationToken cancellation) {
            TimeSpan delay = new TimeSpan(0, 0, 0, 0, updateDelay);
            
            do {
                setTime(time);
                await Task.Delay(updateDelay, cancellation);
                
                time = time.Subtract(delay);
            } while (time > TimeSpan.Zero);
            
            setTime(TimeSpan.Zero);
            onComplete();
        }
        
        public static TimeSpan CalculateRemainingTime(DateTime from, DateTime now, TimeSpan delay) => CalculateRemainingTime(from, now, delay, out _);
        
        public static TimeSpan CalculateRemainingTime(DateTime from, DateTime now, TimeSpan delay, out DateTime endTime) {
            TimeSpan passedTime = now.Subtract(from);
            endTime = from.Add(delay);
            
            if (passedTime > delay) {
                return TimeSpan.Zero;
            }
            
            return delay.Subtract(passedTime);
        }
        
        public static int CalculateOfflineTicks(ref DateTime from, DateTime now, TimeSpan delay, int limit, out TimeSpan remainingTime) {
            remainingTime = CalculateRemainingTime(from, now, delay, out DateTime nextOpenTime);
            int currentTicks;
            
            for (currentTicks = 0; currentTicks < limit; currentTicks++) {
                if (remainingTime.TotalSeconds <= 0) {
                    from = nextOpenTime;
                    remainingTime = CalculateRemainingTime(from, now, delay, out nextOpenTime);
                } else {
                    break;
                }
            }
            
            return currentTicks;
        }
    }
}
#endif