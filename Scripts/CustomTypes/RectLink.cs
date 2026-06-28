// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace TinyUtilities.CustomTypes {
    [Serializable, InlineProperty]
    public sealed class RectLink : IEquatable<RectLink>, IEquatable<Rect> {
        public Rect rect => new Rect(_x, _y, _width, _height);
        
        [SerializeField, HorizontalGroup, HideLabel, SuffixLabel("X", true)]
        private float _x;
        
        [SerializeField, HorizontalGroup, HideLabel, SuffixLabel("Y", true)]
        private float _y;
        
        [SerializeField, HorizontalGroup, HideLabel, SuffixLabel("Width", true)]
        private float _width;
        
        [SerializeField, HorizontalGroup, HideLabel, SuffixLabel("Height", true)]
        private float _height;
        
        public RectLink() { }
        
        public RectLink(Rect rect) {
            _x = rect.x;
            _y = rect.y;
            _width = rect.width;
            _height = rect.height;
        }
        
        public RectLink(float x, float y, float width, float height) {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }
        
        public static implicit operator Rect(RectLink value) => value.rect;
        
        public override string ToString() => $"Rect(x: {_x}, y: {_y}, width: {_width}, height: {_height})";
        
        public bool Equals(RectLink other) {
            return other != null && other._x.Equals(_x) && other._y.Equals(_y) && other._width.Equals(_width) && other._height.Equals(_height);
        }
        
        public bool Equals(Rect other) {
            return other.x.Equals(_x) && other.y.Equals(_y) && other.width.Equals(_width) && other.height.Equals(_height);
        }
        
        public override bool Equals(object obj) {
        #if UNITY_EDITOR
            if (obj == null) {
                return false;
            }
        #endif
            return obj is RectLink other && other.Equals(this);
        }
        
        public override int GetHashCode() => rect.GetHashCode();
    }
}