# Tydus sprite atlas, first pass

These are actual transparent PNG atlases, generated with pixel geometry, not production illustration.

Each image is 192×512 pixels, with 48×64 frames: four animation columns × eight direction rows.

Layers: `tydus_body.png`, `tydus_armor.png`, `tydus_hair.png`.

Directions top-to-bottom: front, front-right, right, back-right, back, back-left, left, front-left.

Frames left-to-right: contact-left, passing, contact-right, passing.

All sheets share identical anchors and can be composited in order: body → armor → hair.

**Status:** Starter sprite assets. Not yet wired into Codea's renderer or tested on device. Pixel artwork and walk-cycle poses require refinement.
