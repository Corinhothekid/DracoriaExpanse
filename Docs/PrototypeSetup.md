# Prototype Setup Guide

## Goal

Build the first tiny playable slice of Wilds of Dracoria: a third-person player in the village of Ironhaven with interaction, fishing, inventory, skills, NPC dialogue, and local save/load.

## Create the Scene

1. Open the Unity project.
2. Create a new empty scene.
3. Save it as `Assets/WildsOfDracoria/Scenes/IronhavenPrototype.unity`.
4. From the Unity top menu, choose `Wilds of Dracoria > Build Ironhaven Prototype Scene`.
5. Save the scene again.
6. Press Play.

## Test Checklist

- Move with WASD or arrow keys.
- Hold Left Shift to run.
- Press Space to jump.
- Walk near Captain Alden and press E.
- Walk to the fishing spot near the dock and press E.
- Wait for the fishing timer to finish.
- Press I to see the caught fish in inventory.
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

## Mobile Input Notes

`ThirdPersonPlayerController` exposes methods designed for future mobile controls:

- `SetMoveInput(Vector2 input)`
- `SetRunInput(bool isRunning)`
- `Jump()`
- `Interact()`

A mobile joystick can call `SetMoveInput`, while touch buttons can call `Jump` and `Interact`.

## Current Prototype Boundaries

This prototype intentionally does not include networking, combat, live economy, real quests, guilds, ships, or persistence beyond local JSON. Those should come after this slice feels good to play.
