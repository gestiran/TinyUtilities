using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TinyUtilities.CustomTypes {
#if ODIN_INSPECTOR && UNITY_EDITOR
    [InlineProperty]
#endif
    [Serializable]
    public sealed class RectLink : IEquatable<RectLink>, IEquatable<Rect> {
        public Rect rect => new Rect(_x, _y, _width, _height);
        
    #if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup, HideLabel, SuffixLabel("X", true)]
    #endif
        [SerializeField]
        private float _x;
        
    #if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup, HideLabel, SuffixLabel("Y", true)]
    #endif
        [SerializeField]
        private float _y;
        
    #if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup, HideLabel, SuffixLabel("Width", true)]
    #endif
        [SerializeField]
        private float _width;
        
    #if ODIN_INSPECTOR && UNITY_EDITOR
        [HorizontalGroup, HideLabel, SuffixLabel("Height", true)]
    #endif
        [SerializeField]
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