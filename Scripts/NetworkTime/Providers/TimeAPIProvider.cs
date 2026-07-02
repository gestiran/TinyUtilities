// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace TinyUtilities.NetworkTime.Providers {
    internal sealed class TimeAPIProvider : ITimeProvider {
        private const string _URL = "https://timeapi.io/api/time/current/zone?timeZone=UTC";
        
        [Serializable]
        private sealed class Response {
            public string dateTime;
            public string timeZone;
        }
        
        public async UniTask<TimeResult> GetTime(CancellationToken cancellation) {
            try {
                using UnityWebRequest webRequest = UnityWebRequest.Get(_URL);
                
                await webRequest.SendWebRequest().WithCancellation(cancellation);
                
                if (webRequest.result == UnityWebRequest.Result.Success) {
                    Response timeData = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
                    
                    if (timeData != null && string.IsNullOrEmpty(timeData.dateTime) == false && DateTime.TryParse(timeData.dateTime, out DateTime current)) {
                        return new TimeResult(current, true);
                    }
                }
            } catch (Exception exception) {
                Debug.LogWarning(new Exception("TimeAPIProvider.GetTime", exception));
            }
            
            return new TimeResult(DateTime.Now, false);
        }
    }
}