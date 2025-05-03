using System;

namespace TinyUtilities.CustomTypes {
    public readonly struct BlittableBool : IEquatable<BlittableBool>, IComparable<BlittableBool> {
        private readonly byte _value;
        
        public BlittableBool(byte value) => _value = value;
        
        public BlittableBool(bool value) => _value = (byte)(value ? 1 : 0);
        
        public static implicit operator bool(BlittableBool bb) => bb._value == 1;
        
        public static implicit operator BlittableBool(bool value) => new BlittableBool(value);
        
        public bool Equals(BlittableBool other) => _value == other._value;
        
        public override bool Equals(object obj) => obj is BlittableBool other && Equals(other);
        
        public int CompareTo(BlittableBool other) => _value.CompareTo(other._value);
        
        public override int GetHashCode() => _value.GetHashCode();
        
        public override string ToString() => (_value == 1).ToString();
    }
}