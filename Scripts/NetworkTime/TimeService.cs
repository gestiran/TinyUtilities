using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinyUtilities.NetworkTime.Providers;

namespace TinyUtilities.NetworkTime {
    public static class TimeService {
        private static DateTime _time;
        private static bool _isInitialized;
        private static bool _isProcess;
        
        private static readonly ITimeProvider[] _providers;
        
        static TimeService() {
            _providers = new ITimeProvider[] {
                new TimeAPIProvider(),
                new GoogleHeaderTimeProvider(),
                new DuckDuckGoHeaderTimeProvider(),
                new CloudflareHeaderTimeProvider(),
                new AwsHeaderTimeProvider()
            };
        }
        
        public static UniTask Sync() => Sync(CancellationToken.None);
        
        public static async UniTask Sync(CancellationToken cancellation) {
            if (_isProcess) {
                return;
            }
            
            _isProcess = true;
            
            try {
                for (int providerId = 0; providerId < _providers.Length; providerId++) {
                    TimeResult result = await _providers[providerId].GetTime(cancellation);
                    
                    if (result.isSuccess) {
                        _time = result.time;
                        _isInitialized = true;
                    }
                }
            } finally {
                _isProcess = false;
            }
        }
        
        public static bool TryGetTime(out DateTime time) {
            time = _time;
            return _isInitialized;
        }
    }
}