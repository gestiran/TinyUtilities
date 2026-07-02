using System;
using System.Globalization;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace TinyUtilities.NetworkTime.Providers {
    internal sealed class CloudflareHeaderTimeProvider : ITimeProvider {
        private const string _URL = "https://www.cloudflare.com";
        private const string _USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        
        public async UniTask<TimeResult> GetTime(CancellationToken cancellation) {
            try {
                using UnityWebRequest request = UnityWebRequest.Head(_URL);
                
                request.SetRequestHeader("User-Agent", _USER_AGENT);
                
                await request.SendWebRequest().WithCancellation(cancellation);
                
                if (request.result == UnityWebRequest.Result.Success) {
                    string dateHeader = request.GetResponseHeader("date");
                    
                    if (string.IsNullOrEmpty(dateHeader) == false) {
                        if (DateTimeOffset.TryParseExact(dateHeader, "r", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset current)) {
                            return new TimeResult(current.DateTime, true);
                        }
                    }
                } else {
                    throw new Exception($"Cloudflare Time Error: {request.responseCode} - {request.error}");
                }
            } catch (Exception exception) {
                Debug.LogWarning(new Exception("CloudflareHeaderProvider.GetTime", exception));
            }
            
            return new TimeResult(DateTime.Now, false);
        }
    }
}