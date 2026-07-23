# TinyUtilities.Editor

A lightweight collection of utilities, extensions, and helper classes for Unity Editor.  
It provides essential tools for asset processing, scene management, play mode configuration, and editor workflow optimization without heavy dependencies.

**Key Features:**
- Automated asset post-processing pipelines (Colliders, Shadows, Layers)
- Customizable Play Mode entry behaviors (Boot scenes, Async cleanup, DoTween reset)
- Editor window tools for merging objects and script references
- Adaptive grid snapping system for precise placement
- Odin Inspector custom drawers and validation rules
- Texture compression validation and enforcement
- Various editor shortcuts and utility extensions

## Table of Contents

- [Editor](#editor)
    - [`EditorSceneUtility`](#editorsceneutility)
    - [`SelectionUtility`](#selectionutility)
- [AssetProcessors](#assetprocessors)
    - [CollidersImport](#collidersimport)
        - [`CollidersImportModule`](#collidersimportmodule)
        - [`CollidersImportPostProcessor`](#collidersimportpostprocessor)
    - [LayerChange](#layerchange)
        - [`LayerChangeModule`](#layerchangemodule)
        - [`LayerChangePostProcessor`](#layerchangepostprocessor)
    - [ShadowsImport](#shadowsimport)
        - [`ShadowImportPostProcessor`](#shadowimportpostprocessor)
        - [`ShadowsImportModule`](#shadowsimportmodule)
    - [`AssetProcessorsPrefs`](#assetprocessorsprefs)
    - [`AssetProcessorsProjectSettings`](#assetprocessorsprojectsettings)
    - [`ImportPrefixes`](#importprefixes)
- [EditorInputs](#editorinputs)
    - [`EditorInput`](#editorinput)
- [EnterPlayMode](#enter-play-mode)
    - [AssemblyPipeline](#assemblypipeline)
        - [`AssemblyPipelinePrefs`](#assemblypipelineprefs)
        - [`AssemblyPipelineProjectSettings`](#assemblypipelineprojectsettings)
    - [AsyncTools](#asynctools)
        - [`AsyncToolsModule`](#asynctoolsmodule)
        - [`AsyncToolsPrefs`](#asynctoolsprefs)
    - [BeforePlayMode](#beforeplaymode)
        - [`BeforePlayModePrefs`](#beforeplaymodeprefs)
        - [`BootSceneModule`](#bootscenemodule)
    - [DoTweenTools](#dotweentools)
        - [`DoTweenToolsModule`](#dotweentoolsmodule)
        - [`DoTweenToolsPrefs`](#dotweentoolsprefs)
    - [`EnterPlayModeProjectSettings`](#enterplaymodeprojectsettings)
- [Extensions](#extensions)
    - [`GameObjectExtension`](#gameobjectextension)
- [GridSnapping](#gridsnapping)
    - [`GridSnappingAdaptive`](#gridsnappingadaptive)
    - [`GridSnappingPrefs`](#gridsnappingprefs)
    - [`GridSnappingProjectSettings`](#gridsnappingprojectsettings)
- [MergeObjects](#mergeobjects)
    - [`MergeObjectsWindow`](#mergeobjectswindow)
- [MergeScripts](#mergescripts)
    - [Handlers](#handlers)
        - [`MergeScriptsHandler`](#mergescriptshandler)
        - [`MergeScriptsValidator`](#mergescriptsvalidator)
    - [Pairs](#pairs)
        - [`ChangeAssemblyPair`](#changeassemblypair)
        - [`ChangeGUIDPair`](#changeguidpair)
        - [`ChangeNamespacePair`](#changenamespacepair)
        - [`ChangePair`](#changepair)
    - [`MergeScriptsWindow`](#mergescriptswindow)
- [Odin](#odin)
    - [`DateTimeDrawer`](#datetimedrawer)
    - [`OdinGUI`](#odingui)
    - [`TimeSpanDrawer`](#timespandrawer)
- [Shortcuts](#shortcuts)
    - [`AssetReserializeShortcut`](#assetreserializeshortcut)
    - [`ChangeMaximizeShortcut`](#changemaximizeshortcut)
    - [`DisableShadowsShortcut`](#disableshadowsshortcut)
    - [`SetDirtyShortcut`](#setdirtyshortcut)
- [TexturesCompressor](#texturescompressor)
    - [`AssetImporterValidator`](#assetimportervalidator)
    - [`TextureImporterExtension`](#textureimporterextension)
    - [`TexturesCompressorRule`](#texturescompressorrule)
    - [`TexturesCompressorSettings`](#texturescompressorsettings)
- [Utilities](#utilities)
    - [`AssetsUtility`](#assetsutility)
    - [`GUIDrawUtility`](#guidrawutility)
    - [`ProjectUtility`](#projectutility)
    - [`RectTransformBakeUtility`](#recttransformbakeutility)
    - [`ReflectionUtility`](#reflectionutility)

## Editor

Core editor utilities for scene management and object selection.

### [`EditorSceneUtility`](EditorSceneUtility.cs)

Provides methods to invoke actions across multiple scenes within a specified folder.
Includes tools for creating new GameObjects with specific hierarchy positioning and retrieving components across the active scene hierarchy.

### [`SelectionUtility`](SelectionUtility.cs)

Offers utility methods focused on the current editor selection.
Allows retrieval of components from all selected GameObjects and their children, supporting inclusive searches for inactive objects.

## AssetProcessors

Automated pipelines that run during asset import to configure models and textures based on predefined rules and naming conventions.

### [`AssetProcessorsPrefs`](AssetProcessors/AssetProcessorsPrefs.cs)

Handles persistent storage of asset processor settings using EditorPrefs.
Supports serialization of boolean flags, integers, and string arrays.

### [`AssetProcessorsProjectSettings`](AssetProcessors/AssetProcessorsProjectSettings.cs)

Initializes asset processor modules and provides a custom Project Settings provider for configuring global asset processing rules.

### [`ImportPrefixes`](AssetProcessors/ImportPrefixes.cs)

Defines constant string values used as prefixes for identifying special import behaviors such as collider types and shadow objects.

### CollidersImport

Module responsible for automatically generating colliders on imported models based on object name prefixes.

#### [`CollidersImportModule`](AssetProcessors/CollidersImport/CollidersImportModule.cs)

Manages the enable state and configuration UI for the collider import post-processor.
Defines the mapping between name prefixes and collider types.

#### [`CollidersImportPostProcessor`](AssetProcessors/CollidersImport/CollidersImportPostProcessor.cs)

Implements the asset post-processing logic.
Detects helper objects with specific prefixes, generates corresponding colliders on parent objects, and cleans up the helper objects.

### LayerChange

Module responsible for overriding layer settings on imported assets.

#### [`LayerChangeModule`](AssetProcessors/LayerChange/LayerChangeModule.cs)

Manages the enable state and target layer configuration for the layer override post-processor.

#### [`LayerChangePostProcessor`](AssetProcessors/LayerChange/LayerChangePostProcessor.cs)

Applies the configured layer to the root and all children of imported models during the import process.

### ShadowsImport

Module responsible for configuring shadow casting modes on imported models.

#### [`ShadowImportPostProcessor`](AssetProcessors/ShadowsImport/ShadowImportPostProcessor.cs)

Identifies shadow-only objects via prefixes and disables shadow casting on parent or sibling objects to optimize rendering performance.

#### [`ShadowsImportModule`](AssetProcessors/ShadowsImport/ShadowsImportModule.cs)

Manages the enable state and prefix configuration for the shadow import post-processor.

## EditorInputs

Utilities for handling low-level editor input events.

### [`EditorInput`](EditorInputs/EditorInput.cs)

Hooks into the global editor event handler to track specific key states, such as the Control key, for use in custom editor tools.

## Enter Play Mode

Modules that configure behavior and perform cleanup tasks when entering or exiting Play Mode.

### [`EnterPlayModeProjectSettings`](EnterPlayMode/EnterPlayModeProjectSettings.cs)

Initializes play mode modules and provides a custom Project Settings provider for configuring global play mode behaviors.

### AssemblyPipeline

Controls assembly reloading behavior when using Enter Play Mode options that disable domain reload.

#### [`AssemblyPipelinePrefs`](EnterPlayMode/AssemblyPipeline/AssemblyPipelinePrefs.cs)

Manages persistent storage for assembly pipeline settings.

#### [`AssemblyPipelineProjectSettings`](EnterPlayMode/AssemblyPipeline/AssemblyPipelineProjectSettings.cs)

Provides settings UI and logic to force assembly reloading when necessary during play mode state changes.

### AsyncTools

Manages asynchronous task cleanup when exiting Play Mode.

#### [`AsyncToolsModule`](EnterPlayMode/AsyncTools/AsyncToolsModule.cs)

Resets the synchronization context when exiting play mode to ensure async operations do not persist incorrectly between sessions.

#### [`AsyncToolsPrefs`](EnterPlayMode/AsyncTools/AsyncToolsPrefs.cs)

Manages persistent storage for async tools settings.

### BeforePlayMode

Configures specific behaviors before entering Play Mode.

#### [`BeforePlayModePrefs`](EnterPlayMode/BeforePlayMode/BeforePlayModePrefs.cs)

Manages persistent storage for boot scene and play mode settings.

#### [`BootSceneModule`](EnterPlayMode/BeforePlayMode/BootSceneModule.cs)

Configures a specific scene to load automatically when entering Play Mode based on Build Settings indices.

### DoTweenTools

Handles DOTween cleanup when exiting Play Mode.

#### [`DoTweenToolsModule`](EnterPlayMode/DoTweenTools/DoTweenToolsModule.cs)

Clears cached tweens and resets the tweening engine to prevent state leakage into the next session.

#### [`DoTweenToolsPrefs`](EnterPlayMode/DoTweenTools/DoTweenToolsPrefs.cs)

Manages persistent storage for DoTween tools settings.

## Extensions

Extension methods that attach new functionality directly onto existing Unity types.

### [`GameObjectExtension`](Extensions/GameObjectExtension.cs)

Extends GameObject and Transform arrays with navigation methods.
Includes functionality to retrieve all children recursively, check for name containment, filter components, and set parents in bulk.

## GridSnapping

Adaptive grid snapping system for precise object placement in the editor.

### [`GridSnappingAdaptive`](GridSnapping/GridSnappingAdaptive.cs)

Listens to object change events and automatically snaps selected objects to the grid based on active snap settings and key modifiers.

### [`GridSnappingPrefs`](GridSnapping/GridSnappingPrefs.cs)

Manages persistent storage for grid snapping settings.

### [`GridSnappingProjectSettings`](GridSnapping/GridSnappingProjectSettings.cs)

Provides a custom Project Settings provider for enabling or disabling the adaptive snapping system.

## MergeObjects

Editor window tools for merging components between objects.

### [`MergeObjectsWindow`](MergeObjects/MergeObjectsWindow.cs)

Provides an interface for copying components from one GameObject to another.
Supports selecting specific component types to merge and options to destroy the source object after merging.

## MergeScripts

Editor window tools for bulk editing script references in assets.

### [`MergeScriptsWindow`](MergeScripts/MergeScriptsWindow.cs)

Provides an interface for configuring and executing script merge operations.
Supports progress tracking and cancellation.

### Handlers

Core logic for processing script changes across assets.

#### [`MergeScriptsHandler`](MergeScripts/Handlers/MergeScriptsHandler.cs)

Executes asynchronous text replacement processes on selected or all prefab and scene assets based on provided change pairs.

#### [`MergeScriptsValidator`](MergeScripts/Handlers/MergeScriptsValidator.cs)

Validates change pairs before processing to ensure data integrity and prevent invalid replacements.

### Pairs

Data structures defining specific types of script reference changes.

#### [`ChangeAssemblyPair`](MergeScripts/Pairs/ChangeAssemblyPair.cs)

Defines a pair of current and new assembly names for a target namespace.

#### [`ChangeGUIDPair`](MergeScripts/Pairs/ChangeGUIDPair.cs)

Defines a pair of current and new GUIDs for asset reference replacement.

#### [`ChangeNamespacePair`](MergeScripts/Pairs/ChangeNamespacePair.cs)

Defines a pair of current and new namespaces for script reference replacement.

#### [`ChangePair`](MergeScripts/Pairs/ChangePair.cs)

Abstract base class for defining script change pairs.

## Odin

Custom drawers and GUI utilities for Odin Inspector.

### [`DateTimeDrawer`](Odin/DateTimeDrawer.cs)

Custom Odin drawer for DateTime structs. Displays individual fields for year, month, day, hour, minute, and second for precise editing.

### [`OdinGUI`](Odin/OdinGUI.cs)

Provides helper methods for drawing custom GUI elements within Odin inspectors, such as inline fields with suffix labels.

### [`TimeSpanDrawer`](Odin/TimeSpanDrawer.cs)

Custom Odin drawer for TimeSpan structs. Displays individual fields for days, hours, minutes, and seconds.

## Shortcuts

Menu items and shortcuts for common editor tasks.

### [`AssetReserializeShortcut`](Shortcuts/AssetReserializeShortcut.cs)

Adds a context menu option to force reserialization of selected assets including metadata.

### [`ChangeMaximizeShortcut`](Shortcuts/ChangeMaximizeShortcut.cs)

Adds a menu option to toggle the maximized state of the currently focused editor window.

### [`DisableShadowsShortcut`](Shortcuts/DisableShadowsShortcut.cs)

Adds context menu options to disable shadow casting on selected GameObjects, their children, or specific child slots.

### [`SetDirtyShortcut`](Shortcuts/SetDirtyShortcut.cs)

Adds a menu option to mark selected assets as dirty and save them, forcing a reload.

## TexturesCompressor

Validation and settings for texture compression standards.

### [`AssetImporterValidator`](TexturesCompressor/AssetImporterValidator.cs)

Abstract base class for creating Odin validation rules on asset importers.

### [`TextureImporterExtension`](TexturesCompressor/TextureImporterExtension.cs)

Adds helper methods to TextureImporter instances.
Simplifies common tasks such as disabling mipmaps, removing alpha sources, and triggering reimports.

### [`TexturesCompressorRule`](TexturesCompressor/TexturesCompressorRule.cs)

Implements Odin validation logic to check texture settings against project standards.
Reports errors for invalid compression formats, sizes, or mipmaps.

### [`TexturesCompressorSettings`](TexturesCompressor/TexturesCompressorSettings.cs)

Manages global settings for texture compression validation.
Defines maximum texture sizes and preferred compression formats for solid and alpha textures.

## Utilities

General purpose helper classes for editor scripting.

### [`AssetsUtility`](Utilities/AssetsUtility.cs)

Contains helper methods for asset database interactions.
Includes functionality to find assets by path, check for naming prefixes on transforms and GameObjects, and strip prefixes or LOD postfixes from names.

### [`GUIDrawUtility`](Utilities/GUIDrawUtility.cs)

A collection of helper methods for drawing common GUI elements in the Editor.
Supports toggles with auto-save callbacks, layer fields, text fields with dirty checking, and dynamic list management.

### [`ProjectUtility`](Utilities/ProjectUtility.cs)

Provides project-specific identification utilities.
Generates a unique key based on the project path for use in persistent storage keys.

### [`RectTransformBakeUtility`](Utilities/RectTransformBakeUtility.cs)

Adds a context menu option to RectTransform components.
Allows baking of scale and size information into the rect transform while recording undo states for relevant UI components.

### [`ReflectionUtility`](Utilities/ReflectionUtility.cs)

Provides reflection-based helpers to get or set private and public fields and properties on objects.
Automatically marks Unity objects as dirty when modifications are made via reflection.