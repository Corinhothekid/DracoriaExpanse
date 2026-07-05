# Technical Debt Tracker

Permanent tracker for known issues, deferred upgrades, and future refactors. Update this whenever Codex leaves a known issue behind.

Priority levels:

- High: likely to block Unity 6.5/mobile builds, cause save loss, or make future systems fragile.
- Medium: should be addressed before content scale-up or serious playtesting.
- Low: useful cleanup or production hardening, but not blocking the prototype.

## Active Debt

### Missing Unity project settings and package manifests

Priority: High

Issue: The repository currently does not include `Packages/manifest.json`, `Packages/packages-lock.json`, or `ProjectSettings` files.

Why it matters: Package compatibility, active input backend, render pipeline settings, build profiles, and mobile platform settings cannot be fully validated from source alone.

Recommended fix: After opening the project in Unity 6.5, commit the package manifest, package lock file, and relevant project settings. Keep generated cache folders out of Git.

### Input System migration is compatibility-layer based

Priority: Medium

Issue: `DracoriaInput` supports Unity's newer Input System when `ENABLE_INPUT_SYSTEM` is active, but the project does not yet include a formal `.inputactions` asset or generated action wrappers.

Why it matters: The compatibility helper is safe for this prototype, but production mobile controls, rebinding, controllers, and accessibility need proper Input Actions.

Recommended fix: Add an Input Actions asset later with maps for Gameplay, UI, Combat, and Mobile. Migrate `DracoriaInput` to read actions instead of individual keyboard/mouse controls.

### Static registries should become ScriptableObjects

Priority: Medium

Issue: Items, recipes, professions, race visuals, gathering nodes, weapons, and other prototype content are stored in static registries.

Why it matters: Static registries are fast for early iteration, but content authoring, validation, localization, Addressables, and balancing will become difficult as the game grows.

Recommended fix: Promote registries to ScriptableObject definitions once each system's data shape stabilizes.

### uGUI remains the current runtime UI

Priority: Medium

Issue: Most UI is generated with uGUI code. This is acceptable for prototype speed, but persistent panels will eventually benefit from UI Toolkit.

Why it matters: Inventory, professions, crafting, character sheet, and future markets will need maintainable layouts, styling, and data binding.

Recommended fix: Keep current UI for the prototype. Later migrate persistent menu panels to UI Toolkit/UXML/USS while evaluating whether combat HUD and touch controls should remain uGUI.

### No object pooling yet

Priority: Medium

Issue: Floating damage text, notifications, gathering progress feedback, future VFX, and future spawn effects are not pooled.

Why it matters: Repeated instantiate/destroy and temporary UI allocation can cause garbage collection spikes on mobile.

Recommended fix: Add simple object pools before larger combat encounters, dense gathering areas, or mobile performance testing.

### Runtime placeholder scene construction creates temporary materials

Priority: Low

Issue: Prototype scene builders and gathering bootstrap create materials at runtime/editor time from code.

Why it matters: Fine for placeholders, but production art should use shared assets for consistency, memory, batching, and Addressables readiness.

Recommended fix: Replace generated materials with project assets when art direction assets begin landing.

### Camera.main and tag lookups remain in startup paths

Priority: Low

Issue: Some scripts still use `Camera.main`, `GameObject.FindGameObjectWithTag`, or `GameObject.Find` in startup/editor tooling paths.

Why it matters: These are not obsolete, but explicit serialized references or manager registration are cleaner and more predictable as scenes grow.

Recommended fix: Convert to serialized references or registration when prefabs/scenes are formalized.

### Save format has no explicit version field

Priority: Medium

Issue: JSON save data currently relies on null guards and normalization rather than a formal save schema version.

Why it matters: As character creation, professions, inventory, visuals, and future dynasty data expand, migrations will need predictable versioning.

Recommended fix: Add a `saveVersion` field and migration pipeline before adding account-like, dynasty, estate, or world-state data.

### Automated tests are not present

Priority: Medium

Issue: There are no Edit Mode or Play Mode smoke tests in the repository.

Why it matters: Registries, save/load normalization, item IDs, recipes, profession XP, and input compatibility can regress silently.

Recommended fix: Add Edit Mode tests for data registries and save/load, then Play Mode smoke tests for core scene interactions.

### Assembly definitions are not present

Priority: Low

Issue: All scripts currently compile in the default assembly.

Why it matters: Compile times and dependency boundaries will worsen as the project grows.

Recommended fix: Add assembly definitions after folder ownership stabilizes, likely separating runtime, editor tools, and tests.

## Resolved or Mitigated

### Obsolete `FindObjectOfType` and `FindObjectsOfType`

Priority: Resolved

Status: Replaced during the Unity 6.5 compatibility pass.

Notes: See `Docs/Unity65CompatibilityReport.md` for details.

### Direct legacy input usage across gameplay scripts

Priority: Mitigated

Status: Gameplay scripts now route through `DracoriaInput`.

Notes: Direct `UnityEngine.Input` calls remain only inside the compatibility helper as the fallback path.

### Runtime-created EventSystems using only `StandaloneInputModule`

Priority: Mitigated

Status: Generated EventSystems now add `InputSystemUIInputModule` when the new Input System is enabled, otherwise they fall back to `StandaloneInputModule`.

Notes: Full UI/input migration should wait until package settings are committed.
