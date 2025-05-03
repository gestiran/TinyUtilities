using System;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using System.Collections.Generic;

#if I2_LOCALIZE
using I2.Loc;
#endif

#endif

namespace TinyUtilities.CustomTypes {
    [Serializable, InlineProperty, HideReferenceObjectPicker, HideDuplicateReferenceBox]
    public sealed class I2LocTerm : IEquatable<I2LocTerm> {
        [ValueDropdown("GetAllTerms"), OnValueChanged("UpdatePreview"), HorizontalGroup, HideLabel]
        public string term;
        
    #if UNITY_EDITOR
        
        [ShowInInspector, HorizontalGroup, HideLabel, ReadOnly]
        private string _valuePreview;
        
    #endif
        
        public static implicit operator string(I2LocTerm term) => term.term;
        
        public override string ToString() => term;
        
        public bool Equals(I2LocTerm other) => other != null && other.term == term;
        
        public override bool Equals(object obj) {
        #if UNITY_EDITOR
            if (obj == null) return false;
        #endif
            return obj is I2LocTerm loc && loc.Equals(this);
        }
        
        public override int GetHashCode() => term.GetHashCode();
        
    #if UNITY_EDITOR
        
        [OnInspectorInit]
        private void UpdatePreview() {
        #if I2_LOCALIZE
            if (TryGetSources(out LanguageSourceAsset source)) {
                _valuePreview = source.mSource.GetTranslation(term);
            }
        #endif
        }
        
        private List<string> GetAllTerms() {
        #if I2_LOCALIZE
            if (TryGetSources(out LanguageSourceAsset source)) {
                return source.mSource.GetTermsList();
            }
        #endif
            return new List<string>();
        }
        
    #if I2_LOCALIZE
        private bool TryGetSources(out LanguageSourceAsset source) {
            source = Resources.Load<LanguageSourceAsset>("I2Languages");
            
            if (source == null) {
                return false;
            }
            
            return true;
        }
        
    #endif
    #endif
    }
}