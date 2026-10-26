# Dracoria Evolved Build 016

## Status
- `Codea/Main.lua` is committed to GitHub.
- `TydusAtlas.png` (384 × 1792, transparent PNG) is packaged separately, **not yet committed**.
- The Codea game has not been verified on an iPhone.

## iPhone setup
1. Download the Build 016 ZIP supplied in the ChatGPT conversation.
2. Extract it in Files.
3. In Codea, create/open `Dracoria Evolved` and replace the Main tab with `Codea/Main.lua` from GitHub.
4. Import `TydusAtlas.png` into Codea Documents assets with the asset name `TydusAtlas`.
5. Run. Use the left joystick and right attack button.

## Atlas contract
- Sheet: 384 × 1792 pixels.
- Cell: 96 × 112 pixels.
- 4 columns × 16 rows.
- First 8 rows: walk; next 8: attack.
- Row order within each animation: down, down-left, left, up-left, up, up-right, right, down-right.
- Some poses were approximated from the provided reference. Visual and Codea runtime validation remain outstanding.
