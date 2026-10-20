# RunnerShooter

A vehicle drives forward through a level, and the player controls its turret. Stickman enemies run at the vehicle when it gets close and damage it on contact. The goal is to reach the finish before the vehicle's HP runs out.

- **Unity:** 6000.3.18f1, URP
- **Libraries:** Zenject (Extenject), UniTask, Addressables, Cinemachine, DOTween, Input System

## Running it

1. Open the project. Packages resolve from `Packages/manifest.json`, and DOTween is committed under `Assets/Plugins/`.
2. Press **Play** from any scene. An editor script always starts from `Boot`, which loads `Gameplay`.
3. Tests: *Test Runner → EditMode → Run All*.

## How to play

| Input | Effect |
|---|---|
| Tap | Start the run, or restart after a win or loss |
| Hold | Auto-fire |
| Drag left / right while holding | Aim the turret (±35°) |

- Enemies wander side to side across the road. When the vehicle gets close, they run at it and explode on contact.
- **Win:** reach the finish line. "You win" appears, and the car coasts to a stop past the line.
- **Lose:** HP reaches 0. The car blows apart, and "You lose" appears.
- A tap after either result restarts the level, and the vehicle waits for the next tap to move.

## Architecture

### Scenes

- **`Boot`** holds app-lifetime services: `IAssetProvider` (Addressables with a cache), `ISceneLoader`, and the loading screen. It stays loaded.
- **`Gameplay`** contains only a `SceneContext` with `GameplayInstaller`, the light and the EventSystem. Everything else spawns at runtime from Addressables. The scene loads additively as a child container of `Boot`.

**Restart reloads `Gameplay`.** Every class is rebuilt from its constructor, so no class needs reset code. Prefabs stay cached in `Boot`, which keeps the reload fast.

### Features

Each feature is one folder under `Assets/Scripts/Gameplay/`: `GameFlow`, `Input`, `Level`, `Vehicle`, `Turret`, `Enemies`, `CameraRig`, `UI`.

- **Services** hold state and rules only. They are plain C#, and they are unit-tested.
- **Systems** connect services to the scene: they spawn and pool objects and drive views.
- **Views** are MonoBehaviours with no game rules.
- **UI** uses MVP: presenters push service state to passive views.

Dependencies point one way (Views → Systems → Services), and features talk only through interfaces.

### Composition

`GameplayInstaller` builds the feature list in code. Each `FeatureInstaller` preloads its prefabs before the container is built, then binds them. Adding a feature takes its code, one line in `CreateFeatures()`, and a config asset. The scene doesn't change.

### Game flow

```
Loading ──▶ WaitingForTap ──tap──▶ Playing ──finish──▶ Won  ──tap──▶ reload
                                          └─HP = 0──▶ Lost ──tap──▶ reload
```

`GameFlowService` owns the state, and other features react to its `StateChanged` event. All loading and pool warm-up happens in `Loading`, so nothing loads mid-run.

### Other decisions

- **Configs:** balance values and prefab references live in one ScriptableObject per feature.
- **Pooling:** `ViewPool<T>` wraps Unity's `ObjectPool<T>`. Views raise an event when they finish, and their system releases them.
- **Physics:** only the `Enemy` and `Vehicle` layers collide. Bullets raycast each frame instead of using colliders, so they can't pass through enemies.
- **Car destruction** is a wreck swap animated with DOTween, so it plays the same every time.

## Where to look

| What | Path |
|---|---|
| Boot, scene loading, Addressables, pooling (separate `Core` asmdef) | `Assets/Scripts/Core/` |
| Feature composition | `Assets/Scripts/Gameplay/GameFlow/Installers/GameplayInstaller.cs` |
| Game state | `Assets/Scripts/Gameplay/GameFlow/Services/GameFlowService.cs` |
| Enemy movement and spawning | `Assets/Scripts/Gameplay/Enemies/Systems/` |
| Tuning | `Assets/Configs/` |
| Tests | `Assets/Tests/EditMode/` |
| Prefabs, models, VFX, grouped by feature | `Assets/Graphics Resources/` |
