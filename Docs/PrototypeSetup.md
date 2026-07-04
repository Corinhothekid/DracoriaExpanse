# Prototype Setup Guide

## Goal

Build the first playable slice of Dracoria Expanse: a third-person player in Ironhaven with interaction, fishing, inventory, local save/load, combat, mobile controls, professions, item database, crafting framework, character creation, visual customization architecture, and gathering nodes.

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
- Tap USE near NPCs, the fishing spot, forge, campfire, notice board, or gathering nodes.
- Tap ATK to attack.
- Hold BLK to block.
- Tap DOD to dodge.
- Tap INV to toggle inventory.
- Tap SKL to toggle the profession list.
- Tap CHR to open the character panel.
- Verify the 6-slot action bar appears empty and reusable.

Profession and item checks:

- Open the profession list with SKL.
- Confirm Fishing, Mining, Blacksmithing, Cooking, Logging, and Farming are available for prototype testing.
- Catch fish and confirm Fishing Profession XP appears.
- Defeat a wolf and confirm possible inventory drops.
- Save with F5 and load with F9, then confirm inventory and profession progress remain.

Gathering checks:

- Find Copper Veins near the rocky edge west of Ironhaven.
- Interact with a Copper Vein and confirm it requires Mining level 1 and uses the starter Beginner Pickaxe.
- Confirm Copper Ore enters inventory and Mining XP appears.
- Find Oak Trees near the forest edge and gather Oak Logs.
- Find Wheat Patches near the farm area and gather Wheat.
- Confirm gathered nodes disappear/deplete, then respawn after their timer.
- Save with F5 and load with F9; inventory and profession XP should persist, node respawn state does not need to persist yet.

Character and visual checks:

- If no save exists, the character creation panel appears before player control is enabled.
- Cycle race and appearance options and confirm the placeholder preview updates.
- Confirm the character and verify control returns.
- Open the character panel and confirm name, house, race, homeland, gold, profession focus, and appearance text appear.

Crafting checks:

- Open the forge or campfire crafting panel.
- Confirm recipes show required ingredients and missing item warnings.
- Gather Copper Ore and Oak Logs, then use the forge recipe path for Training Sword once ingredients are present.

## System 009 Connections

- `GatheringNodeDefinition` describes node id, display name, profession requirement, optional tool, gather time, respawn time, loot table, XP reward, and depletion behavior.
- `GatheringLootEntry` stores itemId, quantity range, and drop chance.
- `GatheringNodeRegistry` registers Copper Vein, Oak Tree, and Wheat Patch prototypes.
- `GatheringNode` is an interactable component that checks profession level/tool requirements, runs a gathering timer, awards ItemDatabase item IDs, awards Profession XP, depletes, and respawns.
- `GatheringNodeSceneBootstrap` adds placeholder Ironhaven gathering nodes at runtime if the scene has none.
- `ItemIds` and `ItemDatabase` now include Wheat.
- `ProfessionRegistry` unlocks Mining, Logging, and Farming for prototype testing.
- `CharacterData` grants a starter Beginner Pickaxe if missing, so Copper Vein testing does not require dev-spawned tools.
- Node respawn state is runtime-only for now; inventory and profession XP continue to persist through existing save/load.

## Current Prototype Boundaries

System 009 intentionally does not add quests, economy, shops, networking, advanced gathering animations, persistent node respawn state, rare resource discovery, or full profession gameplay loops. It only creates the reusable world gathering framework and three prototype node types.