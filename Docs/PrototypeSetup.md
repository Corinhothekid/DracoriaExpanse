# Prototype Setup Guide

## Goal

Build the first playable slice of Wilds of Dracoria: a third-person player in the village of Ironhaven with interaction, fishing, inventory, skills, NPC dialogue, local save/load, basic combat, and Forest Wolf enemy AI.

## Create the Scene

1. Open the Unity project.
2. Create a new empty scene.
3. Save it as `Assets/WildsOfDracoria/Scenes/IronhavenPrototype.unity`.
4. From the Unity top menu, choose `Wilds of Dracoria > Build Ironhaven Prototype Scene`.
5. Save the scene again.
6. Press Play.

## Test Checklist

- Move with WASD or arrow keys.
- Hold Left Shift to sprint and watch stamina drain.
- Stop sprinting and watch stamina regenerate.
- Press Space to jump.
- Walk near Captain Alden and press E.
- Walk to the fishing spot near the dock and press E.
- Wait for the fishing timer to finish.
- Press I to see caught fish and combat drops in inventory.
- Find a Forest Wolf near the edge of Ironhaven.
- Left click to attack.
- Right click to block incoming damage.
- Press Left Alt to dodge.
- Defeat a wolf and confirm Swordsmanship/Endurance XP and possible drops.
- Press F5 to save.
- Press F9 to load.

## Scene Areas

- Ironhaven village green
- Harbor water
- Main dock
- Blacksmith forge
- Fishing spot
- Notice board
- Captain Alden and placeholder NPCs
- Forest Wolf patrol area

## Combat Connections

- `PlayerVitals` owns player health and stamina and implements `IDamageable`.
- `ThirdPersonPlayerController` drains stamina while sprinting.
- `PlayerCombat` reads attack, block, and dodge input and uses `WeaponData` from `WeaponDatabase`.
- `EnemyHealth` owns wolf health, death, XP rewards, and drop rolls.
- `EnemyAI` handles idle, patrol, chase, attack, and return states.
- `CombatUI` shows player health, player stamina, and the current enemy target health.
- `GameManager` still owns inventory, character data, skill XP, save/load, and UI messages.

## Mobile Input Notes

`ThirdPersonPlayerController` and `PlayerCombat` expose methods designed for future mobile controls:

- `SetMoveInput(Vector2 input)`
- `SetRunInput(bool isRunning)`
- `Jump()`
- `Interact()`
- `BasicAttack()`
- `Dodge()`

A mobile joystick can call `SetMoveInput`, while touch buttons can call `Jump`, `Interact`, `BasicAttack`, and `Dodge`.

## Current Prototype Boundaries

This prototype intentionally does not include networking, advanced abilities, ranged bow combat, a quest system, guilds, ships, or a live economy. Those should come after movement, combat feel, inventory, and profession loops are sturdy.
