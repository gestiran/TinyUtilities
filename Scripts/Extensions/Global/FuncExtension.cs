using System;
using System.Collections.Generic;

namespace TinyUtilities.Extensions.Global {
    public static class FuncExtension {
        public static void InvokeAnySafe(this List<Func<bool>> actions, bool target = true) {
            if (actions.Count > 0) {
                Func<bool>[] temp = new Func<bool>[actions.Count];
                actions.CopyTo(temp, 0);
                InvokeAny(temp, target);
            }
        }
        
        public static void InvokeAnySafe(this Func<bool>[] actions, bool target = true) {
            if (actions.Length > 0) {
                Func<bool>[] temp = new Func<bool>[actions.Length];
                Array.Copy(actions, temp, actions.Length);
                InvokeAny(temp, target);
            }
        }
        
        public static void InvokeAny(this List<Func<bool>> actions, bool target = true) {
            foreach (Func<bool> action in actions) {
                if (action.Invoke() == target) {
                    break;
                }
            }
        }
        
        public static void InvokeAny(this Func<bool>[] actions, bool target = true) {
            foreach (Func<bool> action in actions) {
                if (action.Invoke() == target) {
                    break;
                }
            }
        }
    }
}