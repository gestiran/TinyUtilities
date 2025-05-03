using Sirenix.OdinInspector;

namespace TinyUtilities.Editor.MergeScripts.Pairs {
    [ShowInInspector, HideLabel, InlineProperty, HideReferenceObjectPicker, HideDuplicateReferenceBox]
    public sealed class ChangeAssemblyPair : ChangePair {
        [field: ShowInInspector, SuffixLabel("Target Namespace", true), HideLabel]
        public string targetNamespace { get; private set; }
        
        [field: ShowInInspector, SuffixLabel("Current Assembly", true), HideLabel]
        public string current { get; private set; }
        
        [field: ShowInInspector, SuffixLabel("New Assembly", true), HideLabel]
        public string next { get; private set; }
    }
}