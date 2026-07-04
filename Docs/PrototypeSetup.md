# Prototype Setup Guide

## Goal

Build the first playable slice of Wilds of Dracoria: a third-person player in Ironhaven with interaction, fishing, inventory, skills, NPC dialogue, local save/load, basic combat, Forest Wolf enemy AI, and mobile-friendly controls.

## Create the Scene

1. Open the Unity project.
2. Create a new empty scene.
3. Save it as `Assets/WildsOfDracoria/Scenes/IronhavenPrototype.unity`.
4. From the Unity top menu, choose `Wilds of Dracoria > Build Ironhaven Prototype Scene`.
5. Then choose `Wilds of Dracoria > Add Mobile Controls To Current Scene`.
6. Save the scene again.
7. Press Play.

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
- Tap SKL, MAP, and CHR to show placeholder notifications.
- Defeat a wolf and confirm XP popup notifications and possible inventory drops.
- Verify the 6-slot action bar appears empty and reusable.

## System 003 Connections

- `VirtualJoystick` produces movement input.
- `MobileControlsRouter` sends joystick/button state to existing player movement, combat, interact, and inventory scripts.
- `MobileActionButton` maps UI button press/release/click events to router actions.
- `CameraDragInput` sends drag deltas to `ThirdPersonCameraFollow`.
- `ActionBarUI` owns six empty reusable slots.
- `NotificationPopupUI` displays short menu and XP messages.
- `MobileControlsSceneBuilder` creates the mobile UI in the current scene without changing gameplay systems.

## Current Prototype Boundaries

System 003 intentionally does not add quests, professions, networking, new combat abilities, new economy logic, or new gameplay loops. It only creates a mobile control and UI foundation for the existing prototype.
