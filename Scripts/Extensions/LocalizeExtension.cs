// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if I2_LOCALIZE
using I2.Loc;
using System.Collections.Generic;

namespace TinyUtilities.Extensions {
    public static class LocalizeExtension {
        public static void SetTerm<T>(this T objects, string primary) where T : ICollection<Localize> {
            foreach (var localize in objects) {
                localize.SetTerm(primary);
            }
        }
        
        public static void SetTerm<T>(this T objects, string primary, string secondary) where T : ICollection<Localize> {
            foreach (var localize in objects) {
                localize.SetTerm(primary, secondary);
            }
        }
        
        public static void SetParameterValue<T>(this T parameters, string paramName, string paramValue, bool localize = true) where T : ICollection<LocalizationParamsManager> {
            foreach (var parameter in parameters) {
                parameter.SetParameterValue(paramName, paramValue, localize);
            }
        }
    }
}
#endif