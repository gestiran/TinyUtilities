// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Diagnostics.Contracts;
using System.Threading;

namespace TinyUtilities.Extensions.Global {
    public static class CancellationTokenSourceExtension {
        [Pure]
        public static CancellationTokenSource Create(this CancellationTokenSource cancellation) {
            if (cancellation == null) {
                return new CancellationTokenSource();
            }
            
            return cancellation;
        }
        
        [Pure]
        public static CancellationTokenSource Create(this CancellationTokenSource cancellation, params CancellationToken[] tokens) {
            if (cancellation == null) {
                return CancellationTokenSource.CreateLinkedTokenSource(tokens);
            }
            
            return cancellation;
        }
        
        [Pure]
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
            if (cancellation != null) {
                CancelAndDispose(cancellation);
                
            }
            
            result = new CancellationTokenSource();
        }
        
        public static void Recreate(this CancellationTokenSource cancellation, out CancellationTokenSource result, params CancellationToken[] tokens) {
            if (cancellation != null) {
                CancelAndDispose(cancellation);
            }
            
            result = CancellationTokenSource.CreateLinkedTokenSource(tokens);
        }
        
        public static void Update(this CancellationTokenSource cancellation, out CancellationTokenSource result, CancellationTokenSource reference) {
            if (cancellation != null) {
                CancelAndDispose(cancellation);
            }
            
            result = reference;
        }
        
        [Pure]
        public static CancellationTokenSource Update(this CancellationTokenSource cancellation, CancellationTokenSource reference) {
            if (cancellation != null) {
                CancelAndDispose(cancellation);
            }
            
            return reference;
        }
        
        [Pure]
        public static CancellationTokenSource Recreate(this CancellationTokenSource cancellation) {
            if (cancellation != null) {
                CancelAndDispose(cancellation);
            }
            
            return new CancellationTokenSource();
        }
        
        [Pure]
        public static CancellationTokenSource Recreate(this CancellationTokenSource cancellation, params CancellationToken[] tokens) {
            if (cancellation != null) {
                CancelAndDispose(cancellation);
            }
            
            return CancellationTokenSource.CreateLinkedTokenSource(tokens);
        }
        
        public static void CancelAndDispose(this CancellationTokenSource cancellation) {
            cancellation.Cancel();
            cancellation.Dispose();
        }
    }
}