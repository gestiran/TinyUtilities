#if !ODIN_INSPECTOR
    using System;
    using System.Collections.Generic;
    
    namespace TinyUtilities {
        public interface ISelfValidator {
            public void Validate(SelfValidationResult result) { }
        }
        
        public sealed class OnValueChangedAttribute : Attribute {
            public OnValueChangedAttribute(string value) { }
        }
        
        public sealed class FoldoutGroupAttribute : Attribute {
            public int Order;
            
            public FoldoutGroupAttribute(string value = null, int priority = 0) { }
        }
        
        public sealed class BoxGroupAttribute : Attribute {
            public BoxGroupAttribute(string value = null) { }
        }
        
        public sealed class ShowIfAttribute : Attribute {
            public ShowIfAttribute(string value) { }
        }
        
        public sealed class EnableIfAttribute : Attribute {
            public EnableIfAttribute(string value) { }
        }
        
        public sealed class DisableIfAttribute : Attribute {
            public DisableIfAttribute(string value) { }
        }
        
        public sealed class HideIfAttribute : Attribute {
            public HideIfAttribute(string value) { }
        }
        
        public sealed class LabelTextAttribute : Attribute {
            public LabelTextAttribute(string value) { }
        }
        
        public sealed class ChildGameObjectsOnlyAttribute : Attribute {
            public ChildGameObjectsOnlyAttribute(bool includeInactive = false) { }
        }
        
        public sealed class HorizontalGroupAttribute : Attribute {
            public HorizontalGroupAttribute(string value = null) { }
        }
        
        public sealed class TitleAttribute : Attribute {
            public TitleAttribute(string value, bool horizontalLine = true) { }
        }
        
        public sealed class ValueDropdownAttribute : Attribute {
            public bool CopyValues;
            public bool DrawDropdownForListElements;
            
            public ValueDropdownAttribute(string value) { }
        }
        
        public sealed class ListDrawerSettingsAttribute : Attribute {
            public int NumberOfItemsPerPage;
            public bool HideAddButton;
            public bool HideRemoveButton;
            public bool DraggableItems;
            public bool ShowFoldout;
            public string ListElementLabelName;
            
            public ListDrawerSettingsAttribute() { }
        }
        
        public sealed class RequiredAttribute : Attribute { }
        
        public sealed class SearchableAttribute : Attribute {
            public bool FuzzySearch;
            public bool Recursive;
            public SearchFilterOptions FilterOptions;
        }
        
        public sealed class SuffixLabelAttribute : Attribute {
            public SuffixLabelAttribute(string value, bool horizontalLine = true) { }
        }
        
        public sealed class InlinePropertyAttribute : Attribute { }
        
        public sealed class HideReferenceObjectPickerAttribute : Attribute { }
        
        public sealed class HideDuplicateReferenceBoxAttribute : Attribute { }
        
        public sealed class ShowInInspectorAttribute : Attribute { }
        
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true)]
        public sealed class CustomContextMenuAttribute : Attribute {
            public CustomContextMenuAttribute(string title, string method) { }
        }
        
        public sealed class ReadOnlyAttribute : Attribute { }
        
        public sealed class OnInspectorInitAttribute : Attribute { }
        
        public sealed class OnInspectorDisposeAttribute : Attribute { }
        
        public sealed class AssetsOnlyAttribute : Attribute { }
        
        public sealed class HideLabelAttribute : Attribute { }
        
        public sealed class MinValueAttribute : Attribute {
            public MinValueAttribute(int value) { }
        }
        
        public sealed class ButtonAttribute : Attribute {
            public ButtonAttribute(string value = null) { }
        }
        
        public sealed class EnumToggleButtonsAttribute : Attribute { }
        
        public sealed class DisableInPlayModeAttribute : Attribute { }
        
        public sealed class HideInEditorModeAttribute : Attribute { }
        
        public sealed class HideInPlayModeAttribute : Attribute { }
        
        public sealed class ProgressBarAttribute : Attribute {
            public string MaxGetter;
            
            public ProgressBarAttribute(int from, int to) { }
        }
        
        public sealed class AssetSelectorAttribute : Attribute {
            public string Paths;
        }
        
        public sealed class SelfValidationResult {
            private static ResultItem _default;
            
            static SelfValidationResult() => _default = new ResultItem();
            
            public struct ResultItem {
                public ref ResultItem WithFix(Action action) => ref _default;
            }
            
            public ref ResultItem AddError(string message) => ref _default;
            
            public ref ResultItem AddWarning(string message) => ref _default;
        }
        
        public sealed class ValueDropdownItem<T> {
            public ValueDropdownItem(string title, T value) { }
        }
        
        public sealed class ValueDropdownList<T> : List<ValueDropdownItem<T>> {
            public ValueDropdownList() { }
            
            public void Add(T value) { }
            
            public void Add(string title, T value) { }
        }
        
        [Flags]
        public enum SearchFilterOptions {
            TypeOfValue,
            ValueToString
        }
    }
#endif