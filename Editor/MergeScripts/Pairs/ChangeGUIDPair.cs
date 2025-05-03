using Sirenix.OdinInspector;

namespace TinyUtilities.Editor.MergeScripts.Pairs {
    [ShowInInspector, HideLabel, InlineProperty, HideReferenceObjectPicker, HideDuplicateReferenceBox]
    public sealed class ChangeGUIDPair : ChangePair {
        [field: ShowInInspector, SuffixLabel("Current GUID", true), HideLabel]
        public string current { get; private set; }
        
        [field: ShowInInspector, SuffixLabel("New GUID", true), HideLabel]
        public string next { get; private set; }
    }
}