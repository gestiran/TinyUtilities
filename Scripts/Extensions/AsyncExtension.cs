// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using Cysharp.Threading.Tasks;

namespace TinyUtilities.Extensions {
    public static class AsyncExtension {
        public static async UniTask OnComplete<T>(this UniTask<T> task, Action<T> callback) {
            callback(await task);
        }
    }
}