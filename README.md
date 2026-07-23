# TinyUtilities

A comprehensive lightweight collection of utilities, extensions, and helper classes for Unity development. TinyUtilities is designed to boost productivity and maintain code consistency across projects without introducing heavy dependencies. The library is strictly divided into Editor and Runtime assemblies to ensure no editor code leaks into final builds.

## Installation

1. Clone the repository into your Unity project's Assets or Packages folder.
2. Ensure required dependencies (Odin Inspector, UniTask, DOTween) are installed if you intend to use specific modules that rely on them.
3. Configure Project Settings under the TinyUtilities section to enable or disable specific Editor modules such as Asset Processors, Play Mode behaviors, and Grid Snapping.

## Table of Contents

- [Editor](#editor)
    - [Scene and Selection Utilities](#scene-and-selection-utilities)
    - [Asset Processors](#asset-processors)
    - [Enter Play Mode Configuration](#enter-play-mode-configuration)
    - [Editor Windows and Tools](#editor-windows-and-tools)
    - [Grid Snapping](#grid-snapping)
    - [Odin Inspector Extensions](#odin-inspector-extensions)
    - [Shortcuts and Utilities](#shortcuts-and-utilities)
- [Runtime](#runtime)
    - [Core Utilities](#core-utilities)
    - [UI Components](#ui-components)
    - [Extensions](#extensions)
    - [Network and Time](#network-and-time)
    - [Safe Area and Orientation](#safe-area-and-orientation)
    - [Smart Links and States](#smart-links-and-states)
    - [Custom Types and Serialization](#custom-types-and-serialization)
    - [Validation and Vibration](#validation-and-vibration)

## Editor

The Editor module provides essential tools for asset processing, scene management, play mode configuration, and editor workflow optimization. It includes automated pipelines, custom windows, and extensive utility classes to streamline development.

### Scene and Selection Utilities

- **EditorSceneUtility:** Provides methods to invoke actions across multiple scenes within a specified folder. Includes tools for creating new GameObjects with specific hierarchy positioning and retrieving components across the active scene hierarchy.
- **SelectionUtility:** Offers utility methods focused on the current editor selection. Allows retrieval of components from all selected GameObjects and their children, supporting inclusive searches for inactive objects.

### Asset Processors

Automated pipelines that run during asset import to configure models and textures based on predefined rules and naming conventions.

- **AssetProcessorsPrefs & ProjectSettings:** Handles persistent storage of asset processor settings using EditorPrefs and provides a custom Project Settings provider for configuring global asset processing rules.
- **CollidersImport:** Module responsible for automatically generating colliders on imported models based on object name prefixes. The post-processor detects helper objects with specific prefixes, generates corresponding colliders on parent objects, and cleans up the helper objects.
- **LayerChange:** Module responsible for overriding layer settings on imported assets. Applies the configured layer to the root and all children of imported models during the import process.
- **ShadowsImport:** Module responsible for configuring shadow casting modes on imported models. Identifies shadow-only objects via prefixes and disables shadow casting on parent or sibling objects to optimize rendering performance.
- **ImportPrefixes:** Defines constant string values used as prefixes for identifying special import behaviors such as collider types and shadow objects.

### Enter Play Mode Configuration

Modules that configure behavior and perform cleanup tasks when entering or exiting Play Mode.

- **EnterPlayModeProjectSettings:** Initializes play mode modules and provides a custom Project Settings provider for configuring global play mode behaviors.
- **AssemblyPipeline:** Controls assembly reloading behavior when using Enter Play Mode options that disable domain reload. Manages persistent storage and provides settings UI to force assembly reloading when necessary.
- **AsyncTools:** Manages asynchronous task cleanup when exiting Play Mode. Resets the synchronization context to ensure async operations do not persist incorrectly between sessions.
- **BeforePlayMode:** Configures specific behaviors before entering Play Mode. Includes a BootSceneModule to load a specific scene automatically based on Build Settings indices.
- **DoTweenTools:** Handles DOTween cleanup when exiting Play Mode. Clears cached tweens and resets the tweening engine to prevent state leakage into the next session.

### Editor Windows and Tools

- **MergeObjectsWindow:** Provides an interface for copying components from one GameObject to another. Supports selecting specific component types to merge and options to destroy the source object after merging.
- **MergeScriptsWindow:** Provides an interface for configuring and executing script merge operations with progress tracking. Supports handlers for changing Assembly, GUID, and Namespace pairs across prefab and scene assets.
- **MergeScripts Handlers:** Core logic for processing script changes. Includes validators to ensure data integrity and prevent invalid replacements during asynchronous text replacement processes.

### Grid Snapping

Adaptive grid snapping system for precise object placement in the editor.

- **GridSnappingAdaptive:** Listens to object change events and automatically snaps selected objects to the grid based on active snap settings and key modifiers.
- **GridSnapping Settings:** Manages persistent storage for grid snapping settings and provides a custom Project Settings provider for enabling or disabling the adaptive snapping system.

### Odin Inspector Extensions

Custom drawers and GUI utilities for Odin Inspector.

- **DateTimeDrawer:** Custom Odin drawer for DateTime structs. Displays individual fields for year, month, day, hour, minute, and second for precise editing.
- **TimeSpanDrawer:** Custom Odin drawer for TimeSpan structs. Displays individual fields for days, hours, minutes, and seconds.
- **OdinGUI:** Provides helper methods for drawing custom GUI elements within Odin inspectors, such as inline fields with suffix labels.

### Shortcuts and Utilities

- **Shortcuts:** Menu items for common tasks including asset reserialization, toggling window maximization, disabling shadows on selected objects, and marking assets as dirty to force reload.
- **TexturesCompressor:** Validation and settings for texture compression standards. Includes an AssetImporterValidator for Odin rules, TextureImporter extensions for simplifying common tasks, and settings to enforce maximum sizes and preferred compression formats.
- **Utilities:** General purpose helper classes including AssetsUtility for database interactions, GUIDrawUtility for editor GUI elements, ProjectUtility for unique identification, RectTransformBakeUtility for baking scale information, and ReflectionUtility for field manipulation.

---

## Runtime

The Runtime module provides essential tools for asynchronous operations, debugging, navigation, math calculations, and object management without heavy dependencies. It is optimized for performance and ease of use in final builds.

### Core Utilities

- **AsyncUtility:** Provides a comprehensive suite of asynchronous methods built on top of UniTask. Includes functionality for delayed execution, frame waiting, and input detection. Supports cancellation tokens and offers specific delays for scaled, unscaled, and realtime modes.
- **DebugUtility:** Contains extensive debugging helpers for logging and visualization. Supports logging values with custom names, drawing bounding boxes and paths in the Scene view, and conditional logging methods.
- **IDUtility:** Manages the generation of persistent unique identifiers. Supports generating single or multiple unique IDs that persist across sessions using PlayerPrefs or Odin Serialization.
- **NavigationUtility:** Contains advanced helpers for NavMesh operations. Includes methods to calculate paths, sample positions on the mesh, and find random positions near a target or within a range.
- **ReflectionUtility:** Provides reflection-based helpers to access private and public fields and properties. Includes methods to get and set values dynamically by name and supports retrieving subtypes of a given class.
- **TimerUtility:** Manages countdown timers using asynchronous tasks. Supports callbacks for updating remaining time and handles offline time progression based on stored timestamps.
- **Math & Vector Utilities:** Extended mathematical functions beyond standard Unity Mathf, including percentage calculations, approximate equality checks, and Vector3 operations for XZ plane distances and directional vectors.

### UI Components

Extended UI components for sliders, scrolls, layouts, and effects.

- **Sliders:** DoubleSlider and TripleSlider components support multiple independent value handles with configurable direction and constraints. Updates anchor positions to visualize selected ranges.
- **Scroll Views:** ScrollCardsGroup manages carousel-style views with scaling and positioning. ScrollElementsList tracks specific elements for precise scrolling. TrackedScrollRect exposes dragging state.
- **Layout:** ContentSizeFitterAdaptive adjusts RectTransform size based on content. GridLayoutGroupAdaptive supports fixed column or row counts with flexible constraints.
- **Effects:** ButtonEffect adds visual feedback with scale animations. ColorRendererAnimation animates renderer color over time. UIWindowEffects provides fade in/out effects for CanvasGroups.
- **Safe Area:** SafeAreaAnchors and SafeAreaResize adjust RectTransform position and size to respect device safe areas, supporting separate configurations for portrait and landscape orientations.
- **Miscellaneous:** Components include MaskInverted for inverted masking, ParticleSystemRoot for managing child particles, and StaticRootComponent for runtime mesh batching.

### Extensions

Extensive extension methods for Unity API and custom components.

- **Unity Types:** Extensions for GameObject, Transform, RectTransform, Vector2, Vector3, Color, Quaternion, Animator, AudioSource, Camera, CanvasGroup, Image, Renderer, and ParticleSystem.
- **Collections:** Extensions for Arrays and Lists providing shuffling, random element selection, and subset retrieval.
- **UI:** Extensions for Button, ScrollRect, and TextMeshProUGUI allowing batch operations and state toggling.
- **Async & Coroutines:** Extensions for UniTask operations with callback support and Coroutine management for safe stopping and restarting.
- **Mesh & Physics:** Extensions for Mesh and MeshFilter including bounding corner calculations and custom raycasting with texture color sampling.

### Network and Time

- **TimeService:** Main service responsible for synchronizing local time with network time. Manages a list of time providers and attempts to fetch time sequentially until success. Persists the time offset between sessions.
- **Providers:** Includes implementations for AWS, Cloudflare, DuckDuckGo, Google, TimeAPI, and generic Header-based providers. Each implements the ITimeProvider interface for asynchronous time retrieval.
- **ScreenOrientation:** Utility for monitoring screen orientation changes. Provides events for orientation changes, portrait entry, and landscape entry. OrientationBranch component toggles child GameObjects based on current orientation.

### Smart Links and States

- **Smart Links:** Abstract base class for managing prefab references. HardLink ensures instance is created once and persists. SoftLink requires an external root transform for instantiation. Supports lazy initialization.
- **State Machine:** Interfaces for IState and IStateMachine. StateExtension provides safe transition logic, handling exit methods before entering new states.

### Custom Types and Serialization

- **Serializable Wrappers:** DateTimeLink and TimeSpanLink allow editing of system structs in the Inspector with automatic range validation. RectLink wraps Unity Rect structures.
- **Ranges:** FloatMinMax and IntMinMax structures for defining value ranges with random retrieval and clamping methods.
- **Serialization:** FlexibleNameResolver for JSON contract resolution supporting leading underscores. SirenixJson helpers for Odin serialization.
- **Custom Structures:** Includes MeshCorners for bounding data, MeshHit for raycast data, Triangle3 for 3D grid calculations, and ResizeData for camera scaling.

### Validation and Vibration

- **Validation:** Editor utilities for validating and fixing missing components. Includes helpers for generating type dropdowns in inspectors and validating resource existence.
- **Vibration:** Static class managing haptic feedback on mobile devices. Supports amplitude control and waveform vibrations on Android API 26 and above, with fallbacks for iOS and older Android versions.
- **Logging:** FileLog handles writing log data directly to files within the persistent data path, supporting initialization with headers and safe appending.