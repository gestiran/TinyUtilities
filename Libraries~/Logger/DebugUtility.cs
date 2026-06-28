// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

#if EXTERNAL_DEPENDENCIES
using System.Text.Json;
#endif

namespace TinyUtilities.Logger {
    public static class DebugUtility {
        public static event Action<string, LogType> onMessageUpdate;
        
    #if EXTERNAL_DEPENDENCIES
        public static readonly JsonSerializerOptions options;
    #endif
        
        private const string _DIVIDER = "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -";
        
        static DebugUtility() {
        #if EXTERNAL_DEPENDENCIES
            options = new JsonSerializerOptions();
            options.WriteIndented = true;
        #endif
        }
        
    #if EXTERNAL_DEPENDENCIES
        public static string ToJson<T>(T obj) => JsonSerializer.Serialize(obj, options);
    #endif
        
        [Conditional("DEBUG")]
        public static void LogDivider() => Log(_DIVIDER);
        
        [Conditional("DEBUG")]
        public static void LogWarningDivider() => LogWarning(_DIVIDER);
        
        [Conditional("DEBUG")]
        public static void LogErrorDivider() => LogError(_DIVIDER);
        
        [Conditional("DEBUG")]
        public static void Log<T>(T exception) where T : Exception => Log(exception.ToString());
        
        [Conditional("DEBUG")]
        public static void LogWarning<T>(T exception) where T : Exception => LogWarning(exception.ToString());
        
        [Conditional("DEBUG")]
        public static void LogError<T>(T exception) where T : Exception {
        #if UNIT_TEST
            throw exception;
        #else
            LogError(exception.ToString());
        #endif
        }
        
        [Conditional("DEBUG")]
        public static void LogException<T>(T exception) where T : Exception => LogError(exception.ToString());
        
        [Conditional("DEBUG")]
        public static void Log(string message) => Write(message, LogType.Log);
        
        [Conditional("DEBUG")]
        public static void LogWarning(string message) => Write(message, LogType.Warning);
        
        [Conditional("DEBUG")]
        public static void LogError(string message) => Write(message, LogType.Error);
        
        [Conditional("DEBUG")]
        public static void LogErrorValue<T>(string name, T value) => LogError(GetDebugValue(name, value));
        
        [Conditional("DEBUG")]
        public static void LogErrorValue<T>(params (string name, T value)[] values) => LogError(GetDebugValue(values));
        
        [Conditional("DEBUG")]
        public static void LogErrorValue<T>(T value) => LogError($"{value}");
        
        [Conditional("DEBUG")]
        public static void NoteError(string location, string message) {
            LogError($"<b><color=blue>{location}</color></b> - <color=white>{message}</color>");
        }
        
        [Conditional("DEBUG")]
        public static void LogIf(string message, Func<bool> condition) {
            if (condition()) {
                Log(message);
            }
        }
        
        [Conditional("DEBUG")]
        public static void LogWarningIf(string message, Func<bool> condition) {
            if (condition()) {
                LogWarning(message);
            }
        }
        
        [Conditional("DEBUG")]
        public static void LogErrorIf(string message, Func<bool> condition) {
            if (condition()) {
                LogError(message);
            }
        }
        
        [Conditional("DEBUG")]
        public static void LogErrorMarker() {
            StackTrace stack = new StackTrace();
            
            if (stack.FrameCount > 1) {
                StackFrame frame = stack.GetFrame(1);
                
                if (frame != null) {
                    MethodBase method = frame.GetMethod();
                    
                    if (method != null) {
                        ParameterInfo[] parameters = method.GetParameters();
                        
                        StringBuilder builder = new StringBuilder();
                        
                        if (method.DeclaringType != null) {
                            builder.AppendFormat("{0}.", method.DeclaringType.Name);
                        }
                        
                        builder.AppendFormat("{0}{1}", method.Name, "(");
                        
                        for (int parameterId = 0; parameterId < parameters.Length; parameterId++) {
                            builder.AppendFormat("{0} {1}", parameters[parameterId].ParameterType.Name, parameters[parameterId].Name);
                            
                            if (parameterId < parameters.Length - 1) {
                                builder.Append(",\t");
                            }
                        }
                        
                        builder.Append(");");
                        LogError(builder.ToString());
                    }
                }
            } else {
                LogError("Marker");
            }
        }
        
        private static string GetDebugValue<T>(params (string name, T value)[] values) {
            StringBuilder builder = new StringBuilder();
            
            for (int i = 0; i < values.Length; i++) {
                builder.Append(GetDebugValue($"{values[i].name}", values[i].value));
                
                if (i < values.Length - 1) {
                    builder.Append(",\t");
                }
            }
            
            return builder.ToString();
        }
        
        private static string GetDebugValue<T>(string name, T value) => $"{name}: {value}";
        
        private static void Write(string message, LogType type) {
            
        #if APP_DESKTOP
            ConsoleColor current = Console.ForegroundColor;
            
            Console.ForegroundColor = type.ToColor();
            Console.WriteLine(message);
            Console.ForegroundColor = current;
        #elif APP_MOBILE
        #endif
            
            Debug.WriteLine(message, type.ToString());
            onMessageUpdate?.Invoke(message, type);
        }
        
    #if APP_DESKTOP
        private static ConsoleColor ToColor(this in LogType type) {
            switch (type) {
                case LogType.Log: return Console.ForegroundColor;
                case LogType.Warning: return ConsoleColor.Yellow;
                case LogType.Error: return ConsoleColor.Red;
                default: return Console.ForegroundColor;
            }
        }
    #endif
    }
}