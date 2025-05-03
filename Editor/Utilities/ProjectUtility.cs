using System.IO;
using UnityEngine;

namespace TinyUtilities.Editor.Utilities {
    public static class ProjectUtility {
        public static readonly string project;
        
        static ProjectUtility() {
            project = Path.GetFileName(Path.GetDirectoryName(Application.dataPath));
        }
    }
}