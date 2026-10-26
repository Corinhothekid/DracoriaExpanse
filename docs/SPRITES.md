# Dracoria Evolved Sprite Standard

- HD-2D visual direction: crisp pixel-art sprites in dimensional isometric scenes.
- Frame size: **48 x 64 px**.
- Eight facing directions; four walk frames each.
- Sheet dimensions: **192 x 512 px**, four columns by eight rows.
- Every layer uses the same grid, transparent background, and foot anchor.
- Layer order: shadow, rear equipment/cape, body, outfit, hair, foreground weapon/effects.
- Sprite sheets must be genuine transparent PNGs, not concept posters or simulated checkerboards.
- Keep Tydus's silver short wolf cut without a ponytail.
- Start with a single validated Human base and eight-direction walk cycle, then add modular layers.
- Keep creator preview and game rendering pointed at the same character appearance data.

## Direction rows
0: front; 1: front-right; 2: right; 3: back-right; 4: back; 5: back-left; 6: left; 7: front-left.

## Animation columns
0: contact left; 1: passing; 2: contact right; 3: passing.

## Codea integration
Import approved PNGs into the project assets, then implement nearest-neighbor frame sampling and per-layer compositing. Do not claim these sheets exist until they are actually created and validated.
