// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

// using System.Collections.Generic;
// using System.Reflection;
//
// namespace Holein.Extensions {
//     public static class GPGSIdsExtension {
//         private static readonly Dictionary<string, string> _Ids;
//
//         static GPGSIdsExtension() {
//             _Ids = CreateDictionary();
//         }
//         
//         public static bool TryGetAchievementName(string id, out string achievementName) {
//             if (_Ids.ContainsKey(id)) {
//                 achievementName = _Ids[id];
//                 return true;
//             }
//
//             achievementName = null;
//             return false;
//         }
//
//         private static Dictionary<string, string> CreateDictionary() {
//             FieldInfo[] achievementsIds = typeof(GPGSIds).GetFields(BindingFlags.Static | BindingFlags.Public);
//             Dictionary<string, string> ids = new Dictionary<string, string>(achievementsIds.Length);
//
//             for (int fieldId = 0; fieldId < achievementsIds.Length; fieldId++) {
//                 object achievementName = achievementsIds[fieldId].GetValue(null);
//
//                 if (achievementName is not string achievementNameText) {
//                     continue;
//                 }
//                 
//                 ids.Add(achievementNameText, achievementsIds[fieldId].Name);
//             }
//
//             return ids;
//         }
//     }
// }