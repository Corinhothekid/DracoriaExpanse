# Dracoria Expanse

Mobile-first fantasy MMO prototype foundation built for Unity.

Working title: **Dracoria Expanse**  
Repository name: **DracoriaExpanse**  
Style target: **Classic WoW-inspired stylized fantasy**, with RuneScape-like life skills and long-term player identity.

## Core Pitch

Dracoria Expanse is a living fantasy MMO where players do not simply level up. They build lives, families, professions, communities, cities, trade routes, ships, guilds, and histories.

Players can become warriors, fishermen, blacksmiths, merchants, navigators, shipwrights, mayors, pirates, caravan guards, farmers, explorers, or anything in between. Every profession should have its own endgame, adventure, bosses, reputation, and legendary moments.

The world does not revolve around the player. It exists with the player, changes over time, and remembers what players leave behind.

## North Star

> Build a living fantasy world where every player can become legendary, regardless of how they choose to live.

## Documentation

- [Game Bible](Docs/GameBible.md)
- [World Bible](Docs/WorldBible.md)
- [Systems Bible](Docs/SystemsBible.md)
- [Codex Handoff](Docs/CodexHandoff.md)
- [Engineering Standards](Docs/EngineeringStandards.md)
- [Technical Debt Tracker](Docs/TechnicalDebt.md)
- [Unity 6.5 Compatibility Report](Docs/Unity65CompatibilityReport.md)
- [Art Direction](Docs/ArtDirection.md)
- [Prototype Setup](Docs/PrototypeSetup.md)

## Prototype Goal

This prototype creates a small third-person playable scene called **Ironhaven** and now includes **System 009: Gathering Nodes**. It proves the foundation for MMO-style movement, interaction, character data, inventory, fishing, combat, mobile input, professions, crafting, visual customization, centralized items, resource gathering, and local save/load.

## Included Systems

- Third-person WASD movement with run and jump
- Mobile controls and camera swipe support
- Ironhaven placeholder village, harbor, forge, campfire, and gathering nodes
- Character creation and character sheet foundation
- Modular placeholder character visual customization architecture
- Central ItemDatabase with item definitions, item types, rarity, stack limits, values, weight, and gameplay flags
- Inventory saved as item IDs and quantities
- Fishing rewards and Forest Wolf drops using database item IDs
- Profession data, registry, manager, save/load, and list UI
- Crafting framework with forge/campfire recipes
- Gathering framework with Copper Vein, Oak Tree, and Wheat Patch nodes
- Node depletion and respawn during play
- Notification and XP popup foundation
- Captain Alden dialogue
- JSON save/load placeholder
- Player health, stamina, combat, weapon foundation, and Forest Wolf AI

## Unity Setup

1. Create or open a Unity 3D project.
2. Use this repository as the project root, or copy `Assets/WildsOfDracoria` into an existing Unity project.
3. Open the project in Unity and let scripts compile.
4. Create an empty scene named `IronhavenPrototype` under `Assets/WildsOfDracoria/Scenes`.
5. Choose `Wilds of Dracoria > Build Ironhaven Prototype Scene`.
6. Choose `Wilds of Dracoria > Add Mobile Controls To Current Scene`.
7. Choose `Wilds of Dracoria > Add Profession Framework To Current Scene`.
8. Save the scene.
9. Press Play.

## Controls

PC testing:

- `WASD` / arrow keys: Move
- `Middle Mouse Drag`: Rotate camera
- `Left Shift`: Sprint
- `Space`: Jump
- `E`: Interact and gather
- `Left Mouse`: Basic attack
- `Right Mouse`: Block
- `Left Alt`: Dodge roll placeholder
- `I`: Toggle inventory
- `F5`: Save
- `F9`: Load

Mobile UI:

- Left joystick: Move
- Drag open screen area: Rotate camera
- `ATK`: Attack
- `BLK`: Hold block
- `DOD`: Dodge
- `RUN`: Hold sprint
- `USE`: Interact and gather
- `INV`: Inventory
- `SKL`: Profession list
- `MAP`, `CHR`: Placeholder menu buttons / character panel

## Folder Structure

```text
Assets/
  WildsOfDracoria/
    Art/
    Audio/
    Prefabs/
    Scenes/
    Scripts/
      AI/
      Camera/
      CharacterCreation/
      Combat/
      Crafting/
      Data/
      Editor/
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
Docs/
```

## Next Steps

1. Run a Unity 6.5 compile/playtest fix pass after opening the project in the Editor.
2. Add mining/crafting balance polish now that Copper Ore can be gathered.
3. Add a simple recipe ingredient loop around Wheat and Simple Bread.
4. Add low-poly/anime-style placeholder art replacements for nodes and characters.
5. Add quest/contracts later, after the resource loop feels stable.
6. Add economy, sailing, cities, and networking later.