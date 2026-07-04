# Wilds of Dracoria

Mobile-first fantasy MMO prototype foundation built for Unity.

Working title: **Wilds of Dracoria**  
Repository name: **DracoriaExpanse**

## Prototype Goal

This first slice creates a small third-person playable scene called **Ironhaven**. It proves the foundation for MMO-style movement, interaction, character data, skills, inventory, fishing, NPC dialogue, and local save/load.

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
- `Left Shift`: Run
- `Space`: Jump
- `E`: Interact
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
      Camera/
      Data/
      Editor/
      Interaction/
      Player/
      Save/
      Systems/
      UI/
```

## Next Steps

1. Add mobile joystick and action buttons.
2. Replace primitive placeholders with low-poly fantasy art.
3. Add mining, blacksmithing, cooking, and sailing loops.
4. Add Captain Alden's first quest.
5. Add character creation and family names.
6. Add day/night lighting and ambient audio.
7. Add a simple local economy.
8. Add networking only after the single-player prototype feels good.
