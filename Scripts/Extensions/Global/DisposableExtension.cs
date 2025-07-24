// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Collections.Generic;

namespace TinyUtilities.Extensions.Global {
    public static class DisposableExtension {
        public static void Dispose<T>(this T[] disposables) where T : IDisposable {
            for (int disposeId = 0; disposeId < disposables.Length; disposeId++) {
                disposables[disposeId].Dispose();
            }
        }
        
        public static void Dispose<T>(this List<T> disposables) where T : IDisposable {
            for (int disposeId = 0; disposeId < disposables.Count; disposeId++) {
                disposables[disposeId].Dispose();
            }
        }
        
        public static void Dispose<T1, T2>(this Dictionary<T1, T2> disposables) where T2 : IDisposable {
            foreach (T2 disposable in disposables.Values) {
                disposable.Dispose();
            }
        }
    }
}