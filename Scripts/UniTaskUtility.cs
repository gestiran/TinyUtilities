using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinyUtilities {
    public sealed class UniTaskUtility {
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
    }
}