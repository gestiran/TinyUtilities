// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Collections.Generic;

namespace TinyUtilities {
    public static class ArrayDataConverter {
        public static byte[] ToByteArray(this int[] arr) {
            byte[] result = new byte[arr.Length];
            
            for (ushort i = 0; i < result.Length; i++) {
                result[i] = (byte)arr[i];
            }
            
            return result;
        }
        
        public static byte[] ToByteArray(this List<int> arr) {
            byte[] result = new byte[arr.Count];
            
            for (ushort i = 0; i < result.Length; i++) {
                result[i] = (byte)arr[i];
            }
            
            return result;
        }
    }
}