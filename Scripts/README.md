# TinyUtilities.Runtime

A lightweight collection of utilities, extensions, and helper classes for Unity Runtime.  
It provides essential tools for asynchronous operations, debugging, navigation, math calculations, and object management without heavy dependencies.

**Key Features:**

- Unified asynchronous task management using UniTask
- Advanced debugging tools for visualizing bounds and logging values
- Navigation Mesh helpers for pathfinding and random position sampling
- UI layout baking utilities for RectTransform scaling
- Persistent unique ID generation and file logging
- Reflection helpers for field and property manipulation
- Math and Vector extensions for common calculations
- Editor-integrated asset and object management tools
- Collision detection and correction utilities
- Extended UI components for sliders, scrolls, and layouts
- Serializable custom types for common Unity and System structures
- Extensive extension methods for Unity API and custom components
- Network time synchronization with multiple fallback providers
- Device safe area handling for UI layout adjustments
- Screen orientation monitoring and response utilities
- Flexible JSON serialization resolvers
- Smart prefab linking and instantiation patterns
- Basic state machine implementation helpers
- UI animation effects integrated with DOTween
- Editor validation tools for resources and components
- Cross-platform haptic feedback management

## Table of Contents

- [Core](#core)
    - [`AssetsUtility`](#assetsutility)
    - [`AsyncUtility`](#asyncutility)
    - [`Coroutines`](#coroutines)
    - [`DebugUtility`](#debugutility)
    - [`DoTweenUtility`](#dotweenutility)
    - [`FileLog`](#filelog)
    - [`GameObjectUtility`](#gameobjectutility)
    - [`ICoroutineRunner`](#icoroutinerunner)
    - [`IDUtility`](#idutility)
    - [`InspectorNames`](#inspectornames)
    - [`MathfUtility`](#mathfutility)
    - [`MonoUtility`](#monoutility)
    - [`NavigationUtility`](#navigationutility)
    - [`QuaternionUtility`](#quaternionutility)
    - [`RandomUtility`](#randomutility)
    - [`RectTransformBakeUtility`](#recttransformbakeutility)
    - [`ReflectionUtility`](#reflectionutility)
    - [`ScenesUtility`](#scenesutility)
    - [`SirenixJson`](#sirenixjson)
    - [`TimerUtility`](#timerutility)
    - [`Vector3Utility`](#vector3utility)
- [Collisions](#collisions)
    - [`CollisionUtility`](#collisionutility)
    - [`IColliderData`](#icolliderdata)
- [Components](#components)
    - [`ADSTimeScaleFix`](#adstimescalefix)
    - [`ButtonEffect`](#buttoneffect)
    - [`ButtonToggle`](#buttontoggle)
    - [`ColorRendererAnimation`](#colorrendereranimation)
    - [`ContentSizeFitterAdaptive`](#contentsizefitteradaptive)
    - [`DontDestroyOnLoad`](#dontdestroyonload)
    - [`DoubleSlider`](#doubleslider)
    - [`GridLayoutGroupAdaptive`](#gridlayoutgroupadaptive)
    - [`MaskInverted`](#maskinverted)
    - [`ParticleSystemRoot`](#particlesystemroot)
    - [`RectTransformSync`](#recttransformsync)
    - [`RotationFixed`](#rotationfixed)
    - [`ScaleFixed`](#scalefixed)
    - [`ScrollButtonMove`](#scrollbuttonmove)
    - [`ScrollButtonMoveBack`](#scrollbuttonmoveback)
    - [`ScrollButtonMoveNext`](#scrollbuttonmovenext)
    - [`ScrollCardsGroup`](#scrollcardsgroup)
    - [`ScrollElementsList`](#scrollelementslist)
    - [`SizeConstraint`](#sizeconstraint)
    - [`StaticName`](#staticname)
    - [`StaticRootComponent`](#staticrootcomponent)
    - [`TakeScreenshot`](#takescreenshot)
    - [`TrackedScrollRect`](#trackedscrollrect)
    - [`TripleSlider`](#tripleslider)
    - [`UIRebuilder`](#uirebuilder)
- [CustomTypes](#customtypes)
    - [`DateTimeLink`](#datetimelink)
    - [`EnumName`](#enumname)
    - [`FloatMinMax`](#floatminmax)
    - [`I2LocTerm`](#i2locterm)
    - [`IntMinMax`](#intminmax)
    - [`MeshCorners`](#meshcorners)
    - [`MeshHit`](#meshhit)
    - [`RectLink`](#rectlink)
    - [`ResizeData`](#resizedata)
    - [`TimeSpanLink`](#timespanlink)
    - [`Triangle3`](#triangle3)
- [Extensions](#extensions)
    - [`AnimatorExtension`](#animatorextension)
    - [`ArrayExtension`](#arrayextension)
    - [`AsyncExtension`](#asyncextension)
    - [`AudioMixerExtension`](#audiomixerextension)
    - [`AudioSourceExtension`](#audiosourceextension)
    - [`ButtonExtension`](#buttonextension)
    - [`CameraExtension`](#cameraextension)
    - [`CanvasGroupExtension`](#canvasgroupextension)
    - [`ColorExtension`](#colorextension)
    - [`CoroutineExtension`](#coroutineextension)
    - [`DoTweenDoubleSliderExtension`](#dotweendoublesliderextension)
    - [`DoTweenExtension`](#dotweenextension)
    - [`DoTweenTripleSliderExtension`](#dotweentriplesliderextension)
    - [`EnumNameExtensions`](#enumnameextensions)
    - [`GameObjectExtension`](#gameobjectextension)
    - [`ImageExtension`](#imageextension)
    - [`ListExtension`](#listextension)
    - [`LocalizeExtension`](#localizeextension)
    - [`MeshExtension`](#meshextension)
    - [`MeshFilterExtension`](#meshfilterextension)
    - [`MonoBehaviourExtension`](#monobehaviourextension)
    - [`ObjectExtension`](#objectextension)
    - [`ParticleSystemExtension`](#particlesystemextension)
    - [`RectTransformExtension`](#recttransformextension)
    - [`RendererExtension`](#rendererextension)
    - [`ResourcesExtension`](#resourcesextension)
    - [`ScrollRectExtension`](#scrollrectextension)
    - [`TextMeshProUGUIExtension`](#textmeshprouguiextension)
    - [`TransformExtension`](#transformextension)
    - [`UnityEventExtension`](#unityeventextension)
    - [`Vector2Extension`](#vector2extension)
    - [`Vector3Extension`](#vector3extension)
- [NetworkTime](#networktime)
    - [`TimeResult`](#timeresult)
    - [`TimeService`](#timeservice)
    - [`TimeServicePrefs`](#timeserviceprefs)
    - [Providers](#providers)
        - [`AwsHeaderTimeProvider`](#awsheadertimeprovider)
        - [`CloudflareHeaderTimeProvider`](#cloudflareheadertimeprovider)
        - [`DuckDuckGoHeaderTimeProvider`](#duckduckgoheadertimeprovider)
        - [`GoogleHeaderTimeProvider`](#googleheadertimeprovider)
        - [`ITimeProvider`](#itimeprovider)
        - [`TimeAPIProvider`](#timeapiprovider)
- [SafeArea](#safearea)
    - [`SafeAreaAnchors`](#safeareaanchors)
    - [`SafeAreaResize`](#safearearesize)
    - [`SafeAreaUtility`](#safeareautility)
- [ScreenOrientation](#screenorientation)
    - [`OrientationBranch`](#orientationbranch)
    - [`ScreenOrientationUtility`](#screenorientationutility)
- [Serialization](#serialization)
    - [`FlexibleNameResolver`](#flexiblenameresolver)
- [SmartLinks](#smartlinks)
    - [`HardLink`](#hardlink)
    - [`SmartLink`](#smartlink)
    - [`SoftLink`](#softlink)
- [States](#states)
    - [`IState`](#istate)
    - [`IStateMachine`](#istatemachine)
    - [`StateExtension`](#stateextension)
- [UIEffects](#uieffects)
    - [`UIButtonEffects`](#uibuttoneffects)
    - [`UIWindowEffects`](#uiwindoweffects)
- [Validation](#validation)
    - [`FixExtension`](#fixextension)
    - [`InspectorUtility`](#inspectorutility)
    - [`ResourcesDebug`](#resourcesdebug)
    - [`ResourcesValidate`](#resourcesvalidate)
    - [`ValidateExtension`](#validateextension)
    - [`ValidateMessage`](#validatemessage)
- [Vibration](#vibration)
    - [`PredefinedEffect`](#predefinedeffect)
    - [`VibrationEffect`](#vibrationeffect)

## Core

### [`AssetsUtility`](AssetsUtility.cs)

Provides utilities for interacting with the Unity Asset Database.
Includes functionality to load assets of a specific type from a given path within the editor.
Offers methods to generate lists of prefabs for editor dropdowns, with options to process naming conventions by splitting strings.

### [`AsyncUtility`](AsyncUtility.cs)

Provides a comprehensive suite of asynchronous methods built on top of UniTask.
Includes functionality for delayed execution, frame waiting, and input detection.
Supports cancellation tokens and offers specific delays for scaled, unscaled, and realtime modes.
Contains logic for waiting for user input gestures such as clicks and moves.
Manages a global cancellation token source that resets on subsystem registration.

### [`Coroutines`](Coroutines.cs)

Offers standard Unity Coroutine helpers for common timing patterns.
Includes methods to execute actions after a specific delay, after realtime delay, or after the current frame ends.
Provides utility to wait for multiple coroutines to complete sequentially.

### [`DebugUtility`](DebugUtility.cs)

Contains extensive debugging helpers for logging and visualization.
Supports logging values with custom names and contexts.
Provides methods to draw bounding boxes and paths in the Scene view using debug lines.
Includes conditional logging methods and stack trace markers for error tracking.

### [`DoTweenUtility`](DoTweenUtility.cs)

Contains integration helpers for the DOTween library.
Provides methods to create tweens for float values using getter and setter functions.
Handles target management for tweens created via dynamic access.
Requires the DOTWEEN scripting define symbol.

### [`FileLog`](FileLog.cs)

Handles writing log data directly to files within the persistent data path.
Supports initializing log files with header labels.
Provides methods to append string values to the log file safely, handling exceptions internally.

### [`GameObjectUtility`](GameObjectUtility.cs)

Offers helpers for instantiating GameObjects and MonoBehaviours.
Supports instantiation of prefabs under a specific parent.
Includes editor-specific functionality to find objects within the current Prefab stage.

### [`ICoroutineRunner`](ICoroutineRunner.cs)

Defines an interface for managing coroutine lifecycle.
Specifies methods for starting and stopping coroutines, allowing for dependency injection of coroutine management capabilities into other classes.

### [`IDUtility`](IDUtility.cs)

Manages the generation of persistent unique identifiers.
Supports generating single or multiple unique IDs that persist across sessions using PlayerPrefs or Odin Serialization depending on availability.
Includes methods to retrieve IDs as formatted strings.

### [`InspectorNames`](InspectorNames.cs)

Defines a set of constant string values used for labeling inspector fields and groups.
Provides standardized names for common categories such as Model, Links, Parameters, and Debug to ensure consistency across the project.

### [`MathfUtility`](MathfUtility.cs)

Provides extended mathematical functions beyond the standard Unity Mathf library.
Includes methods for calculating percentages, applying percentages to values, and checking approximate equality across multiple values.
Offers custom rounding logic and integer comparison helpers.

### [`MonoUtility`](MonoUtility.cs)

Provides specialized instantiation methods for MonoBehaviours.
Handles differences between runtime and editor prefab instantiation to ensure correct behavior in both contexts.
Supports setting position and rotation during instantiation.

### [`NavigationUtility`](NavigationUtility.cs)

Contains advanced helpers for NavMesh operations.
Includes methods to calculate paths, sample positions on the mesh, and find random positions near a target or within a range.
Supports filtering by agent type and area mask. Provides logic to find nearest waypoints along a calculated path.

### [`QuaternionUtility`](QuaternionUtility.cs)

Offers utilities for rotation calculations.
Includes methods to create rotations looking along a specific direction while locking the Y-axis.
Supports normalized rotation outputs for consistent orientation handling.

### [`RandomUtility`](RandomUtility.cs)

Provides helpers for random value generation. Includes methods to select random items from an array.
Offers functionality to generate random offsets around a base value using absolute or percentage-based ranges.

### [`RectTransformBakeUtility`](RectTransformBakeUtility.cs)

Provides methods to bake scale transformations into RectTransform components.
Adjusts offsets, sizes, and component properties such as Image, Text, and Layout Groups to maintain visual consistency after removing scale.
Supports recursive baking for child elements.

### [`ReflectionUtility`](ReflectionUtility.cs)

Provides reflection-based helpers to access private and public fields and properties.
Includes methods to get and set values dynamically by name.
Automatically marks Unity objects as dirty when modifications are made during editor mode.
Supports retrieving subtypes of a given class.

### [`ScenesUtility`](ScenesUtility.cs)

Contains tools for scene management and object creation.
Includes methods to retrieve scene names by build index.
Provides helpers to create new GameObjects with specific hierarchy positioning and to gather components across the active scene hierarchy.

### [`SirenixJson`](SirenixJson.cs)

Offers serialization helpers specifically for Odin Inspector.
Provides methods to serialize objects to JSON strings and deserialize JSON strings back to objects using Odin's serialization utility.
Requires the ODIN_INSPECTOR scripting define symbol.

### [`TimerUtility`](TimerUtility.cs)

Manages countdown timers using asynchronous tasks.
Supports callbacks for updating remaining time and completing the timer.
Includes functionality for calculating time remaining until the next day and handling offline time progression based on stored timestamps.

### [`Vector3Utility`](Vector3Utility.cs)

Contains helper methods for Vector3 operations.
Includes functionality to calculate distances on the XZ plane, average multiple vectors, and generate directional vectors based on angles.
Provides methods to extract XZ directions from positions or velocities.

## Collisions

### [`CollisionUtility`](Collisions/CollisionUtility.cs)

Provides static methods for handling collision detection and position correction using raycasting.
Includes functionality to correct horizontal movement by casting rays in a circle around the object to detect obstacles.
Calculates offset positions based on raycast hits to prevent clipping.
Handles vertical collision correction by raycasting downwards to determine ground height.
Contains debug visualization tools for rays when used in the Unity Editor.

### [`IColliderData`](Collisions/IColliderData.cs)

Defines an interface for objects that participate in custom collision calculations.
Requires implementing properties for current position, velocity vector, and collider radius.
Used by CollisionUtility to retrieve necessary data for physics corrections without direct dependency on specific Collider components.

## Components

### [`ADSTimeScaleFix`](Components/ADSTimeScaleFix.cs)

Ensures that Unity's Time.timeScale is restored to its original value when the component is destroyed.
Useful for scenarios where time scale might be modified temporarily, such as during advertisement playback, to prevent permanent slowdowns.

### [`ButtonEffect`](Components/ButtonEffect.cs)

Adds visual feedback to UI Buttons using scale animations and bouncing effects.
Supports configuration for press and release scales, easing types, and bounce behavior.
Integrates with DOTween for smooth animations if the library is available.
Includes options for randomizing bounce timing and setting custom start scales.

### [`ButtonToggle`](Components/ButtonToggle.cs)

Creates a toggle button that switches between active and inactive states represented by different GameObjects.
Invokes an event when the state changes.
Includes editor utilities for resetting references and testing toggle functionality outside of play mode.

### [`ColorRendererAnimation`](Components/ColorRendererAnimation.cs)

Animates the color property of a Renderer component over time.
Supports looping, custom durations, and ignoring time scale.
Integrates with DOTween for interpolation.
Includes validation logic to ensure the correct shader is assigned to the material.

### [`ContentSizeFitterAdaptive`](Components/ContentSizeFitterAdaptive.cs)

A layout component that adjusts the size of a RectTransform based on its content's preferred or minimum size.
Offers flexible fitting modes for horizontal and vertical axes.
Implements Unity's layout controller interface to integrate with the UI system.

### [`DontDestroyOnLoad`](Components/DontDestroyOnLoad.cs)

Persists the GameObject across scene loads by calling DontDestroyOnLoad on start.
Provides a simple component-based approach to object persistence without additional setup.

### [`DoubleSlider`](Components/DoubleSlider.cs)

A UI slider component that supports two independent values handles.
Allows configuration of direction, size, and whole number constraints.
Updates anchor positions of child RectTransforms to visualize the selected range.
Invokes events when values change and supports animation property changes.

### [`GridLayoutGroupAdaptive`](Components/GridLayoutGroupAdaptive.cs)

An enhanced version of Unity's GridLayoutGroup with adaptive layout capabilities.
Supports fixed column or row counts, as well as flexible constraints.
Includes logic for centering adaptation and handling spacing and padding correctly.
Calculates layout input for both horizontal and vertical axes based on child count.

### [`MaskInverted`](Components/MaskInverted.cs)

A UI mask component that inverts the masking effect, hiding content inside the mask shape instead of outside.
Implements material modification using stencil buffers to achieve the inverted effect.
Supports masking graphics and updates child materials accordingly.

### [`ParticleSystemRoot`](Components/ParticleSystemRoot.cs)

Manages multiple ParticleSystem components located in child objects.
Provides methods to play all particles simultaneously or configure their play-on-awake state.
Includes editor utilities to automatically gather child particle systems.

### [`RectTransformSync`](Components/RectTransformSync.cs)

Synchronizes RectTransform properties between two different objects.
Copies anchored position, anchors, offsets, and size delta from a source to a target.
Executes synchronization after frame ends to ensure layout calculations are complete.

### [`RotationFixed`](Components/RotationFixed.cs)

Locks the rotation of a Transform to its initial value every frame.
Prevents external forces or scripts from altering the object's orientation during runtime.

### [`ScaleFixed`](Components/ScaleFixed.cs)

Locks the scale of a Transform to its initial lossy scale every frame.
Prevents external forces or scripts from altering the object's size during runtime.
Uses extension methods to apply lossy scale corrections.

### [`ScrollButtonMove`](Components/ScrollButtonMove.cs)

An abstract base class for buttons that control scroll view navigation.
Manages active and inactive states based on scroll position limits.
Invokes events when the button becomes active or inactive.
Requires implementation of click behavior and active state checks in derived classes.

### [`ScrollButtonMoveBack`](Components/ScrollButtonMoveBack.cs)

A concrete implementation of ScrollButtonMove for navigating to the previous element in a list.
Decrements the current element index on click.
Determines active state based on whether the first element is currently visible.

### [`ScrollButtonMoveNext`](Components/ScrollButtonMoveNext.cs)

A concrete implementation of ScrollButtonMove for navigating to the next element in a list.
Increments the current element index on click.
Determines active state based on whether the last element is currently visible.

### [`ScrollCardsGroup`](Components/ScrollCardsGroup.cs)

Manages a carousel-style scroll view where cards scale and position based on selection.
Supports opening and closing animations with configurable durations.
Handles card reordering to ensure the selected card is rendered on top.
Includes validation for ScrollRect content setup and orientation.

### [`ScrollElementsList`](Components/ScrollElementsList.cs)

An advanced scroll view component that tracks specific elements within a list.
Calculates positions for elements to enable precise scrolling to specific indices.
Supports inverted orientations and padding skipping.
Integrates with DOTween for animated scrolling and handles drag events to update current element tracking.

### [`SizeConstraint`](Components/SizeConstraint.cs)

Adjusts the scale of a GameObject based on its distance from a specific camera.
Maintains a consistent screen size for the object regardless of camera distance.
Uses custom resize data to calculate the appropriate scale factor every frame.

### [`StaticName`](Components/StaticName.cs)

Enforces a specific name for the GameObject it is attached to.
Validates the name in the editor and provides a fix to reset it if changed.
Useful for ensuring consistent naming conventions across scenes and prefabs.

### [`StaticRootComponent`](Components/StaticRootComponent.cs)

Combines child meshes into a single mesh for static batching at runtime.
Improves rendering performance by reducing draw calls for static geometry.
Executes batching logic on start.

### [`TakeScreenshot`](Components/TakeScreenshot.cs)

Captures a screenshot when a specific keyboard combination is pressed.
Saves the image to a predefined path with a unique filename.
Operates during the update loop to detect input.

### [`TrackedScrollRect`](Components/TrackedScrollRect.cs)

Extends the standard Unity ScrollRect to expose dragging state.
Provides a boolean property indicating whether the user is currently dragging the scroll view.
Overrides begin and end drag events to update the state.

### [`TripleSlider`](Components/TripleSlider.cs)

A UI slider component that supports three independent value handles.
Allows configuration of direction, size, and whole number constraints.
Updates anchor positions of three child RectTransforms to visualize the selected ranges.
Invokes events when any of the three values change.

### [`UIRebuilder`](Components/UIRebuilder.cs)

Provides methods to force immediate or marked layout rebuilds for a RectTransform.
Useful for resolving UI layout issues where changes are not immediately reflected.
Wraps Unity's LayoutRebuilder functionality for easier access.

## CustomTypes

### [`DateTimeLink`](CustomTypes/DateTimeLink.cs)

Serializable wrapper for the System DateTime structure.
Allows separate editing of year, month, day, hour, minute, and second components in the Inspector with automatic range validation.
Supports implicit conversion to standard DateTime and provides equality comparison methods.
Includes editor-specific clamping to ensure valid date values.

### [`EnumName`](CustomTypes/EnumName.cs)

Utility for serializing Enum values as strings to maintain stability across code changes.
Provides a generic wrapper class to maintain enum type safety while storing data internally as a string.
Includes a static factory method for easy instantiation.
Supports equality comparison against both wrapper instances and raw enum values.

### [`FloatMinMax`](CustomTypes/FloatMinMax.cs)

Serializable structure for defining a range of float values.
Provides methods to retrieve a random value within the defined range or clamp a value to the bounds.
Includes editor validation to ensure the minimum value is not greater than the maximum value.
Supports Odin Inspector attributes for improved visualization.

### [`I2LocTerm`](CustomTypes/I2LocTerm.cs)

Wrapper for I2 Localization terms to facilitate editor integration.
Allows selecting localization terms via a dropdown in the Inspector and previews the translated value during edit time.
Supports implicit conversion to string for easy usage in code.
Includes equality checks and hash code generation for collection usage.

### [`IntMinMax`](CustomTypes/IntMinMax.cs)

Serializable structure for defining a range of integer values.
Similar functionality to FloatMinMax but tailored for integers.
Provides methods to retrieve a random integer within the range or clamp values.
Includes editor validation to maintain logical min-max order.

### [`MeshCorners`](CustomTypes/MeshCorners.cs)

Represents the bounding corners of a mesh using min and max Vector3 coordinates.
Provides properties for calculating size along individual axes and the maximum size.
Includes operator overloads for mathematical transformations such as addition, subtraction, multiplication, and division.
Calculates the center point of the bounding box.

### [`MeshHit`](CustomTypes/MeshHit.cs)

Readonly structure storing information about a raycast hit on a mesh.
Contains the hit position in 3D space and the color sampled from the texture at that point.
Used primarily by MeshFilter extensions for detailed collision data.

### [`RectLink`](CustomTypes/RectLink.cs)

Serializable wrapper for Unity Rect structures.
Allows individual editing of x, y, width, and height properties in the Inspector.
Supports implicit conversion to Rect for seamless integration with Unity UI APIs.
Includes equality comparison methods for both wrapper and standard Rect types.

### [`ResizeData`](CustomTypes/ResizeData.cs)

Data container for camera-based scaling calculations.
Stores target size, reference distance, and field of view values required for dynamic object scaling.
Used in conjunction with Camera extensions to maintain visual consistency across distances.

### [`TimeSpanLink`](CustomTypes/TimeSpanLink.cs)

Serializable wrapper for the System TimeSpan structure.
Allows editing days, hours, minutes, and seconds separately in the Inspector.
Supports JSON serialization attributes for external data storage.
Includes implicit conversion to TimeSpan and equality comparison methods.

### [`Triangle3`](CustomTypes/Triangle3.cs)

Represents a triangle in 3D space defined by three Vector3 vertices.
Provides functionality to calculate integer grid points located inside the triangle projection on the XZ plane.
Uses barycentric coordinates to determine point inclusion.

## Extensions

### [`AnimatorExtension`](Extensions/AnimatorExtension.cs)

Extensions for the Animator component.
Includes methods to check for the existence of parameters without generating console warnings.
Optimizes parameter lookup by caching or iterating efficiently.

### [`ArrayExtension`](Extensions/ArrayExtension.cs)

Extensions for Arrays.
Provides methods for selecting random elements, shuffling array order, and selecting subsets of elements.
Supports generic types and includes overloads for retrieving indices alongside values.

### [`AsyncExtension`](Extensions/AsyncExtension.cs)

Extensions for UniTask asynchronous operations.
Adds callback support to execute actions immediately upon task completion.
Simplifies chaining logic for async workflows.

### [`AudioMixerExtension`](Extensions/AudioMixerExtension.cs)

Extensions for AudioMixer.
Simplifies transitioning to specific snapshots by name rather than reference.
Handles weight and transition time parameters for smooth audio mixing.

### [`AudioSourceExtension`](Extensions/AudioSourceExtension.cs)

Extensions for AudioSource.
Allows setting pitch and volume with random ranges or specific values via method chaining.
Returns the AudioSource instance to facilitate fluent interface patterns.

### [`ButtonExtension`](Extensions/ButtonExtension.cs)

Extensions for UI Buttons and collections of Buttons.
Allows toggling interactable state for single or multiple buttons efficiently.
Supports generic enumeration of Button components for batch operations.

### [`CameraExtension`](Extensions/CameraExtension.cs)

Extensions for Camera.
Includes calculations for object resizing based on distance and field of view using ResizeData.
Provides pure functions for determining scale factors to maintain screen-space size.

### [`CanvasGroupExtension`](Extensions/CanvasGroupExtension.cs)

Extensions for CanvasGroup collections.
Allows setting alpha transparency for multiple groups simultaneously.
Returns the collection to support method chaining.

### [`ColorExtension`](Extensions/ColorExtension.cs)

Extensions for Color.
Converts Color structures to hexadecimal string representation.
Handles clamping of color channels to ensure valid output.

### [`CoroutineExtension`](Extensions/CoroutineExtension.cs)

Extensions for Coroutine management.
Provides methods to stop or restart coroutines safely on components implementing ICoroutineRunner.
Handles null checks to prevent errors during lifecycle management.

### [`DoTweenDoubleSliderExtension`](Extensions/DoTweenDoubleSliderExtension.cs)

DOTween integration for DoubleSlider component.
Enables tweening of first and second values independently.
Requires the DOTWEEN scripting define symbol.

### [`DoTweenExtension`](Extensions/DoTweenExtension.cs)

General DOTween extensions for various Unity components.
Supports tweening Slider max value, AudioSource volume, Animator float parameters, custom color renderers, and Transform rotation axes.
Includes batch kill functionality for collections of components.

### [`DoTweenTripleSliderExtension`](Extensions/DoTweenTripleSliderExtension.cs)

DOTween integration for TripleSlider component.
Enables tweening of all three slider values independently.
Requires the DOTWEEN scripting define symbol.

### [`EnumNameExtensions`](Extensions/EnumNameExtensions.cs)

Extensions for EnumName custom type.
Checks if a specific enum value exists within a collection of EnumName wrappers.
Facilitates validation of enum states in serialized collections.

### [`GameObjectExtension`](Extensions/GameObjectExtension.cs)

Extensions for GameObject.
Includes methods for scaling, delayed destruction, finding components, managing active state, layer assignment, and child retrieval.
Provides editor-specific utilities for checking scene asset status.

### [`ImageExtension`](Extensions/ImageExtension.cs)

Extensions for UI Image.
Allows setting sprites on collections of images simultaneously.
Supports generic enumeration for flexible usage.

### [`ListExtension`](Extensions/ListExtension.cs)

Extensions for Lists.
Provides shuffling and random element selection similar to ArrayExtension.
Operates directly on List generic types.

### [`LocalizeExtension`](Extensions/LocalizeExtension.cs)

Extensions for I2 Localization components.
Allows setting localization terms on arrays of Localize components.
Supports primary and secondary term assignment.
Requires the I2_LOCALIZE scripting define symbol.

### [`MeshExtension`](Extensions/MeshExtension.cs)

Extensions for Mesh.
Calculates bounding corners and center points for single or multiple meshes.
Iterates through vertices to determine extents accurately.

### [`MeshFilterExtension`](Extensions/MeshFilterExtension.cs)

Extensions for MeshFilter.
Includes mesh data uploading, center calculation, and custom raycasting against mesh geometry with texture color sampling.
Provides detailed hit information including UV-based color retrieval.

### [`MonoBehaviourExtension`](Extensions/MonoBehaviourExtension.cs)

Extensions for MonoBehaviour collections.
Retrieves specific components from a collection of MonoBehaviours.
Filters out null results automatically.

### [`ObjectExtension`](Extensions/ObjectExtension.cs)

Extensions for Unity Object.
Marks objects as dirty in the editor for serialization updates.
Conditional compilation ensures code only runs in editor mode.

### [`ParticleSystemExtension`](Extensions/ParticleSystemExtension.cs)

Extensions for ParticleSystem.
Includes instantiation, scaling, delayed destruction, batch play/stop/clear, and manual particle bursting with shape support.
Handles complex shape modules for accurate particle positioning.

### [`RectTransformExtension`](Extensions/RectTransformExtension.cs)

Extensions for RectTransform.
Calculates height, expands to fullscreen, and manages parent assignment for collections.
Forces layout updates to ensure immediate visual changes.

### [`RendererExtension`](Extensions/RendererExtension.cs)

Extensions for Renderer.
Allows enabling/disabling or setting materials on collections of renderers.
Supports both arrays and lists for flexibility.

### [`ResourcesExtension`](Extensions/ResourcesExtension.cs)

Extensions for Resources loading.
Loads assets only if the current reference is null to optimize performance.
Accepts a load function for lazy loading logic.

### [`ScrollRectExtension`](Extensions/ScrollRectExtension.cs)

Extensions for ScrollRect.
Moves scroll view to a specific element index vertically based on cell size.
Calculates anchored position adjustments automatically.

### [`TextMeshProUGUIExtension`](Extensions/TextMeshProUGUIExtension.cs)

Extensions for TextMeshProUGUI.
Sets text on collections of TMP components simultaneously.
Optimizes batch text updates for UI lists.

### [`TransformExtension`](Extensions/TransformExtension.cs)

Extensions for Transform.
Extensive utilities for scaling, destruction, finding children, resetting properties, managing hierarchy, and coordinate calculations.
Includes methods for lossy scale adjustment and recursive child retrieval.

### [`UnityEventExtension`](Extensions/UnityEventExtension.cs)

Extensions for UnityEvent.
Checks if a specific method on a target object is already registered as a listener.
Prevents duplicate event subscriptions.

### [`Vector2Extension`](Extensions/Vector2Extension.cs)

Extensions for Vector2.
Calculates average position from a list or array of vectors.
Returns success status based on collection emptiness.

### [`Vector3Extension`](Extensions/Vector3Extension.cs)

Extensions for Vector3.
Extensive math utilities including absolute values, conversion to int, axis offsets, rotation, median calculations, and point filtering based on linecasts or proximity.
Includes methods for component-wise multiplication and division with zero-division protection.

## NetworkTime

### [`TimeResult`](NetworkTime/TimeResult.cs)

Encapsulates the result of a network time synchronization request.
Contains the retrieved DateTime value and a boolean flag indicating the success of the operation.
Used internally by providers to return standardized time data to the TimeService.

### [`TimeService`](NetworkTime/TimeService.cs)

Main service responsible for synchronizing local time with network time.
Manages a list of time providers and attempts to fetch time sequentially until success.
Handles initialization logic, offset calculation, and caching of the network time.
Provides asynchronous methods to sync time and retrieve the current network-adjusted time.
Persists the time offset between sessions to maintain accuracy without constant network requests.

### [`TimeServicePrefs`](NetworkTime/TimeServicePrefs.cs)

Handles the persistence of the time offset using Unity PlayerPrefs.
Checks for existing offset data and loads or saves the hour difference between local and network time.
Ensures that the time correction remains consistent across application restarts.

### Providers

#### [`AwsHeaderTimeProvider`](NetworkTime/Providers/AwsHeaderTimeProvider.cs)

Implementation of ITimeProvider that fetches time from the AWS website headers.
Sends a HEAD request to the AWS domain and parses the "date" header.
Returns a TimeResult indicating success or failure based on the HTTP response.

#### [`CloudflareHeaderTimeProvider`](NetworkTime/Providers/CloudflareHeaderTimeProvider.cs)

Implementation of ITimeProvider that fetches time from the Cloudflare website headers.
Sends a HEAD request with a specific User-Agent to the Cloudflare domain.
Parses the "date" header from the response to determine the network time.

#### [`DuckDuckGoHeaderTimeProvider`](NetworkTime/Providers/DuckDuckGoHeaderTimeProvider.cs)

Implementation of ITimeProvider that fetches time from the DuckDuckGo website headers.
Sends a HEAD request to the DuckDuckGo domain and extracts the time from the response headers.
Provides a fallback result if the request fails or the header is missing.

#### [`GoogleHeaderTimeProvider`](NetworkTime/Providers/GoogleHeaderTimeProvider.cs)

Implementation of ITimeProvider that fetches time from the Google website headers.
Sends a HEAD request to the Google domain and parses the "date" header.
Used as one of the primary sources for network time synchronization.

#### [`ITimeProvider`](NetworkTime/Providers/ITimeProvider.cs)

Interface defining the contract for time providers.
Requires implementation of an asynchronous method to retrieve time with cancellation support.
Allows the TimeService to swap or extend providers without changing core logic.

#### [`TimeAPIProvider`](NetworkTime/Providers/TimeAPIProvider.cs)

Implementation of ITimeProvider that fetches time from a public JSON API.
Sends a GET request to the TimeAPI service and deserializes the JSON response.
Extracts the dateTime field to provide the current UTC time.

## SafeArea

### [`SafeAreaAnchors`](SafeArea/SafeAreaAnchors.cs)

Component that adjusts the anchored position of RectTransforms to respect the device safe area.
Supports separate configurations for portrait and landscape orientations.
Automatically updates the position when the screen orientation changes or the component is enabled.
Includes editor validation to prevent conflicting anchor settings.

### [`SafeAreaResize`](SafeArea/SafeAreaResize.cs)

Component that adjusts the size delta of RectTransforms to fit within the device safe area.
Supports separate configurations for portrait and landscape orientations.
Automatically recalculates the size when the screen orientation changes.
Ensures UI elements do not extend into notches or rounded corners.

### [`SafeAreaUtility`](SafeArea/SafeAreaUtility.cs)

Static utility class providing methods to calculate safe area offsets and sizes.
Calculates top, bottom, left, and right offsets based on Screen.safeArea.
Provides methods to compute soft or full offsets for RectTransforms based on anchor flags.
Updates offsets automatically when the screen orientation changes.

## ScreenOrientation

### [`OrientationBranch`](ScreenOrientation/OrientationBranch.cs)

Component that toggles the active state of child GameObjects based on the current screen orientation.
Assigns specific GameObjects for portrait and landscape modes.
Automatically switches visibility when the device orientation changes.
Simplifies handling different UI layouts for different orientations.

### [`ScreenOrientationUtility`](ScreenOrientation/ScreenOrientationUtility.cs)

Static utility for monitoring screen orientation changes.
Provides events for orientation changes, portrait entry, and landscape entry.
Includes helper methods to check the current orientation state.
Runs a background listener to detect orientation updates during runtime.

## Serialization

### [`FlexibleNameResolver`](Serialization/FlexibleNameResolver.cs)

Custom JSON contract resolver that extends the default Newtonsoft.Json resolver.
Supports deserializing properties that may have leading underscores in their names.
Enhances flexibility when mapping JSON data to C# classes with private fields.
Useful for handling inconsistent naming conventions in external data sources.

## SmartLinks

### [`HardLink`](SmartLinks/HardLink.cs)

A strong reference link to a prefab that manages instantiation.
Ensures the instance is created only once and persists until explicitly destroyed.
Provides methods to retrieve the instance with optional initialization actions.
Inherits from SmartLink to share base instantiation logic.

### [`SmartLink`](SmartLinks/SmartLink.cs)

Abstract base class for managing prefab references and instantiation logic.
Supports lazy initialization of prefabs upon first request.
Stores the prefab reference and tracks whether the instance has been created.
Provides editor utilities to access the underlying prefab reference.

### [`SoftLink`](SmartLinks/SoftLink.cs)

A weak reference link to a prefab that requires an external root transform for instantiation.
Instantiates the prefab under a specified root transform when requested.
Suitable for scenarios where the instance parent is determined dynamically at runtime.
Inherits from SmartLink to share base instantiation logic.

## States

### [`IState`](States/IState.cs)

Interface defining the contract for state objects in a state machine.
Requires implementation of Enter and Exit methods to handle state transitions.
Used to encapsulate behavior specific to a particular state.

### [`IStateMachine`](States/IStateMachine.cs)

Interface defining the contract for a state machine controller.
Requires a property to get and set the current active state.
Allows state extensions to manipulate the state machine without knowing the concrete implementation.

### [`StateExtension`](States/StateExtension.cs)

Extension methods providing safe state transition logic for state machines.
Handles exiting the current state before entering the new one.
Includes a method to change state only if the target state type differs from the current one.
Simplifies state management code in implementing classes.

## UIEffects

### [`UIButtonEffects`](UIEffects/UIButtonEffects.cs)

Extension methods for adding click animation effects to UI buttons using DOTween.
Provides a method to animate the scale of a transform to simulate a press effect.
Requires the DOTWEEN scripting define symbol to function.
Returns a Tweener object for further customization of the animation.

### [`UIWindowEffects`](UIEffects/UIWindowEffects.cs)

Extension methods for adding show/hide fade effects to CanvasGroup components using DOTween.
Provides methods to fade the alpha value in or out over a specified duration.
Includes force methods to instantly set the alpha value without animation.
Requires the DOTWEEN scripting define symbol to function.

## Validation

### [`FixExtension`](Validation/FixExtension.cs)

Editor utilities for validating and fixing missing components on GameObjects.
Attempts to get a required component and logs an error if it is missing.
Marks the component as dirty to ensure changes are saved in the editor.

### [`InspectorUtility`](Validation/InspectorUtility.cs)

Helper for generating dropdown lists of types in the Inspector.
Retrieves all subtypes of a given generic type using reflection.
Often used with Odin Inspector to provide type selection fields.

### [`ResourcesDebug`](Validation/ResourcesDebug.cs)

Debugging helper for loading resources with error logging if the asset is missing.
Wraps the standard Resources.Load method with conditional error checking.
Active in Editor or when a specific debug symbol is defined.

### [`ResourcesValidate`](Validation/ResourcesValidate.cs)

Editor validation tools for checking resource existence and creating ScriptableAssets.
Provides methods to verify if a resource exists at a given path and report validation errors.
Includes functionality to create new ScriptableObject assets in the Resources folder.

### [`ValidateExtension`](Validation/ValidateExtension.cs)

Extension methods for validating component relationships in the editor.
Checks if a component is not the current instance to prevent self-references.
Used in custom inspector validation logic to ensure data integrity.

### [`ValidateMessage`](Validation/ValidateMessage.cs)

Helper for adding standardized validation error messages to self-validation results.
Provides specific error messages for common validation failures like incorrect component types.
Integrates with Odin Inspector's validation system.

## Vibration

### [`PredefinedEffect`](Vibration/PredefinedEffect.cs)

Enumeration of standard haptic feedback patterns.
Includes presets for Click, DoubleClick, HeavyClick, and Tick.
Used to simplify the invocation of common vibration patterns.

### [`VibrationEffect`](Vibration/VibrationEffect.cs)

Static class managing haptic feedback on mobile devices.
Supports amplitude control and waveform vibrations on Android API 26 and above.
Provides fallback to standard Handheld.Vibrate for iOS and older Android versions.
Includes methods to create one-shot vibrations, waveforms, and predefined effects.
Handles cancellation of ongoing vibration effects.