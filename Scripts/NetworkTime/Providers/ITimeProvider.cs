// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinyUtilities.NetworkTime.Providers {
    internal interface ITimeProvider {
        public UniTask<TimeResult> GetTime(CancellationToken cancellation);
    }
}