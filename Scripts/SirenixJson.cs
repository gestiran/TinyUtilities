﻿using System.Text;
using Sirenix.Serialization;

namespace TinyUtilities {
    public static class SirenixJson {
        public static string To<T>(T value) {
            byte[] bytes = SerializationUtility.SerializeValue(value, DataFormat.JSON);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
        
        public static T From<T>(string json) {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return SerializationUtility.DeserializeValue<T>(bytes, DataFormat.JSON);
        }
    }
}