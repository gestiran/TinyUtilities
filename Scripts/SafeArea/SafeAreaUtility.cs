// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using TinyUtilities.ScreenOrientation;
using UnityEngine;

namespace TinyUtilities.SafeArea {
    public static class SafeAreaUtility {
        public static float topOffset => _topOffset;
        public static float bottomOffset => _bottomOffset;
        public static float leftOffset => _leftOffset;
        public static float rightOffset => _rightOffset;
        
        private static float _topOffset;
        private static float _bottomOffset;
        private static float _leftOffset;
        private static float _rightOffset;
        
        public static readonly bool isHave;
        
        [Flags]
        public enum Anchors : byte {
            None = 0,
            Top = 2,
            Left = 4,
            Right = 8,
            Bottom = 16
        }
        
        static SafeAreaUtility() {
            CalculateOffsets();
            isHave = Approximately(0, _topOffset, _bottomOffset, _leftOffset, _rightOffset) == false;
            ScreenOrientationUtility.onChange += CalculateOffsets;
        }
        
        public static Anchors CalculateAnchors(RectTransform rectTransform) {
            Anchors result = Anchors.None;
            
            Vector2 anchorMin = rectTransform.anchorMin;
            Vector2 anchorMax = rectTransform.anchorMax;
            
            if (Mathf.Approximately(anchorMin.y, 1f) && Mathf.Approximately(anchorMax.y, 1f)) {
                result |= Anchors.Top;
            } else if (Mathf.Approximately(anchorMin.y, 0f) && Mathf.Approximately(anchorMax.y, 0f)) {
                result |= Anchors.Bottom;
            }
            
            if (Mathf.Approximately(anchorMin.x, 1f) && Mathf.Approximately(anchorMax.x, 1f)) {
                result |= Anchors.Right;
            } else if (Mathf.Approximately(anchorMin.x, 0f) && Mathf.Approximately(anchorMax.x, 0f)) {
                result |= Anchors.Left;
            }
            
            return result;
        }
        
        public static Vector2 CalculateSoftOffset(RectTransform rectTransform, Anchors anchors) {
            Vector2 min = rectTransform.offsetMin;
            Vector2 max = rectTransform.offsetMax;
            
            Vector2 result = Vector2.zero;
            
            if (anchors.HasFlag(Anchors.Top)) {
                if (max.y > -topOffset) {
                    result.y += -topOffset - max.y;
                }
            } else if (anchors.HasFlag(Anchors.Bottom)) {
                if (min.y < bottomOffset) {
                    result.y += bottomOffset - min.y;
                }
            }
            
            if (anchors.HasFlag(Anchors.Left)) {
                if (min.x < leftOffset) {
                    result.x += leftOffset - min.x;
                }
            } else if (anchors.HasFlag(Anchors.Right)) {
                if (max.x > -rightOffset) {
                    result.x += -rightOffset - max.x;
                }
            }
            
            return result;
        }
        
        public static Vector2 CalculateFullOffset(Anchors anchors) {
            Vector2 result = Vector2.zero;
            
            if (anchors.HasFlag(Anchors.Top)) {
                result.y -= topOffset;
            } else if (anchors.HasFlag(Anchors.Bottom)) {
                result.y += bottomOffset;
            }
            
            if (anchors.HasFlag(Anchors.Left)) {
                result.x += leftOffset;
            } else if (anchors.HasFlag(Anchors.Right)) {
                result.x -= rightOffset;
            }
            
            return result;
        }
        
        public static Vector2 CalculateSize(Vector2 sizeDelta, Anchors anchors) {
            if (anchors.HasFlag(Anchors.Top)) {
                sizeDelta.y -= topOffset;
            }
            
            if (anchors.HasFlag(Anchors.Bottom)) {
                sizeDelta.y -= bottomOffset;
            }
            
            if (anchors.HasFlag(Anchors.Left)) {
                sizeDelta.x -= leftOffset;
            }
            
            if (anchors.HasFlag(Anchors.Right)) {
                sizeDelta.x -= rightOffset;
            }
            
            return sizeDelta;
        }
        
        private static void CalculateOffsets() {
            Rect screen = new Rect(0, 0, Screen.width, Screen.height);
            Rect safeArea = Screen.safeArea;
            
            _topOffset = screen.height - (safeArea.y + safeArea.height);
            _bottomOffset = screen.height - (_topOffset + safeArea.height);
            
            _leftOffset = screen.width - (safeArea.x + safeArea.width);
            _rightOffset = screen.width - (_leftOffset + safeArea.width);
        }
        
        private static bool Approximately(float value, params float[] values) {
            for (int i = 0; i < values.Length; i++) {
                if (Mathf.Approximately(value, values[i])) {
                    continue;
                }
                
                return false;
            }
            
            return true;
        }
    }
}