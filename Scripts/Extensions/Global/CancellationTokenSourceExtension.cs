using System.Threading;

namespace TinyUtilities.Extensions.Global {
    public static class CancellationTokenSourceExtension {
        public static CancellationTokenSource Create(this CancellationTokenSource cancellation) {
            if (cancellation == null) {
                return new CancellationTokenSource();;
            }
            
            return cancellation;
        }
        
        public static CancellationTokenSource Reset(this CancellationTokenSource cancellation) {
            if (cancellation == null) {
                return null;
            }
            
            CancelAndDispose(cancellation);
            return null;
        }
        
        public static CancellationTokenSource Recreate(this CancellationTokenSource cancellation) {
            if (cancellation == null) {
                return new CancellationTokenSource();
            }
            
            CancelAndDispose(cancellation);
            return new CancellationTokenSource();
        }
        
        public static void CancelAndDispose(this CancellationTokenSource cancellation) {
            cancellation.Cancel();
            cancellation.Dispose();
        }
    }
}