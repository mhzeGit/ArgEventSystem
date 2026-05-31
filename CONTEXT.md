# CONTEXT.md — ArgEvent System

## Project Overview

- Serializable Unity event binding system with multi-parameter support (0, 1, 2 generic args), flexible argument sourcing (Constant/Script/Event), and a UI Toolkit-based custom inspector.
- Replaces boilerplate UnityEvent wiring: developers configure event listeners entirely in the Inspector — drag-drop, reorder, copy/paste, source parameters from constants, other components, or invoke-time arguments.
- Target platform: Unity 2022.3+, packaged as a UPM package (`com.mhze.argevent`). Install via Git URL `https://github.com/mhzeGit/ArgEventSystem.git?path=Packages/com.mhze.argevent`.

## File & Folder Structure

- `Packages/com.mhze.argevent/` — the actual UPM package; everything else is sample project scaffolding
  - `Runtime/` — runtime assembly (`ArgEvent` namespace), shipped in builds
    - `ArgumentSource.cs` — enum: `Constant`, `Script`, `Event`
    - `EventBinding.cs` — `ArgEventBinding` base + `ArgEventBinding<T>` + `ArgEventBinding<T1, T2>` generic variants
    - `Listener.cs` — binds Component + method, caches reflection, resolves parameters at invoke time
    - `ParameterEntry.cs` — per-parameter serialized config (source type, constant value, script/event references)
  - `Editor/` — editor-only assembly (`ArgEvent.Editor` namespace), not shipped
    - `EventBindingDrawer.cs` — 1629-line UI Toolkit PropertyDrawer (CreatePropertyGUI, listener cards, drag-drop, copy/paste, method filtering, type resolution)
    - `Styles.cs` — empty placeholder (all styles migrated to USS)
    - `Resources/EventBindingDrawer.uss` — 431-line dark theme stylesheet for the inspector UI
  - `package.json` — UPM manifest: `com.mhze.argevent` v1.0.0
- `Packages/manifest.json` — Unity project manifest (depends only on imgui, jsonserialize, ui, uieleements modules)
- `ArgEventSystem.slnx` — solution file referencing the two .csproj files
- `com.mhze.argevent.Runtime.csproj` / `com.mhze.argevent.Editor.csproj` — auto-generated .NET project files
- `Assembly-CSharp.csproj` / `Assembly-CSharp-Editor.csproj` — Unity default assemblies (no user code assigned to them)
- `Assets/` — only default URP assets + preview GIF; contains NO user scripts
- `ProjectSettings/` — standard Unity project settings (URP-based project)
- `.vscode/` — VS Code config: extension recommendations (vstuc), launch profile for Unity attach, file nesting rules
- `.kilo/` — Kilo AI agent manager config; ignore for code understanding

**Folders/files to ignore:**
- `.git/`, `.kilo/`, `Library/`, `Logs/`, `Temp/`, `UserSettings/`, `*.csproj`, `*.meta`, `*.asset`, `ProjectSettings/*`
- `Assets/DefaultVolumeProfile.asset`, `Assets/UniversalRenderPipelineGlobalSettings.asset` — default URP assets, not part of the package
- `Assembly-CSharp.csproj`, `Assembly-CSharp-Editor.csproj` — Unity-generated, no user code

**One-line description of every meaningful script:**

| Script | What it is and does |
|--------|-------------------|
| `ArgumentSource.cs` | Enum defining three ways a parameter value can be sourced: literal (Constant), reflected from another component (Script), or from invoke-time event args (Event) |
| `EventBinding.cs` | Three serializable binding classes (0/1/2 generic params) holding `List<Listener>`, exposes `Invoke()` which iterates listeners and dispatches args as `object[]` |
| `Listener.cs` | Binds a target `Component` + method name via reflection; resolves each parameter from its `ParameterEntry` source at invoke time; caches `MethodInfo`, `ParameterInfo`, and creates an `Action` delegate for zero-arg methods |
| `ParameterEntry.cs` | Serializable container per method parameter: stores constant values for ~25+ types (primitives, Unity structs, enums, Object references); resolves Script values via reflected field/property on a Component; supports dot-path traversal (`DamageData.Source.Position`) for Event source |
| `EventBindingDrawer.cs` | UI Toolkit `PropertyDrawer` for `ArgEventBinding`: builds foldout header, draggable/reorderable listener cards with GameObject/Component/Method selectors, per-parameter source sections with type-appropriate value fields, copy/paste via JSON, method filtering (excludes ~60 Unity special methods, generics, out/ref, obsolete), auto-matching of event arg variables |
| `Styles.cs` | Empty file; all styles live in the USS stylesheet under `Resources/` |
| `EventBindingDrawer.uss` | Dark theme USS stylesheet: defines .event-binding-root, .listener-card, .accent-strip, .drag-handle, .remove-button, .drop-indicator, .method-display-field, .add-listener-footer, .script-source-container, and all parameter section/entry classes |

## Architecture & Patterns

**How the codebase is organized:**
- Runtime and Editor are in separate assemblies (asmdefs) with clean namespace isolation: `ArgEvent` vs `ArgEvent.Editor`
- Composition, not inheritance: `ArgEventBinding` owns `Listener[]`, each `Listener` owns `ParameterEntry[]`, each `ParameterEntry` has an `ArgumentSource`
- Generic variants (`ArgEventBinding<T>`, `ArgEventBinding<T1, T2>`) inherit from base `ArgEventBinding` and only override `Invoke()` to wrap typed args into `object[]`
- Editor uses UI Toolkit (`CreatePropertyGUI`), NOT IMGUI — all styles are in USS, not C#

**Key base classes / shared systems:**
- `ArgEventBinding` — the only public-facing runtime class users declare as `[SerializeField]` fields in their MonoBehaviours
- `Listener` — core dispatch engine; all invoke paths converge on `Listener.Invoke(object[])` which resolves every parameter before calling `MethodInfo.Invoke()`
- `ParameterEntry` — responsible for all value resolution; its `GetConstantValue()` handles type coercion (e.g., Quaternion from Euler Vector3, char from string, enum from int)
- `EventBindingDrawer` — single monolithic PropertyDrawer; all editor UI logic lives here (no ViewModel, no separate controller classes)
- `MethodImplOptions.AggressiveInlining` used on all `Invoke()` methods for performance

**State management:**
- No runtime state management framework — this is a serialization-only system
- All state is Unity-serialized `[SerializeField]` fields on the binding/listener/parameter classes
- Editor reads/writes `SerializedProperty` directly (no Model-View separation)
- Editor clipboard state uses static string + `JsonUtility.ToJson/FromJson` with `InstanceID` for Unity Object references
- Reflection caches (`_cachedMethod`, `_cachedField`, etc.) are `[NonSerialized]` and lazily initialized on first use; invalidated when target/method changes

**Naming conventions used consistently:**
- Namespaces: `ArgEvent` (runtime), `ArgEvent.Editor` (editor)
- Public classes: PascalCase (`ArgEventBinding`, `Listener`, `ParameterEntry`, `ArgumentSource`)
- Private/internal fields: `_camelCase` with underscore prefix and `[SerializeField]` or `internal` modifier
- Non-serialized cache fields: `_camelCase` with `[NonSerialized]` attribute
- Properties: PascalCase (`Enabled`, `Target`, `MethodName`, `Parameters`)
- Public methods: PascalCase (`Invoke`, `AddListener`, `RemoveListener`, `Initialize`, `InvalidateCache`)
- Private methods: PascalCase (`GetConstantValue`, `ResolveEventArg`, `CacheMemberInfo`, `ResolveScriptValue`)
- Local variables: `camelCase`
- Enum values: PascalCase (`Constant`, `Script`, `Event`)
- File names match class names exactly
- Constants: PascalCase (`UnitySpecialMethods`)
