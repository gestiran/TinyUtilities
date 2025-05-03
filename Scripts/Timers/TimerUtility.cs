using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinyUtilities.Timers {
    public static class TimerUtility {
        private static readonly CancellationTokenSource _global;
        
        private const int _SECOND = 1000;
        
        static TimerUtility() => _global = new CancellationTokenSource();
        
        public static TimeSpan GetTimeToNextDay() {
            DateTime now = DateTime.Now;
            
            int hours = 23 - now.Hour;
            int minutes = 59 - now.Minute;
            int seconds = 59 - now.Second;
            
            return new TimeSpan(hours, minutes, seconds);
        }
        
        public static UniTask StartTimer(TimeSpan time, Action onComplete, int updateDelay) {
            return StartTimer(time, _ => { }, onComplete, _global.Token, true, updateDelay);
        }
        
        public static UniTask StartTimer(TimeSpan time, Action onComplete, CancellationToken cancellation, int updateDelay) {
            return StartTimer(time, _ => { }, onComplete, cancellation, true, updateDelay);
        }
        
        public static UniTask StartTimer(TimeSpan time, Action<TimeSpan> setTime, Action onComplete, int updateDelay) {
            return StartTimer(time, setTime, onComplete, _global.Token, true, updateDelay);
        }
        
        public static UniTask StartTimer(TimeSpan time, Action<TimeSpan> setTime, Action onComplete, CancellationToken cancellation, int updateDelay) {
            return StartTimer(time, setTime, onComplete, cancellation, true, updateDelay);
        }
        
        public static UniTask StartTimer(TimeSpan time, Action onComplete, bool ignoreTimeScale = true, int updateDelay = 1000) {
            return StartTimer(time, _ => { }, onComplete, _global.Token, ignoreTimeScale, updateDelay);
        }
        
        public static UniTask StartTimer(TimeSpan time, Action onComplete, CancellationToken cancellation, bool ignoreTimeScale = true, int updateDelay = 1000) {
            return StartTimer(time, _ => { }, onComplete, cancellation, ignoreTimeScale, updateDelay);
        }
        
        public static UniTask StartTimer(TimeSpan time, Action<TimeSpan> setTime, Action onComplete, bool ignoreTimeScale = true, int updateDelay = 1000) {
            return StartTimer(time, setTime, onComplete, _global.Token, ignoreTimeScale, updateDelay);
        }
        
        public static async UniTask StartTimer(TimeSpan time, Action<TimeSpan> setTime, Action onComplete, CancellationToken cancellation, bool ignoreTimeScale = true,
                                               int updateDelay = 1000) {
            TimeSpan delay = new TimeSpan(0, 0, 0, 0, updateDelay);
            
            do {
                setTime(time);
                await UniTask.Delay(updateDelay, ignoreTimeScale, PlayerLoopTiming.Update, cancellation);
                
                time = time.Subtract(delay);
            } while (time > TimeSpan.Zero);
            
            setTime(TimeSpan.Zero);
            onComplete();
        }
        
        public static async UniTask StartTimerUnscaled(TimeSpan time, Action<TimeSpan> setTime, Action onComplete, CancellationToken cancellation) {
            DateTime end = DateTime.Now.Add(time);
            
            do {
                setTime(time);
                await UniTask.Delay(_SECOND, true, PlayerLoopTiming.Update, cancellation);
                
                time = end.Subtract(DateTime.Now);
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