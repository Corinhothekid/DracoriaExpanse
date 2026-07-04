# Prototype Setup Guide

## Goal

Build the first playable slice of Dracoria Expanse: a third-person player in Ironhaven with interaction, fishing, inventory, skills, NPC dialogue, local save/load, basic combat, Forest Wolf enemy AI, mobile-friendly controls, the reusable profession framework, and a central item database.

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
- Confirm Fishing, Mining, Blacksmithing, Cooking, Sailing, Merchanting, Logging, Farming, Hunting, and Navigation all appear.
- Confirm locked professions still appear.
- Interact with the fishing spot and confirm Fishing Profession XP popup appears.
- Save with F5 and load with F9, then reopen the profession list and confirm Fishing profession progress remains.

Item database checks:

- Catch fish at the fishing spot and confirm the inventory shows display names such as Small Silverfin, River Trout, Old Boot, or Rare Golden Carp.
- Defeat a Forest Wolf and confirm any drops display as Wolf Pelt, Raw Meat, or Small Fang.
- Save with F5 and load with F9, then confirm inventory quantities remain.
- Confirm old display-name saves still load by normalizing into item IDs.
- Confirm no crafting, shops, or economy loops were added yet.

## System 004 Connections

- `ProfessionData` stores saveable profession state: id, display name, description, level, XP, mastery, reputation, unlocks, and journal entries.
- `ProfessionDefinition` describes registry entries.
- `ProfessionRegistry` lists current and future professions.
- `ProfessionManager` initializes profession data, grants XP, unlocks professions, fires events, refreshes UI, and uses existing notification popups.
- `ProfessionUI` displays every profession, including locked professions.
- `CharacterData` owns the saved profession list.
- `JsonSaveSystem` persists professions through the existing local JSON save.
- `FishingSpot` grants Fishing Profession XP without changing the fishing interaction loop.
- `ProfessionFrameworkSceneBuilder` adds the profession UI to the current scene.

## System 005 Connections

- `ItemDefinition` describes each item with id, display name, description, type, rarity, stack limit, value, weight, icon placeholder, and gameplay flags.
- `ItemType` and `ItemRarity` provide shared categories for inventory, loot, crafting, economy, and UI systems.
- `ItemIds` stores stable constants and normalizes older display-name inventory saves into item IDs.
- `ItemDatabase` registers all starter item definitions and provides lookup, validation, display name, stack limit, and type filtering helpers.
- `InventoryItem` now stores `itemId` and `quantity`.
- `InventoryUI` displays player-friendly names by looking up item IDs in `ItemDatabase`.
- `FishingSpot` awards item IDs from the database instead of raw item names.
- `EnemyDropTable` and `EnemyHealth` roll and award database item IDs for wolf drops.
- `CharacterData`, `GameManager`, and `JsonSaveSystem` normalize inventory before adding, saving, or loading items.

## Current Prototype Boundaries

System 005 intentionally does not add quests, networking, cities, sailing, economy systems, shops, crafting gameplay, new gathering gameplay, or full profession gameplay loops. It only creates reusable item architecture and migrates the existing fishing and wolf drop rewards into it.