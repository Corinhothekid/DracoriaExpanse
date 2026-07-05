# Codex Handoff

This file gives Codex the working vision and development guardrails.

## Project

**Dracoria Expanse**: a mobile-first living fantasy MMO prototype built in Unity.

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

Current foundation includes:

1. Third-person movement
2. Mobile controls
3. Character data and character creation
4. Inventory and item database
5. Fishing
6. Combat and enemy AI
7. NPC interaction
8. Profession framework
9. Crafting framework
10. Gathering nodes
11. Contracts and Notice Board
12. Visual customization architecture
13. JSON save/load
14. Ironhaven starter village
15. Unity 6.5 compatibility modernization pass
16. Project standards and technical debt tracking

## What Codex Should Do Next

Follow the latest user request. Before adding any new system, read:

- `README.md`
- `Docs/CodexHandoff.md`
- `Docs/EngineeringStandards.md`
- `Docs/TechnicalDebt.md`
- Any relevant bible or system document named by the user

If no new system is specified, prefer stability, Unity compile fixes, and documentation over adding features.

### Do Not Add Unless Requested

- New quests beyond the contract foundation
- Networking
- Guilds
- Cities
- Sailing
- Full economy
- Crown Isles
- Dynasties beyond existing identity/family data
- AI world simulation
- Extra features not requested

## Development Rules

1. Always check `Docs/EngineeringStandards.md` before coding.
2. Always update `Docs/TechnicalDebt.md` when leaving known issues, risky shortcuts, or deferred migrations.
3. Never add new systems during cleanup passes.
4. Explain the implementation plan before coding when the user asks for it or when the change is substantial.
5. Keep code modular.
6. Preserve existing systems unless a change is required for integration or compatibility.
7. Do not expand scope without asking.
8. Support PC testing and mobile touch.
9. Prefer reusable UI components.
10. Keep systems data-driven where possible.
11. Every script should be easy to expand later.
12. Avoid hardcoding future gameplay assumptions.
13. After changes, list created/modified files and Unity test steps when behavior changed.

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
      Player/
      Professions/
      Save/
      Systems/
      UI/
        Mobile/
      Visuals/
Docs/
```

## MVP Sequence

Completed foundation:

1. Movement
2. Combat
3. Mobile controls and action bar
4. Profession framework
5. Item database
6. Crafting framework
7. Character creation
8. Character visual customization architecture
9. Gathering nodes
10. Unity 6.5 compatibility pass
11. Project standards and technical debt tracker
12. Contracts and Notice Board

Possible next systems, only when requested:

1. Unity compile/playtest fix pass
2. Contract board usability polish
3. Gathering/crafting balance polish
4. Simple bread and cooking loop polish
5. Fishing polish
6. Mining and blacksmithing expansion
7. Homestead stub
8. Simple boat/ferry stub
9. Local market stub
10. Dynasty/family UI stub
11. World manager with time/weather

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
