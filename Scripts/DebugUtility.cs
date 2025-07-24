// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityDebug = UnityEngine.Debug;

namespace TinyUtilities {
    public static class DebugUtility {
        private static readonly Color _defaultColor;
        private static readonly float _defaultDuration;
        
        static DebugUtility() {
            _defaultColor = Color.red;
            _defaultDuration = 1f;
        }
        
        public static void LogErrorValue<T>(string name, T value) => UnityDebug.LogError(GetDebugValue(name, value));
        
        public static void LogErrorValue<T>(UnityObject context, string name, T value) => UnityDebug.LogError(GetDebugValue(name, value), context);
        
        public static void LogErrorValue<T>(params (string name, T value)[] values) => UnityDebug.LogError(GetDebugValue(values));
        
        public static void LogErrorValue<T>(UnityObject context, params (string name, T value)[] values) => UnityDebug.LogError(GetDebugValue(values), context);
        
        public static void DrawBounds(Bounds bounds) => DrawBounds(bounds, _defaultColor, _defaultDuration);
        
        public static void DrawBounds(Bounds bounds, Color color) => DrawBounds(bounds, color, _defaultDuration);
        
        public static void DrawBounds(Bounds bounds, Color color, float duration) {
            DrawBoundsSimple(bounds, color, duration);
            
            Vector3 position = bounds.center;
            Vector3 sizeHalf = bounds.size * 0.5f;
            
            Vector3 t1 = new Vector3(position.x - sizeHalf.x, position.y + sizeHalf.y, position.z - sizeHalf.z);
            Vector3 t2 = new Vector3(position.x - sizeHalf.x, position.y + sizeHalf.y, position.z + sizeHalf.z);
            Vector3 t3 = new Vector3(position.x + sizeHalf.x, position.y + sizeHalf.y, position.z + sizeHalf.z);
            Vector3 t4 = new Vector3(position.x + sizeHalf.x, position.y + sizeHalf.y, position.z - sizeHalf.z);
            
            Vector3 b1 = new Vector3(position.x - sizeHalf.x, position.y - sizeHalf.y, position.z - sizeHalf.z);
            Vector3 b2 = new Vector3(position.x - sizeHalf.x, position.y - sizeHalf.y, position.z + sizeHalf.z);
            Vector3 b3 = new Vector3(position.x + sizeHalf.x, position.y - sizeHalf.y, position.z + sizeHalf.z);
            Vector3 b4 = new Vector3(position.x + sizeHalf.x, position.y - sizeHalf.y, position.z - sizeHalf.z);
            
            UnityDebug.DrawLine(t1, t3, color, duration);
            UnityDebug.DrawLine(t2, t4, color, duration);
            
            UnityDebug.DrawLine(b1, b3, color, duration);
            UnityDebug.DrawLine(b2, b4, color, duration);
            
            UnityDebug.DrawLine(t1, b2, color, duration);
            UnityDebug.DrawLine(t2, b3, color, duration);
            UnityDebug.DrawLine(t3, b4, color, duration);
            UnityDebug.DrawLine(t4, b1, color, duration);
            
            UnityDebug.DrawLine(t1, b4, color, duration);
            UnityDebug.DrawLine(t2, b1, color, duration);
            UnityDebug.DrawLine(t3, b2, color, duration);
            UnityDebug.DrawLine(t4, b3, color, duration);
        }
        
        public static void DrawBoundsSimple(Bounds bounds) => DrawBoundsSimple(bounds, _defaultColor, _defaultDuration);
        
        public static void DrawBoundsSimple(Bounds bounds, Color color) => DrawBoundsSimple(bounds, color, _defaultDuration);
        
        public static void DrawBoundsSimple(Bounds bounds, Color color, float duration) {
            Vector3 position = bounds.center;
            Vector3 sizeHalf = bounds.size * 0.5f;
            
            Vector3 t1 = new Vector3(position.x - sizeHalf.x, position.y + sizeHalf.y, position.z - sizeHalf.z);
            Vector3 t2 = new Vector3(position.x - sizeHalf.x, position.y + sizeHalf.y, position.z + sizeHalf.z);
            Vector3 t3 = new Vector3(position.x + sizeHalf.x, position.y + sizeHalf.y, position.z + sizeHalf.z);
            Vector3 t4 = new Vector3(position.x + sizeHalf.x, position.y + sizeHalf.y, position.z - sizeHalf.z);
            
            Vector3 b1 = new Vector3(position.x - sizeHalf.x, position.y - sizeHalf.y, position.z - sizeHalf.z);
            Vector3 b2 = new Vector3(position.x - sizeHalf.x, position.y - sizeHalf.y, position.z + sizeHalf.z);
            Vector3 b3 = new Vector3(position.x + sizeHalf.x, position.y - sizeHalf.y, position.z + sizeHalf.z);
            Vector3 b4 = new Vector3(position.x + sizeHalf.x, position.y - sizeHalf.y, position.z - sizeHalf.z);
            
            UnityDebug.DrawLine(t1, t2, color, duration);
            UnityDebug.DrawLine(t2, t3, color, duration);
            UnityDebug.DrawLine(t3, t4, color, duration);
            UnityDebug.DrawLine(t4, t1, color, duration);
            
            UnityDebug.DrawLine(b1, b2, color, duration);
            UnityDebug.DrawLine(b2, b3, color, duration);
            UnityDebug.DrawLine(b3, b4, color, duration);
            UnityDebug.DrawLine(b4, b1, color, duration);
            
            UnityDebug.DrawLine(t1, b1, color, duration);
            UnityDebug.DrawLine(t2, b2, color, duration);
            UnityDebug.DrawLine(t3, b3, color, duration);
            UnityDebug.DrawLine(t4, b4, color, duration);
        }
        
        public static void DrawPath(Vector3[] points) => DrawPath(points, _defaultColor, _defaultDuration);
        
        public static void DrawPath(Vector3[] points, Color color) => DrawPath(points, color, _defaultDuration);
        
        public static void DrawPath(Vector3[] points, Color color, float duration) {
            for (int pointId = 0; pointId < points.Length; pointId++) {
                Vector3 pointPosition = points[pointId];
                
                int nextId = pointId + 1;
                
                if (nextId >= points.Length) {
                    continue;
                }
                
                Vector3 nextPosition = points[nextId];
                UnityDebug.DrawLine(pointPosition, nextPosition, color, duration);
            }
        }
        
        public static void LogErrorValue<T>(T value) => UnityDebug.LogError($"{value}");
        
        public static void NoteError(string location, string message) {
            UnityDebug.LogError($"<b><color=blue>{location}</color></b> - <color=white>{message}</color>");
        }
        
        public static void LogIf(string message, Func<bool> condition) {
            if (condition()) {
                UnityDebug.Log(message);
            }
        }
        
        public static void LogWarningIf(string message, Func<bool> condition) {
            if (condition()) {
                UnityDebug.LogWarning(message);
            }
        }
        
        public static void LogErrorIf(string message, Func<bool> condition) {
            if (condition()) {
                UnityDebug.LogError(message);
            }
        }
        
        public static void LogErrorMarker() {
            StackTrace stack = new StackTrace();
            
            if (stack.FrameCount > 1) {
                MethodBase method = stack.GetFrame(1).GetMethod();
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
                UnityDebug.LogError(builder.ToString());
            } else {
                UnityDebug.LogError("Marker");
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
    }
}