# Dawnhaven World Specification (Build 017)

**Status:** First scaled, procedurally laid-out world prototype; not yet verified on the user's iPhone.

## World scale
- 1 logical tile = 2 metres.
- Isometric ground projection = 48 pixels wide × 24 pixels high per tile.
- Playable logical bounds = -90 to +90 on each axis, about 362 metres per side.
- City wall bounds = approximately -34 to +34 tiles on each axis, about 136 metres across.
- Roads connect the central square to four gates. District avenues run at +/-18 tiles.
- Buildings have blocking footprints and render in depth order with the player.
- Woodlands populate beyond the maintained perimeter, with collision around trunks.

## Art and performance
- Terrain currently uses flat-shaded procedural tiles with low-frequency variation, not finished painted terrain textures.
- The current render loop is a prototype and may need batching/chunking for mobile performance.
- Sprite rendering expects `TydusAtlas.png` imported into Codea Documents assets. GitHub binary upload of that atlas remains unconfirmed.
- No quests, interiors, NPC pathfinding, combat damage, or save system yet.

## Next priorities
1. Verify Codea Lua runtime and frame orientation on iPhone.
2. Fix performance issues using chunked ground meshes or cached tile textures.
3. Replace placeholder building/forest shapes with authored texture atlases.
4. Add landmarks, district names, gate guards, NPCs, and first quests.
