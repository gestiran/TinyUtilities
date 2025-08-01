﻿// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Threading.Tasks;

namespace TinyUtilities {
    public sealed class AsyncPause {
        private bool _isPause;
        
        public void Pause() => _isPause = true;
        
        public void UnPause() => _isPause = false;
        
        public async Task Waiting() {
            if (_isPause) {
                await Task.Yield();
            }
        }
    }
}