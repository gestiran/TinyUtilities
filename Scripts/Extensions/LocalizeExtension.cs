// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

#if I2_LOCALIZE
    using I2.Loc;
#endif

namespace TinyUtilities.Extensions {
    public static class LocalizeExtension {
    #if I2_LOCALIZE
        public static void SetTerm(this Localize[] objects, string primary) {
            for (int i = 0; i < objects.Length; i++) {
                objects[i].SetTerm(primary);
            }
        }
        
        public static void SetTerm(this Localize[] objects, string primary, string secondary) {
            for (int i = 0; i < objects.Length; i++) {
                objects[i].SetTerm(primary, secondary);
            }
        }
        
    #endif
    }
}