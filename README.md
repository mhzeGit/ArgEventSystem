# ArgEvent System

A serializable event binding system for Unity with multi-parameter support, flexible argument sourcing, and a polished inspector UI.

Stop writing boilerplate UnityEvents and manually wiring up callbacks. ArgEvent lets you configure event listeners entirely in the Inspector — drag, drop, reorder, copy/paste, and source parameter values from constants, other components, or event arguments at runtime.

![ArgEvent System preview](Assets/ArgEventGif01.gif)

## Features

- **Serializable** — Works with Unity serialization, no runtime registration needed
- **Multi-parameter** — Supports 0–8 generic type arguments with `ArgEventBinding` through `ArgEventBinding<T1…T8>`, or any number via the non-generic `ArgEventBinding.Invoke(object[])`
- **Flexible argument sourcing** — each parameter can be:
  - **Constant** — a literal value (int, float, bool, Vector2/3/4, Color, string, enum, GameObject, Component, etc.)
  - **Script** — a public field or property from any other component on any GameObject
  - **Event** — the event argument passed at invoke time, with support for member path traversal (e.g. `Arg 0 > position`)
- **Custom Inspector** — UI Toolkit-based property drawer with:
  - Dark theme styling
  - Drag-and-drop listener reordering
  - Copy/paste between listeners
  - Color-coded method signatures
  - Foldable parameter sections with colored accents per source type

## Installation

### Via Unity Package Manager (Git URL)

1. Open **Window > Package Manager**
2. Click the **+** button and select **"Add package from git URL..."**
3. Paste:

```
https://github.com/mhzeGit/ArgEventSystem.git?path=Packages/com.mhze.argevent
```

## Quick Start

```csharp
using ArgEvent;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private ArgEventBinding _onDamageTaken;
    [SerializeField] private ArgEventBinding<DamageData> _onDamaged;
    [SerializeField] private ArgEventBinding<int, int> _onScoreChanged;
    [SerializeField] private ArgEventBinding<string, float, GameObject, int> _onMultiParamEvent;

    private void Awake()
    {
        // Invoke from code — the Inspector-configured listeners handle the rest
        _onDamageTaken.Invoke();
        _onDamaged.Invoke(new DamageData { Amount = 10 });
        _onScoreChanged.Invoke(100, 5);
        _onMultiParamEvent.Invoke("headshot", 25.5f, target, 1);
    }
}
```

In the Inspector, configure listeners by selecting a target GameObject, a script component, a method, and how each parameter should be sourced.

## How It Works

| Type | Description |
|------|-------------|
| `ArgEventBinding` | Base class. Holds a list of `Listener` objects. `Invoke()` fires zero-arg listeners; `Invoke(object[])` passes arbitrary arguments. |
| `ArgEventBinding<T>` | Generic variant that passes one typed argument to listeners. |
| `ArgEventBinding<T1, T2>` | Generic variant for two typed arguments. |
| `ArgEventBinding<T1…T8>` | Typed variants for 3 through 8 arguments (same pattern). |
| `Listener` | Binds a target component + method name. Resolves parameter values at runtime via reflection. |
| `ParameterEntry` | Configures how a single method parameter is sourced (Constant, Script, or Event). |
| `ArgumentSource` | Enum: `Constant`, `Script`, `Event` |

### Parameter Source Types

- **Constant** — the value is entered directly in the Inspector (int, float, bool, string, Vector2/3/4, Color, Rect, Bounds, AnimationCurve, Gradient, LayerMask, GameObject, Component, enum, etc.)
- **Script** — pulls a value from a public field or property on any component on any GameObject
- **Event** — uses the event argument passed at invoke time. Supports member path traversal: if the event arg is a `DamageData` struct, you can drill into `DamageData.Source.Position`

## Author

**MHZE** — mhzeGit

## License

MIT
