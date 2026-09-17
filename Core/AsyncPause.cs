// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Threading;
using System.Threading.Tasks;
using TinyUtilities.Logger;

namespace TinyUtilities {
    public sealed class AsyncPause {
        private bool _isPause;
        
        public void Pause() => _isPause = true;
        
        public void UnPause() => _isPause = false;
        
        public Task Waiting() => Waiting(CancellationToken.None);
        
        public async Task Waiting(CancellationToken cancellation) {
            try {
                while (_isPause) {
                    await Task.Delay(1, cancellation);
                }   
            } catch (TaskCanceledException) {
                DebugUtility.Log("AsyncPause.Waiting - Canceled.");
            }
        }
    }
}