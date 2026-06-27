// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if UNITY_ENGINE
using System.Diagnostics.Contracts;
using UnityEngine.Events;
using UnityObject = UnityEngine.Object;

namespace TinyUtilities.Extensions.Unity {
    public static class UnityEventExtension {
        [Pure]
        public static bool IsStartCurrent<T>(this UnityEvent targetEvent, T current, string methodName, bool excludeCallStateOff = true) where T : UnityObject {
            int currentId = current.GetInstanceID();
            int eventsCount = targetEvent.GetPersistentEventCount();
            
            for (int eventId = 0; eventId < eventsCount; eventId++) {
                if (excludeCallStateOff && targetEvent.GetPersistentListenerState(eventId) == UnityEventCallState.Off) {
                    continue;
                }
                
                UnityObject targetObject = targetEvent.GetPersistentTarget(eventId);
                
                if (targetObject is T target && target.GetInstanceID() == currentId) {
                    return targetEvent.GetPersistentMethodName(eventId) == methodName;
                }
            }
            
            return false;
        }
    }
}
#endif