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

## Prototype Goal

This prototype creates a small third-person playable scene called **Ironhaven** and now includes **System 003: Mobile Controls & Action Bar**. It proves the foundation for MMO-style movement, interaction, character data, skills, inventory, fishing, NPC dialogue, combat, enemy drops, mobile input, and local save/load.

## Included Systems

- Third-person WASD movement with run and jump
- Camera follow with swipe/drag support
- Virtual joystick movement
- Mobile attack, block, dodge, sprint, and interact buttons
- Reusable 6-slot empty action bar
- Basic menu buttons for Inventory, Skills, Map, and Character
- Notification and XP popup foundation
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
- Basic attack, block, and dodge
- Weapon data foundation
- Damage system with `IDamageable`
- Forest Wolf enemy AI
- Enemy drops and combat XP
- Player and enemy combat UI

## Unity Setup

1. Create or open a Unity 3D project.
2. Use this repository as the project root, or copy `Assets/WildsOfDracoria` into an existing Unity project.
3. Open the project in Unity and let scripts compile.
4. Create an empty scene named `IronhavenPrototype` under `Assets/WildsOfDracoria/Scenes`.
5. Choose `Wilds of Dracoria > Build Ironhaven Prototype Scene`.
6. Choose `Wilds of Dracoria > Add Mobile Controls To Current Scene`.
7. Save the scene.
8. Press Play.

## Controls

PC testing:

- `WASD` / arrow keys: Move
- `Middle Mouse Drag`: Rotate camera
- `Left Shift`: Sprint
- `Space`: Jump
- `E`: Interact
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
- `USE`: Interact
- `INV`, `SKL`, `MAP`, `CHR`: Basic menu buttons

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
        Mobile/
Docs/
```

## Next Steps

1. Replace primitive placeholders with low-poly fantasy art.
2. Add enemy spawn points and respawning.
3. Tune touch camera sensitivity after device testing.
4. Add professions and quests later.
5. Add networking only after the single-player prototype feels good.
