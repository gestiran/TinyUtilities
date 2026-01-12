// Copyright (c) 2023 Derek Sliman
// Licensed under the MIT License. See LICENSE.md for details.

using System.Text;
using TinyUtilities.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace TinyUtilities.Editor.AssetProcessors.CollidersImport {
    public sealed class CollidersImportModule {
        public static bool isEnabled { get; private set; }
        
        private readonly AssetProcessorsPrefs _prefs;
        
        public CollidersImportModule() {
            _prefs = new AssetProcessorsPrefs();
            Init();
        }
        
        public void Init() {
            isEnabled = _prefs.LoadIsEnable(false);
        }
        
        public void Draw() {
            EditorGUILayout.LabelField($"Сolliders ({(isEnabled ? "Enabled" : "Disabled")})", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(HelpMessage(), MessageType.Info);
            
            GUIContent isEnableLabel = new GUIContent("Enabled");
            isEnableLabel.tooltip = "Enable colliders import post processors.";
            isEnabled = GUIDrawUtility.DrawToggle(isEnableLabel, isEnabled, _prefs.SaveIsEnable);
        }
        
        private string HelpMessage() {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("The required Collider will be added to the parent object depending on the prefix.");
            builder.AppendLine();
            builder.AppendLine("Used prefixes:");
            builder.AppendLine($"{ImportPrefixes.BOX_COLLIDER} - {nameof(BoxCollider)}");
            builder.AppendLine($"{ImportPrefixes.CAPSULE_COLLIDER} - {nameof(CapsuleCollider)}");
            builder.AppendLine($"{ImportPrefixes.SPHERE_COLLIDER} - {nameof(SphereCollider)}");
            builder.AppendLine($"{ImportPrefixes.MESH_CONVEX_COLLIDER} - {nameof(MeshCollider)} with convex");
            builder.AppendLine($"{ImportPrefixes.MESH_COLLIDER} - {nameof(MeshCollider)}");
            
            return builder.ToString();
        }
    }
}