# Dracoria Expanse

Mobile-first fantasy MMO prototype foundation built for Unity.

Working title: **Dracoria Expanse**  
Repository name: **DracoriaExpanse**  
Style target: **Classic WoW-inspired stylized fantasy**, with RuneScape-like life skills and long-term player identity.

## Core Pitch

Dracoria Expanse is a living fantasy MMO where players do not simply level up. They build lives, families, professions, communities, cities, trade routes, ships, guilds, and histories.

Players can become warriors, fishermen, blacksmiths, merchants, navigators, shipwrights, mayors, pirates, caravan guards, farmers, cooks, founders of family dynasties, or anything in between. Every profession should have its own endgame, adventure, bosses, reputation, and legendary moments.

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

This prototype creates a small third-person playable scene called **Ironhaven** and now includes **System 012: Local Market & Vendor Stall Foundation**. It proves the foundation for MMO-style movement, interaction, character data, inventory, fishing, combat, mobile input, professions, crafting, visual customization, centralized items, resource gathering, local contracts, local vendor stalls, and local save/load.

## Included Systems

- Third-person WASD movement with run and jump
- Mobile controls and camera swipe support
- Ironhaven placeholder village, harbor, forge, campfire, notice board, gathering nodes, and market stalls
- Character creation and character sheet foundation
- Modular placeholder character visual customization architecture
- Central ItemDatabase with item definitions, item types, rarity, stack limits, values, weight, and gameplay flags
- Inventory saved as item IDs and quantities
- Fishing rewards and Forest Wolf drops using database item IDs
- Profession data, registry, manager, save/load, and list UI
- Crafting framework with forge/campfire recipes
- Gathering framework with Copper Vein, Oak Tree, and Wheat Patch nodes
- Contract framework with available, accepted, ready, completed, failed, and abandoned states
- Starter Ironhaven contracts for fishing, gathering, combat, and crafting
- Contract Board and Contract Journal UI opened from the Notice Board
- Local market foundation with Ironhaven vendor stalls, listings, buying, stall gold balances, and saved stock state
- Player-owned stall data stubs for future market passes
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
- `E`: Interact, gather, open the Notice Board, and shop at vendor stalls
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
- `USE`: Interact, gather, and shop
- `INV`: Inventory
- `SKL`: Profession list
- `MAP`: Opens the local Contract Board / journal until a real map exists
- `CHR`: Character panel

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
      Contracts/
      Crafting/
      Data/
      Editor/
      Gathering/
      Input/
      Interaction/
      Items/
      Markets/
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
2. Polish contract and market board usability after the first playtest.
3. Add mining/crafting balance polish now that Copper Ore can be gathered.
4. Add a simple recipe ingredient loop around Wheat and Simple Bread.
5. Add low-poly/anime-style placeholder art replacements for nodes, stalls, and characters.
6. Add player selling, stall rent, economy simulation, sailing, cities, and networking later.
