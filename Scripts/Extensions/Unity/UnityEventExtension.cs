using UnityEngine;
using UnityEngine.Events;

namespace TinyUtilities.Extensions.Unity {
    public static class UnityEventExtension {
        public static bool IsStartCurrent<T>(this UnityEvent targetEvent, T current, string methodName, bool excludeCallStateOff = true) where T : Object {
            int currentId = current.GetInstanceID();
            int eventsCount = targetEvent.GetPersistentEventCount();
            
            for (int eventId = 0; eventId < eventsCount; eventId++) {
                if (excludeCallStateOff && targetEvent.GetPersistentListenerState(eventId) == UnityEventCallState.Off) {
                    continue;
                }
                
                Object targetObject = targetEvent.GetPersistentTarget(eventId);
                
                if (targetObject is T target && target.GetInstanceID() == currentId) {
                    return targetEvent.GetPersistentMethodName(eventId) == methodName;
                }
            }
            
            return false;
        }
    }
}