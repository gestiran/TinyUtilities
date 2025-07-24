// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using Sirenix.OdinInspector;
using UnityEngine;

namespace TinyUtilities {
    public sealed class DebugViews : MonoBehaviour {
        [field: SerializeField, BoxGroup("Debug"), AssetsOnly, AssetSelector(Paths = "Packages/com.geg.applicationstats/Prefabs"), Required]
        public GameObject applicationStatsCanvas { get; private set; }
        
        [field: SerializeField, BoxGroup("Debug"), AssetsOnly, AssetSelector(Paths = "Packages/com.yasirkula.ingame.debug.console"), Required]
        public GameObject inGameDebugConsole { get; private set; }
        
        private void Start() {
            applicationStatsCanvas = Instantiate(applicationStatsCanvas);
            inGameDebugConsole = Instantiate(inGameDebugConsole);
            
            DontDestroyOnLoad(applicationStatsCanvas);
            DontDestroyOnLoad(inGameDebugConsole);
        }
    }
}