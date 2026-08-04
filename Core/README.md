# TinyUtilities

A lightweight collection of utilities, extensions, and helper classes for C#.
It provides essential tools for mathematics, collections, asynchronous operations, logging, and system interactions without heavy dependencies.

**Key Features:**
- Comprehensive extension methods for standard types (`Array`, `List`, `Action`, `Enum`, `DateTime`)
- Math and Random utilities
- Async timer and pause mechanisms
- Safe logging system with conditional compilation
- JSON converters for flexible schema handling
- System process execution helpers

## Table of Contents

- [Core](#core)
	- [`ArrayDataConverter`](#arraydataconverter)
	- [`AsyncPause`](#asyncpause)
	- [`EnumUtility`](#enumutility)
	- [`MathfUtility`](#mathfutility)
	- [`RandomUtility`](#randomutility)
	- [`ShellUtility`](#shellutility)
	- [`TextUtility`](#textutility)
	- [`TimerUtility`](#timerutility)
	- [`TimeSpanUtility`](#timespanutility)
- [Custom Types](#custom-types)
	- [`BlittableBool`](#blittablebool)
- [Extensions](#extensions)
	- [`ActionExtension`](#actionextension)
	- [`ArrayExtension`](#arrayextension)
	- [`BoolExtension`](#boolextension)
	- [`CancellationTokenSourceExtension`](#cancellationtokensourceextension)
	- [`DateTimeExtension`](#datetimeextension)
	- [`DictionaryExtensions`](#dictionaryextensions)
	- [`DisposableExtension`](#disposableextension)
	- [`EnumExtension`](#enumextension)
	- [`FuncExtension`](#funcextension)
	- [`IntExtension`](#intextension)
	- [`ListExtension`](#listextension)
	- [`ObjectExtension`](#objectextension)
	- [`TimeConvertExtension`](#timeconvertextension)
	- [`TimeSpanExtension`](#timespanextension)
- [JSON Converters](#json-converters)
	- [`JsonStringOrArrayConverter`](#jsonstringorarrayconverter)
- [Logger](#logger)
	- [`DebugUtility`](#debugutility)
	- [`DebugDisplay`](#debugdisplay)
	- [`LogType`](#logtype)

## Core

Standalone static utility classes, each focused on a single domain of functionality.

### [`ArrayDataConverter`](ArrayDataConverter.cs)

Provides extension methods to convert integer arrays and integer lists into byte arrays.
Each integer element is explicitly cast to a byte, suitable for data serialization where size reduction is required.

### [`AsyncPause`](AsyncPause.cs)

Implements a mechanism to pause and resume asynchronous tasks without cancellation.
The `Waiting` method yields execution if the pause flag is active, allowing external control over task flow.

### [`EnumUtility`](EnumUtility.cs)

Offers reflection-based tools for Enumerations. Includes methods to iterate over all enum values, execute actions on the first matching value, convert enums to arrays, retrieve value-index tuples, and count total defined values.

### [`MathfUtility`](MathfUtility.cs)

Contains mathematical constants and functions typically found in game development frameworks.
Includes trigonometric functions, min/max operations with parameter arrays, clamping, interpolation (Lerp, SmoothStep), smoothing (SmoothDamp), and power-of-two calculations.

### [`RandomUtility`](RandomUtility.cs)

Provides a static interface for random number generation.
Supports range generation for integers and floats, selecting random elements from arrays, and generating offset values based on a central point.

### [`ShellUtility`](ShellUtility.cs)

Facilitates asynchronous execution of system processes. Includes specific helpers for WSL and PowerShell commands.
Supports capturing standard output as strings or running silent processes with optional event-based logging.

### [`TextUtility`](TextUtility.cs)

Handles text formatting for numbers and time. Converts large integers and floats into abbreviated strings using K (thousands) and M (millions) suffixes with specific rich text formatting tags.
Also formats `TimeSpan` objects into HH:MM:SS strings.

### [`TimerUtility`](TimerUtility.cs)

Implements asynchronous countdown timers. Supports callbacks for updating remaining time and completion events.
Includes helpers to calculate time remaining until the next day and offline tick calculations for idle progress systems.

### [`TimeSpanUtility`](TimeSpanUtility.cs)

Provides comparison operations for `TimeSpan` structures. Includes methods to find the minimum or maximum value from parameters and clamp a `TimeSpan` value within a defined minimum and maximum range.

## Custom Types

New types introduced by the library itself — structs, classes, or records that define their own data shape or behavior, rather than adding functionality to an existing .NET type.

### [`BlittableBool`](CustomTypes/BlittableBool.cs)

A readonly struct that wraps a boolean value using a byte internally.
Implements equality and comparison interfaces.
Designed for scenarios requiring blittable types for marshalling or specific memory layout constraints.

## Extensions

Extension methods that attach new functionality directly onto existing types, keeping the call syntax fluent and instance-like instead of requiring a separate utility call.

### [`ActionExtension`](Extensions/ActionExtension.cs)

Extends `Action` lists and arrays with invocation methods. Includes `InvokeSafe` variants that copy the collection before execution to prevent errors if the collection is modified during invocation.
Supports generic actions with up to three parameters.

### [`ArrayExtension`](Extensions/ArrayExtension.cs)

Provides manipulation methods for arrays. Includes functionality to add or remove elements, generate number sequences, check containment, calculate averages, convert to string representations, remove ranges, find indices, and reverse arrays.

### [`BoolExtension`](Extensions/BoolExtension.cs)

Adds a method to convert boolean values into capitalized string representations ("True" or "False").

### [`CancellationTokenSourceExtension`](Extensions/CancellationTokenSourceExtension.cs)

Manages the lifecycle of `CancellationTokenSource` instances. Includes methods to reset, recreate, update, and safely cancel and dispose of token sources to prevent resource leaks.

### [`DateTimeExtension`](Extensions/DateTimeExtension.cs)

Enables serialization of `DateTime` objects to XML strings and parsing them back. Includes a helper to determine if a given date falls on the next day relative to another date.

### [`DictionaryExtensions`](Extensions/DictionaryExtensions.cs)

Adds functionality to dictionaries with integer values.
The `AddOrSet` method increments an existing value by a specified amount or sets the initial value if the key does not exist.

### [`DisposableExtension`](Extensions/DisposableExtension.cs)

Provides bulk disposal methods for collections.
Iterates over arrays, lists, or dictionary values implementing `IDisposable` and calls Dispose on each element.

### [`EnumExtension`](Extensions/EnumExtension.cs)

Implements bitwise flag checking for Enumerations.
Converts enum values to unsigned shorts to perform `Is`, `IsNot`, and `Any` checks against provided flags.

### [`FuncExtension`](Extensions/FuncExtension.cs)

Extends lists and arrays of `Func<bool>`.
Provides invocation methods that iterate through the functions and stop execution once a target boolean result is achieved.
Includes safe invocation variants.

### [`IntExtension`](Extensions/IntExtension.cs)

Adds a method to check if an integer value matches any value within a provided parameter array.

### [`ListExtension`](Extensions/ListExtension.cs)

Extends `List<T>` with manipulation tools. Includes adding ranges with exclusion predicates, checking containment, counting unique elements based on hash codes, verifying uniqueness, retrieving values by hash, and converting lists to string representations.

### [`ObjectExtension`](Extensions/ObjectExtension.cs)

Provides a generic method to ensure an object instance exists.
Returns the current object if not null, otherwise creates and returns a new instance using the parameterless constructor.

### [`TimeConvertExtension`](Extensions/TimeConvertExtension.cs)

Provides conversion methods between seconds and milliseconds.
Converts float seconds to integer milliseconds and integer or double milliseconds to float seconds.

### [`TimeSpanExtension`](Extensions/TimeSpanExtension.cs)

Extends `TimeSpan` with serialization and calculation helpers.
Includes XML serialization, parsing, custom total hours and minutes calculations, and subtraction of seconds.

## JSON Converters

Custom serialization/deserialization logic for handling JSON shapes.

### [`JsonStringOrArrayConverter`](JsonConverters/JsonStringOrArrayConverter.cs)

A `System.Text.Json` converter for string arrays.
Allows a JSON field to be deserialized from either a single string (converted to a single-element array) or a standard JSON array.
Serializes single-element arrays as a raw string and multi-element arrays as a JSON array.

## Logger

A small, self-contained logging pipeline.

### [`DebugUtility`](Logger/DebugUtility.cs)

Centralized logging system wrapped in conditional compilation (`DEBUG`).
Supports log levels, dividers, exception logging, and value dumping. Includes a marker method that logs the calling method's signature via stack trace.
Supports desktop console coloring and event-based message updates.

### [`DebugDisplay`](Logger/DebugDisplay.cs)

Low-level logging helper that writes messages to the console with colors based on log type or sends messages to the system debug output.

### [`LogType`](Logger/LogType.cs)

Enumeration defining the severity levels for logging: Log, Warning, and Error.
Used to determine console coloring and log categorization.