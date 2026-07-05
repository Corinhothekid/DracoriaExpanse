# Engineering Standards

Permanent rules for Dracoria Expanse development. Read this before coding.

## Core Rules

- Build for Unity 6.5 / 6000.5 or newer unless the project explicitly changes target version.
- Preserve existing gameplay during cleanup, modernization, and refactor passes.
- Do not add new systems during cleanup passes.
- Do not expand scope without explicit user approval.
- Keep the prototype mobile-first, readable, modular, and easy to grow.
- Update `Docs/TechnicalDebt.md` whenever a known issue, risky shortcut, or deferred migration is left behind.

## Unity 6.5 Coding Standards

- Do not use obsolete Unity APIs.
- Use `Object.FindAnyObjectByType` for occasional startup/runtime lookups where any instance is acceptable.
- Use `Object.FindObjectsByType` for editor tooling that needs multiple objects.
- Avoid scene-wide object searches in `Update`, `FixedUpdate`, or frequently repeated gameplay paths.
- Cache component references in `Awake`, `Start`, or explicit registration methods.
- Prefer `TryGetComponent` for optional component checks when adding new code.
- Keep `MonoBehaviour` scripts small and responsible for scene/runtime behavior only.
- Keep serializable data models free of Unity scene references unless they are explicitly Unity assets.
- Use `nameof(...)` where practical for reflected field/property names in editor tooling.

## Folder Structure Rules

Runtime code belongs under `Assets/WildsOfDracoria/Scripts` in the closest existing domain folder:

```text
AI/
Camera/
CharacterCreation/
Combat/
Crafting/
Data/
Gathering/
Input/
Interaction/
Items/
Player/
Professions/
Save/
Systems/
UI/
  Mobile/
Visuals/
```

Rules:

- Editor-only code goes in `Scripts/Editor` and must be wrapped in `#if UNITY_EDITOR`.
- New systems get their own domain folder only when they have multiple scripts or clear ownership.
- Shared IDs/constants stay near their domain, such as `ItemIds`, `ProfessionIds`, or future `NodeIds`.
- Documentation lives in `Docs`.
- Do not place generated cache, Unity Library, Temp, Logs, builds, or large binary artifacts in the repo.

## Naming Conventions

- Classes, enums, properties, and public methods use `PascalCase`.
- Private fields use `camelCase`.
- Serialized private fields use `[SerializeField] private` and `camelCase`.
- Constants use `PascalCase` when they behave like domain IDs, such as `ItemIds.CopperOre`.
- Runtime item/profession/node IDs use lowercase snake case, such as `copper_ore`.
- UI factory-created GameObjects should use clear human-readable names.
- Avoid abbreviations unless already common in the project, such as UI or XP.

## Script Responsibility Rules

- A manager registers, coordinates, and exposes events; it should not own unrelated UI layout or content authoring.
- A data class stores serializable state and validation helpers; it should not search scenes or instantiate objects.
- A UI class displays state and forwards user actions; it should not become the source of gameplay truth.
- A registry defines static prototype content until that content is promoted to ScriptableObjects.
- An interactable owns only its interaction behavior and should call managers for inventory, XP, saves, or notifications.
- Keep existing systems modular: movement, combat, inventory, professions, crafting, gathering, save/load, and UI should not directly absorb each other.

## Mobile-First Performance Rules

- Avoid allocations in per-frame paths.
- Do not build strings every frame unless the text actually changed.
- Do not run scene-wide searches during gameplay loops.
- Pool repeated temporary visuals later, especially floating damage text, notifications, gathering progress indicators, VFX, and spawned enemies.
- Prefer simple colliders and primitive placeholders during prototyping.
- Keep shaders, materials, and UI effects modest for mobile.
- Avoid physics queries with broad masks unless the interaction/combat range is small and bounded.
- Test touch controls and PC fallback after any input or UI change.

## Input System Rules

- Gameplay scripts should use `WildsOfDracoria.Inputs.DracoriaInput` instead of direct `UnityEngine.Input` calls.
- Direct legacy input calls are allowed only inside the input compatibility layer.
- When the Unity Input System package is enabled, generated EventSystems should use `InputSystemUIInputModule`.
- During migration, support Active Input Handling set to Both before moving to Input System Package only.
- Do not hardwire new gameplay to a single device type.
- Mobile controls and PC testing controls must remain functionally equivalent.

## Save/Load Rules

- Preserve backward compatibility with existing JSON saves whenever practical.
- Always normalize IDs after loading save data.
- Keep saved inventory as `itemId` plus quantity.
- Save profession state through `ProfessionData`, not old skill-only pathways.
- Do not store scene object references in save data.
- Add null guards for lists and nested serializable data because older saves may omit newer fields.
- If a migration is risky, document it in `Docs/TechnicalDebt.md` before changing the save format.

## UI Rules

- Existing prototype UI may remain uGUI while systems are still changing quickly.
- Do not perform a broad UI Toolkit rewrite during unrelated feature work.
- New persistent menu panels should be designed so they can later migrate to UI Toolkit/UXML/USS.
- Combat HUD, virtual joystick, and action buttons may remain uGUI until mobile playtesting proves the best path.
- UI scripts should refresh from managers/data models and avoid owning gameplay truth.
- Avoid nested card-heavy UI and keep mobile text readable.
- Keep button labels short and touch targets large enough for phones.

## ScriptableObject and Data-Driven Rules

- Static registries are acceptable during early prototype work.
- As content grows, promote items, recipes, professions, race visuals, gathering nodes, enemies, and dialogue to ScriptableObjects.
- Data definitions should use stable IDs and avoid scene-only assumptions.
- Future content should be Addressables-ready: reference by stable ID first, asset reference second.
- Do not duplicate item, profession, recipe, node, or race IDs across systems.

## No-Scope-Creep Rules

- Build only the system requested by the user.
- Cleanup passes may only modernize, document, fix warnings, or reduce risk.
- Do not add quests, networking, economy, sailing, cities, guilds, or new professions unless the user explicitly asks.
- Do not redesign gameplay while doing compatibility or standards work.
- If a useful idea appears but is outside scope, add it to `Docs/TechnicalDebt.md` or the next-steps list instead of implementing it.

## Required Handoff Behavior

After every meaningful change:

- List every file created or changed.
- Explain why each non-trivial modification was made.
- Give Unity testing steps when code or scene behavior changed.
- Update `Docs/CodexHandoff.md` if the project state or next system guidance changes.
- Update `Docs/TechnicalDebt.md` if known issues remain.
