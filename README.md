# Wilds of Dracoria

Mobile-first fantasy MMO prototype foundation built for Unity.

Working title: **Wilds of Dracoria**  
Repository name: **DracoriaExpanse**  
Style target: **Classic WoW-inspired stylized fantasy**, with RuneScape-like life skills and long-term player identity.

## Core Pitch

Wilds of Dracoria is a living fantasy MMO where players do not simply level up. They build lives, families, professions, communities, cities, trade routes, ships, guilds, and histories.

Players can become warriors, fishermen, blacksmiths, merchants, navigators, shipwrights, mayors, pirates, caravan guards, farmers, explorers, or anything in between. Every profession should have its own endgame, adventure, bosses, reputation, and legendary moments.

The world does not revolve around the player. It exists with the player, changes over time, and remembers what players leave behind.

## North Star

> Build a living fantasy world where every player can become legendary, regardless of how they choose to live.

## Documentation

- [Game Bible](Docs/GameBible.md)
- [World Bible](Docs/WorldBible.md)
- [Systems Bible](Docs/SystemsBible.md)
- [Codex Handoff](Docs/CodexHandoff.md)

## Prototype Goal

This prototype creates a small third-person playable scene called **Ironhaven** and now includes **System 002: Basic Combat & Enemy AI**. It proves the foundation for MMO-style movement, interaction, character data, skills, inventory, fishing, NPC dialogue, combat, enemy drops, and local save/load.

## Included Systems

- Third-person WASD movement with run and jump
- Camera follow
- Mobile-ready player input methods
- Ironhaven placeholder village scene builder
- Dock/harbor area
- Blacksmith area
- Fishing spot
- Placeholder NPCs
- Character data model
- Skill data and XP/level-up logic
- Interaction system
- Fishing prototype
- Inventory UI
- Captain Alden dialogue
- JSON save/load placeholder
- Player health and stamina
- Sprint stamina drain and stamina regeneration
- Basic attack, block, and dodge
- Weapon data foundation
- Damage system with `IDamageable`
- Forest Wolf enemy AI
- Enemy drops and combat XP
- Player and enemy combat UI

## Unity Setup

1. Create a Unity 3D project.
2. Use this repository as the project root, or copy `Assets/WildsOfDracoria` into an existing Unity project.
3. Open the project in Unity.
4. Let scripts compile.
5. Create an empty scene named `IronhavenPrototype` under `Assets/WildsOfDracoria/Scenes`.
6. In Unity, choose `Wilds of Dracoria > Build Ironhaven Prototype Scene`.
7. Save the scene.
8. Press Play.

## Controls

- `WASD` / arrow keys: Move
- `Left Shift`: Sprint
- `Space`: Jump
- `E`: Interact
- `Left Mouse`: Basic attack
- `Right Mouse`: Block
- `Left Alt`: Dodge roll placeholder
- `I`: Toggle inventory
- `F5`: Save
- `F9`: Load

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
      Combat/
      Data/
      Editor/
      Interaction/
      Player/
      Save/
      Systems/
      UI/
Docs/
```

## Next Steps

1. Add mobile joystick and combat buttons.
2. Replace primitive placeholders with low-poly fantasy art.
3. Add enemy spawn points and respawning.
4. Add mining, blacksmithing, cooking, and sailing loops.
5. Add Captain Alden's first quest.
6. Add character creation and family names.
7. Add a simple local economy.
8. Add networking only after the single-player prototype feels good.
