// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TinyUtilities.CustomTypes {
    public static class EnumName {
        public static EnumName<T> New<T>(T value) where T : Enum => new EnumName<T>(value);
    }
    
    [Serializable, InlineProperty]
    public sealed class EnumName<T> : IEquatable<EnumName<T>>, IEquatable<T> where T : Enum {
        [SerializeField, HideInInspector]
        private string _enum;
        
        [ShowInInspector, HideLabel]
        public T value {
            get {
                if (Enum.TryParse(typeof(T), _enum, out object result)) {
                    return (T)result;
                }
                
                return default;
            }
            private set => _enum = value.ToString();
        }
        
        internal EnumName(T value) => this.value = value;
        
        public bool Equals(EnumName<T> other) => other != null && other._enum.Equals(_enum);
        
        public bool Equals(T other) => other != null && other.ToString().Equals(_enum);
        
        public override bool Equals(object obj) => obj != null && obj is EnumName<T> other && other._enum.Equals(_enum);
        
        public override int GetHashCode() => value.GetHashCode();
        
        public override string ToString() => _enum;
    }
}