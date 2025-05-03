using System.Threading;

namespace TinyUtilities.Extensions.Global {
    public static class CancellationTokenSourceExtension {
        public static CancellationTokenSource Reset(this CancellationTokenSource obj) {
            if (obj == null) {
                return null;
            }
            
            obj.Cancel();
            obj.Dispose();
            return null;
        }
        
        public static CancellationTokenSource Recreate(this CancellationTokenSource obj) {
            if (obj == null) {
                return new CancellationTokenSource();
            }
            
            obj.Cancel();
            obj.Dispose();
            return new CancellationTokenSource();
        }
    }
}