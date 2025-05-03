using Sirenix.OdinInspector;

namespace TinyUtilities.Editor.MergeScripts.Pairs {
    [ShowInInspector, HideLabel, InlineProperty, HideReferenceObjectPicker, HideDuplicateReferenceBox]
    public sealed class ChangeNamespacePair : ChangePair {
        [field: ShowInInspector, SuffixLabel("Current Namespace", true), HideLabel]
        public string current { get; private set; }
        
        [field: ShowInInspector, SuffixLabel("New Namespace", true), HideLabel]
        public string next { get; private set; }
    }
}