// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System;
using UnityEngine;

#if ODIN_INSPECTOR
using System.IO;
using Sirenix.Serialization;
#endif

namespace TinyUtilities {
    public static class IDUtility {
        private static uint _id;
        
        private const string _ID_KEY = "R_ID";
        
        static IDUtility() => _id = Load();
        
        public static uint Get() {
            uint result = _id;
            
            _id += 1;
            Save(_id);
            
            return result;
        }
        
        public static uint[] Get(int count) {
            if (count <= 0) {
                return Array.Empty<uint>();
            }
            
            uint[] result = new uint[count];
            result[0] = _id;
            
            for (int i = 1; i < count; i++) {
                _id += 1;
                result[i] = _id;
            }
            
            Save(_id);
            return result;
        }
        
        public static string GetString() => $"{Get():0000000000}";
        
        private static void Save(uint id) {
        #if ODIN_INSPECTOR
            byte[] data = SerializationUtility.SerializeValue(id, DataFormat.Binary);
            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, _ID_KEY), data);
        #else
            PlayerPrefs.SetInt(_ID_KEY, (int)id);
        #endif
        }
        
        private static uint Load() {
        #if ODIN_INSPECTOR
            string path = Path.Combine(Application.persistentDataPath, _ID_KEY);
            
            if (!File.Exists(path)) {
                return default;
            }
            
            byte[] bytes = File.ReadAllBytes(path);
            
            return SerializationUtility.DeserializeValue<uint>(bytes, DataFormat.Binary);
        #else
            return (uint)PlayerPrefs.GetInt(_ID_KEY, 0);
        #endif
        }
    }
}