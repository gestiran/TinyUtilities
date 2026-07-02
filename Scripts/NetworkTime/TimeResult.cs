// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;

namespace TinyUtilities.NetworkTime {
    internal sealed class TimeResult {
        public readonly DateTime time;
        public readonly bool isSuccess;
        
        public TimeResult(DateTime time, bool isSuccess) {
            this.time = time;
            this.isSuccess = isSuccess;
        }
    }
}