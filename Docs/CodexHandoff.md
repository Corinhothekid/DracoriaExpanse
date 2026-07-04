# Codex Handoff

This file gives Codex the working vision and development guardrails.

## Project

**Wilds of Dracoria**: a mobile-first living fantasy MMO prototype built in Unity.

## Style Target

Classic WoW-inspired stylized fantasy, not photorealistic.

Important visual rules:

- Strong silhouettes
- Bright readable color
- Fantasy warmth
- Low-to-mid poly assets
- Mobile-friendly performance
- Clear UI readability
- Cozy towns, dangerous wilderness, alive harbors

## Core Vision

A living fantasy MMO where players build lives, families, professions, communities, cities, ships, guilds, and histories.

The final game will include:

- No fixed classes
- Skill-based progression
- Professions with endgame
- Player economy
- Local markets
- Player shops
- Homesteads and communities
- Guild cities
- Sailing
- Player-run shipping
- NPC early-game transport
- Piracy
- Crown Isles faction campaigns
- Dynasties/family lines
- Living NPC routines
- Server chronicle

## Current Prototype Scope

Do not build the full MMO yet.

Current focus:

1. Third-person movement
2. Mobile controls
3. Character data
4. Skills
5. Inventory
6. Basic fishing
7. Basic combat
8. NPC interaction
9. Save/load
10. Ironhaven starter village

## What Codex Should Do Next

### Immediate Next System

**System 003: Mobile Controls & Action Bar**

Build only:

1. Virtual joystick for movement
2. Camera swipe/drag support
3. Mobile attack button
4. Mobile block button
5. Mobile dodge button
6. Mobile sprint button
7. Mobile interact button
8. Reusable action bar with 6 empty slots
9. Basic menu buttons for inventory, skills, map, character
10. Simple notification/XP popup system

### Do Not Add Yet

- New quests
- Networking
- Guilds
- Cities
- Sailing
- Full economy
- Full profession system
- Crown Isles
- Dynasties beyond basic familyName data
- AI world simulation
- Extra features not requested

## Development Rules

1. Explain the implementation plan before coding.
2. Keep code modular.
3. Preserve existing systems unless a change is required for integration.
4. Do not expand scope without asking.
5. Support PC testing and mobile touch.
6. Prefer reusable UI components.
7. Keep systems data-driven where possible.
8. Every script should be easy to expand later.
9. Avoid hardcoding future gameplay assumptions.
10. After changes, list created/modified files and Unity test steps.

## Project Architecture Target

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
      Professions/
      Save/
      Systems/
      UI/
      World/
Docs/
```

## MVP Sequence

1. Movement
2. Combat
3. Mobile controls and action bar
4. Profession framework
5. Fishing polish
6. Mining
7. Blacksmithing
8. Cooking
9. Simple contracts/notice board
10. Homestead stub
11. Simple boat/ferry stub
12. Local market stub
13. Dynasty/family UI stub
14. World manager with time/weather

## Long-Term Guardrail

This project is not just a combat RPG. It is a living world where every system should eventually support one or more of these:

- Player identity
- Community
- Economy
- Travel
- Profession mastery
- World history
- Legacy

If a feature does not help the world feel alive, pause and ask before adding it.
