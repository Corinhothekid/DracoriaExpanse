# Prototype Setup Guide

## Goal

Build the first playable slice of Wilds of Dracoria: a third-person player in Ironhaven with interaction, fishing, inventory, skills, NPC dialogue, local save/load, basic combat, Forest Wolf enemy AI, mobile-friendly controls, and the reusable profession framework.

## Create the Scene

1. Open the Unity project.
2. Create a new empty scene.
3. Save it as `Assets/WildsOfDracoria/Scenes/IronhavenPrototype.unity`.
4. From the Unity top menu, choose `Wilds of Dracoria > Build Ironhaven Prototype Scene`.
5. Choose `Wilds of Dracoria > Add Mobile Controls To Current Scene`.
6. Choose `Wilds of Dracoria > Add Profession Framework To Current Scene`.
7. Save the scene again.
8. Press Play.

## Test Checklist

PC checks:

- Move with WASD or arrow keys.
- Hold Left Shift to sprint and watch stamina drain.
- Stop sprinting and watch stamina regenerate.
- Middle mouse drag to rotate the camera.
- Press Space to jump.
- Walk near Captain Alden and press E.
- Walk to the fishing spot near the dock and press E.
- Left click to attack a Forest Wolf.
- Right click to block.
- Press Left Alt to dodge.
- Press I to toggle inventory.

Mobile UI checks in Play Mode:

- Use the left joystick to move.
- Drag the open screen area to rotate the camera.
- Hold RUN to sprint and drain stamina.
- Tap USE near NPCs, the fishing spot, forge, or notice board.
- Tap ATK to attack.
- Hold BLK to block.
- Tap DOD to dodge.
- Tap INV to toggle inventory.
- Tap SKL to toggle the profession list.
- Tap MAP and CHR to show placeholder notifications.
- Defeat a wolf and confirm XP popup notifications and possible inventory drops.
- Verify the 6-slot action bar appears empty and reusable.

Profession framework checks:

- Open the profession list with SKL.
- Confirm Fishing, Mining, Blacksmithing, Cooking, Farming, Logging, Merchant, Navigator, Shipwright, Carpenter, and Hunter all appear.
- Confirm locked professions still appear.
- Interact with the fishing spot and confirm Fishing Profession XP popup appears.
- Save with F5 and load with F9, then reopen the profession list and confirm Fishing profession progress remains.

## System 004 Connections

- `ProfessionData` stores saveable profession state.
- `ProfessionDefinition` describes registry entries.
- `ProfessionRegistry` lists current and future professions.
- `ProfessionManager` initializes profession data, grants XP, unlocks professions, refreshes UI, and uses existing notification popups.
- `ProfessionUI` displays every profession, including locked professions.
- `CharacterData` owns the saved profession list.
- `JsonSaveSystem` persists professions through the existing local JSON save.
- `FishingSpot` now grants Fishing Profession XP without changing the fishing interaction loop.
- `ProfessionFrameworkSceneBuilder` adds the profession UI to the current scene.

## Current Prototype Boundaries

System 004 intentionally does not add quests, networking, cities, sailing, economy systems, or full profession gameplay loops. It only creates reusable profession architecture and migrates the existing Fishing XP path into it.
