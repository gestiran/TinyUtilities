// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Threading;

namespace TinyUtilities.Extensions.Global {
    public static class CancellationTokenSourceExtension {
        public static CancellationTokenSource Create(this CancellationTokenSource cancellation) {
            if (cancellation == null) {
                return new CancellationTokenSource();
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
        
        public static void ResetForce(this CancellationTokenSource cancellation) {
            if (cancellation == null) {
                return;
            }
            
            CancelAndDispose(cancellation);
        }
        
        public static void Recreate(this CancellationTokenSource cancellation, out CancellationTokenSource result) {
            if (cancellation == null) {
                result = new CancellationTokenSource();
            }
            
            CancelAndDispose(cancellation);
            result = new CancellationTokenSource();
        }
        
        public static void Recreate(this CancellationTokenSource cancellation, CancellationTokenSource reference, out CancellationTokenSource result) {
            if (cancellation == null) {
                result = reference;
            }
            
            CancelAndDispose(cancellation);
            result = reference;
        }
        
        public static CancellationTokenSource Recreate(this CancellationTokenSource cancellation, CancellationTokenSource reference) {
            if (cancellation == null) {
                return reference;
            }
            
            CancelAndDispose(cancellation);
            return reference;
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