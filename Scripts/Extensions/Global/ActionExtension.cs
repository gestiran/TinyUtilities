using System;
using System.Collections.Generic;

namespace TinyUtilities.Extensions.Global {
    public static class ActionExtension {
        public static void InvokeSafe(this List<Action> actions) {
            if (actions.Count > 0) {
                Action[] temp = new Action[actions.Count];
                actions.CopyTo(temp);
                Invoke(temp);   
            }
        }
        
        public static void InvokeSafe(this Action[] actions) {
            if (actions.Length > 0) {
                Action[] temp = new Action[actions.Length];
                Array.Copy(actions, temp, actions.Length);
                Invoke(temp);
            }
        }
        
        public static void Invoke(this List<Action> actions) {
            foreach (Action action in actions) {
                action.Invoke();
            }
        }
        
        public static void Invoke(this Action[] actions) {
            foreach (Action action in actions) {
                action.Invoke();
            }
        }
        
        [Obsolete("Can`t use without parameters!", true)]
        public static void InvokeSafe<T>(this List<Action<T>> actions) { }
        
        [Obsolete("Can`t use without parameters!", true)]
        public static void InvokeSafe<T>(this Action<T>[] actions) { }
        
        public static void InvokeSafe<T>(this List<Action<T>> actions, params T[] values) {
            if (actions.Count > 0) {
                Action<T>[] temp = new Action<T>[actions.Count];
                actions.CopyTo(temp);
                Invoke(temp, values);
            }
        }
        
        public static void InvokeSafe<T>(this Action<T>[] actions, params T[] values) {
            if (actions.Length > 0) {
                Action<T>[] temp = new Action<T>[actions.Length];
                Array.Copy(actions, temp, actions.Length);
                Invoke(temp, values);
            }
        }
        
        [Obsolete("Can`t use without parameters!", true)]
        public static void Invoke<T>(this List<Action<T>> actions) { }
        
        [Obsolete("Can`t use without parameters!", true)]
        public static void Invoke<T>(this Action<T>[] actions) { }
        
        public static void Invoke<T>(this List<Action<T>> actions, params T[] values) {
            foreach (T value in values) {
                foreach (Action<T> action in actions) {
                    action.Invoke(value);
                }
            }
        }
        
        public static void Invoke<T>(this Action<T>[] actions, params T[] values) {
            foreach (T value in values) {
                foreach (Action<T> action in actions) {
                    action.Invoke(value);
                }
            }
        }
        
        public static void InvokeSafe<T>(this List<Action<T>> actions, T value) {
            if (actions.Count > 0) {
                Action<T>[] temp = new Action<T>[actions.Count];
                actions.CopyTo(temp);
                Invoke(temp, value);
            }
        }
        
        public static void InvokeSafe<T>(this Action<T>[] actions, T value) {
            if (actions.Length > 0) {
                Action<T>[] temp = new Action<T>[actions.Length];
                Array.Copy(actions, temp, actions.Length);
                Invoke(temp, value);
            }
        }
        
        public static void Invoke<T>(this List<Action<T>> actions, T value) {
            foreach (Action<T> action in actions) {
                action.Invoke(value);
            }
        }
        
        public static void Invoke<T>(this Action<T>[] actions, T value) {
            foreach (Action<T> action in actions) {
                action.Invoke(value);
            }
        }
        
        public static void InvokeSafe<T1, T2>(this List<Action<T1, T2>> actions, T1 value1, T2 value2) {
            if (actions.Count > 0) {
                Action<T1, T2>[] temp = new Action<T1, T2>[actions.Count];
                actions.CopyTo(temp);
                Invoke(temp, value1, value2);
            }
        }
        
        public static void InvokeSafe<T1, T2>(this Action<T1, T2>[] actions, T1 value1, T2 value2) {
            if (actions.Length > 0) {
                Action<T1, T2>[] temp = new Action<T1, T2>[actions.Length];
                Array.Copy(actions, temp, actions.Length);
                Invoke(temp, value1, value2);
            }
        }
        
        public static void Invoke<T1, T2>(this List<Action<T1, T2>> actions, T1 value1, T2 value2) {
            foreach (Action<T1, T2> action in actions) {
                action.Invoke(value1, value2);
            }
        }
        
        public static void Invoke<T1, T2>(this Action<T1, T2>[] actions, T1 value1, T2 value2) {
            foreach (Action<T1, T2> action in actions) {
                action.Invoke(value1 , value2);
            }
        }
        
        public static void InvokeSafe<T1, T2, T3>(this List<Action<T1, T2, T3>> actions, T1 value1, T2 value2, T3 value3) {
            if (actions.Count > 0) {
                Action<T1, T2, T3>[] temp = new Action<T1, T2, T3>[actions.Count];
                actions.CopyTo(temp);
                Invoke(temp, value1, value2, value3);
            }
        }
        
        public static void InvokeSafe<T1, T2, T3>(this Action<T1, T2, T3>[] actions, T1 value1, T2 value2, T3 value3) {
            if (actions.Length > 0) {
                Action<T1, T2, T3>[] temp = new Action<T1, T2, T3>[actions.Length];
                Array.Copy(actions, temp, actions.Length);
                Invoke(temp, value1, value2, value3);
            }
        }
        
        public static void Invoke<T1, T2, T3>(this List<Action<T1, T2, T3>> actions, T1 value1, T2 value2, T3 value3) {
            foreach (Action<T1, T2, T3> action in actions) {
                action.Invoke(value1, value2, value3);
            }
        }
        
        public static void Invoke<T1, T2, T3>(this Action<T1, T2, T3>[] actions, T1 value1, T2 value2, T3 value3) {
            foreach (Action<T1, T2, T3> action in actions) {
                action.Invoke(value1 , value2, value3);
            }
        }
    }
}