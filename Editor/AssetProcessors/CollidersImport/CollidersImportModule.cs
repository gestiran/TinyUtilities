// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using TinyUtilities.Editor.Utilities;

namespace TinyUtilities.Editor.AssetProcessors.CollidersImport {
    public sealed class CollidersImportModule {
        public static bool isEnable { get; private set; }
        
        private readonly CollidersImportPrefs _prefs;
        
        public CollidersImportModule() {
            _prefs = new CollidersImportPrefs();
            Init();
        }
        
        public void Init() {
            isEnable = _prefs.LoadIsEnable();
        }
        
        public void Draw() {
            isEnable = GUIDrawUtility.DrawToggle("Import colliders", isEnable, _prefs.SaveIsEnable);
        }
    }
}