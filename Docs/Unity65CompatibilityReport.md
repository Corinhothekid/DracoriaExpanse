# Unity 6.5 Compatibility Report

Date: 2026-07-04
Target: Unity 6.5 / 6000.5
Scope: Compatibility and modernization only. No gameplay systems were added or redesigned.

## Documentation Reviewed

- Unity Manual, Unity 6.5 / 6000.5, New in Unity 6.0 LTS: https://docs.unity3d.com/Manual/WhatsNewUnity6.html
- Unity Manual, Upgrade Unity: https://docs.unity3d.com/Manual/UpgradeGuides.html
- Unity Scripting API, Object.FindObjectOfType: https://docs.unity3d.com/ScriptReference/Object.FindObjectOfType.html
- Unity Scripting API, Object.FindAnyObjectByType: https://docs.unity3d.com/ScriptReference/Object.FindAnyObjectByType.html
- Unity Scripting API, Object.FindObjectsByType: https://docs.unity3d.com/ScriptReference/Object.FindObjectsByType.html
- Unity Input System package manual 1.14.2: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/index.html
- Unity Manual, UI Toolkit: https://docs.unity3d.com/Manual/UIElements.html
- Unity Manual, Optimization for Android: https://docs.unity3d.com/Manual/android-optimization.html

## Audit Summary

### Deprecated object lookup APIs

Issue found: Several runtime and editor scripts used `FindObjectOfType` or `FindObjectsOfType`, which are obsolete in Unity 6.5.

Action taken: Replaced them with `Object.FindAnyObjectByType` or `Object.FindObjectsByType`, using `FindObjectsInactive.Include` where the previous call searched inactive UI.

Reason: Unity 6.5 marks `Object.FindObjectOfType` obsolete and recommends `Object.FindAnyObjectByType` when any instance is acceptable. The project only needs an available instance in these locations.

### Legacy Input Manager usage

Issue found: Player movement, camera mouse look, combat controls, crafting UI close, and manager hotkeys directly used `UnityEngine.Input` APIs.

Action taken: Added `DracoriaInput`, a small input compatibility helper. Gameplay scripts now call the helper instead of directly using `Input.GetKey`, `Input.GetAxis`, or mouse button calls.

Reason: Unity's newer Input System package is the recommended replacement for the classic Input Manager. The helper supports the new Input System behind `ENABLE_INPUT_SYSTEM` while preserving old Input Manager behavior when the package/define is not enabled.

### Runtime-created EventSystems

Issue found: UI factories/builders created `StandaloneInputModule`, which belongs to the older input workflow.

Action taken: Runtime/editor-generated EventSystems now add `InputSystemUIInputModule` when `ENABLE_INPUT_SYSTEM` is active, and fall back to `StandaloneInputModule` otherwise.

Reason: This keeps generated uGUI usable with either input backend without forcing a risky UI rewrite.

### UI Toolkit readiness

Issue found: The prototype currently uses uGUI generated from code. That is acceptable for this prototype, but not the long-term ideal for scalable production UI.

Action taken: No UI Toolkit rewrite was performed because it would be a broad redesign. The compatibility pass only made existing uGUI safer with the new input module path.

Deferred recommendation: When the UI stabilizes, move persistent panels such as inventory, professions, character sheet, and crafting to UI Toolkit/UXML/USS while keeping combat HUD and touch controls evaluated case-by-case.

### Serialization

Issue found: Runtime save data uses serializable plain C# data and JSON. No obsolete Unity serialization attributes were found.

Action taken: No serialization format rewrite. Existing save compatibility was preserved.

Deferred recommendation: Move static registries such as items, recipes, professions, race visuals, and gathering nodes to ScriptableObject assets later. This will improve authoring, Addressables readiness, and content iteration.

### Mobile performance and garbage collection

Issue found: Some prototype systems allocate short-lived strings and UI text during interactions, crafting refreshes, notifications, and generated placeholder setup. Current risk is acceptable for the tiny prototype, but some systems will need pooling as content grows.

Action taken: No gameplay-facing pooling change was made. Direct deprecated APIs were prioritized.

Deferred recommendation: Add object pools for floating damage text, notification popups, gathering progress visuals, and future enemy/NPC spawn effects before mobile combat density increases.

### Build pipeline and package compatibility

Issue found: The repository does not currently include `Packages/manifest.json`, `Packages/packages-lock.json`, or `ProjectSettings` files. That means package and active input backend compatibility cannot be fully validated from source alone.

Action taken: No package manifest was generated automatically, because doing so without the local Unity project settings could unintentionally change package resolution.

Deferred recommendation: After opening in Unity 6.5, commit `Packages/manifest.json`, `Packages/packages-lock.json`, and relevant `ProjectSettings` files. Install/enable `com.unity.inputsystem` and set Active Input Handling to Input System Package or Both during the transition.

## Files Modified

- `Assets/WildsOfDracoria/Scripts/Input/DracoriaInput.cs`
  - Created a compatibility wrapper for keyboard, mouse, and movement axes.
- `Assets/WildsOfDracoria/Scripts/Camera/ThirdPersonCameraFollow.cs`
  - Routed mouse look through `DracoriaInput`.
- `Assets/WildsOfDracoria/Scripts/Player/ThirdPersonPlayerController.cs`
  - Routed movement, sprint, jump, and interact input through `DracoriaInput`.
- `Assets/WildsOfDracoria/Scripts/Combat/PlayerCombat.cs`
  - Routed attack, block, and dodge input through `DracoriaInput`.
- `Assets/WildsOfDracoria/Scripts/Crafting/CraftingUI.cs`
  - Routed Escape close input through `DracoriaInput`.
- `Assets/WildsOfDracoria/Scripts/Systems/GameManager.cs`
  - Routed hotkeys through `DracoriaInput` and replaced obsolete object lookups.
- `Assets/WildsOfDracoria/Scripts/CharacterCreation/CharacterCreationStartup.cs`
  - Replaced obsolete inactive UI lookup.
- `Assets/WildsOfDracoria/Scripts/CharacterCreation/CharacterCreationUIFactory.cs`
  - Replaced obsolete EventSystem lookup and added Input System UI module support.
- `Assets/WildsOfDracoria/Scripts/Combat/EnemyHealth.cs`
  - Replaced obsolete player combat lookup after enemy death.
- `Assets/WildsOfDracoria/Scripts/Crafting/CraftingManager.cs`
  - Replaced obsolete crafting UI lookups.
- `Assets/WildsOfDracoria/Scripts/Gathering/GatheringNodeSceneBootstrap.cs`
  - Replaced obsolete gathering node lookup.
- `Assets/WildsOfDracoria/Scripts/UI/Mobile/MobileControlsBootstrap.cs`
  - Replaced obsolete mobile router lookup.
- `Assets/WildsOfDracoria/Scripts/UI/Mobile/MobileControlsRouter.cs`
  - Replaced obsolete runtime object/UI lookups.
- `Assets/WildsOfDracoria/Scripts/Editor/IronhavenPrototypeBuilder.cs`
  - Replaced obsolete full-scene object search.
- `Assets/WildsOfDracoria/Scripts/Editor/MobileControlsSceneBuilder.cs`
  - Replaced obsolete EventSystem lookup and added Input System UI module support.
- `Assets/WildsOfDracoria/Scripts/Editor/ProfessionFrameworkSceneBuilder.cs`
  - Replaced obsolete manager/canvas lookups.
- `Docs/Unity65CompatibilityReport.md`
  - Added this report.

## Remaining Recommendations

1. Commit Unity project settings and packages after opening in Unity 6.5.
2. Install/enable the Input System package and test with Active Input Handling set to Both first, then Input System Package only after all controls are verified.
3. Convert content registries to ScriptableObject assets before item/profession counts grow.
4. Add object pooling for floating damage numbers and notifications before mobile performance testing.
5. Keep uGUI for the prototype, but plan a UI Toolkit migration for persistent menu panels.
6. Add assembly definitions once the folder structure stabilizes.
7. Add automated Edit Mode smoke tests for registries, save/load, and input helper behavior.

## Verification Notes

A source scan after the changes found no remaining direct `FindObjectOfType` or `FindObjectsOfType` calls. The remaining direct `UnityEngine.Input` calls are isolated inside `DracoriaInput` as the intended compatibility layer.

Unity compile/play testing still needs to be performed in the Unity Editor because this workspace does not contain a local Unity installation or full checked-out project settings.
