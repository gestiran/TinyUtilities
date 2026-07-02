using System;
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace TinyUtilities.NetworkTime.Providers {
    internal sealed class DuckDuckGoHeaderTimeProvider : ITimeProvider {
        private const string _URL = "https://duckduckgo.com";
        
        public async UniTask<TimeResult> GetTime(CancellationToken cancellation) {
            try {
                using UnityWebRequest request = UnityWebRequest.Head(_URL);
                
                await request.SendWebRequest().WithCancellation(cancellation);
                
                if (request.result == UnityWebRequest.Result.Success) {
                    string dateHeader = request.GetResponseHeader("date");
                    
                    if (string.IsNullOrEmpty(dateHeader) == false) {
                        if (DateTimeOffset.TryParseExact(dateHeader, "r", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset current)) {
                            return new TimeResult(current.DateTime, true);
                        }
                    }
                }
            } catch (Exception exception) {
                Debug.LogWarning(new Exception("DuckDuckGoHeaderProvider.GetTime", exception));
            }
            
            return new TimeResult(DateTime.Now, false);
        }
    }
}