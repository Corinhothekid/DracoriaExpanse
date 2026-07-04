# Art Direction

## Character Target

Dracoria Expanse characters should aim for:

```text
Classic World of Warcraft proportions
+ modern anime-inspired faces
+ stylized fantasy armor
+ vibrant readable fantasy color
```

The goal is not photorealism. Characters should have strong silhouettes, readable shapes, expressive faces, and gear that is clear on mobile screens.

## Anime-Inspired 3D Direction

Faces should eventually lean toward modern anime-inspired readability:

- Larger expressive eyes
- Clean simplified facial planes
- Appealing hair silhouettes
- Clear emotional readability
- Stylized proportions rather than realistic scans

This should still live inside a fantasy MMO body language: sturdy stances, readable armor chunks, bold weapons, capes, tools, and profession gear.

## Classic MMO Proportions

Body proportions should stay readable from a third-person camera:

- Slightly heroic hands, feet, heads, and shoulders
- Strong race silhouettes
- Armor shapes that read from a distance
- Bright faction, profession, and material colors
- Low-to-mid poly shapes suitable for mobile performance

## Modular Character System

The current prototype uses placeholder primitives only. The architecture should later support modular parts:

- Body/base mesh
- Head
- Hair
- Face preset
- Eyes
- Outfit
- Armor set
- Cape
- Main-hand item
- Off-hand item
- Profession tools such as fishing rods, hammers, pickaxes, and navigation gear

## Shared Skeleton Goal

Long term, most playable races should use one shared animation skeleton where possible. Race identity should come from scale rules, proportions, heads, materials, hair, horns, ears, tails, armor silhouettes, and animation flavor rather than requiring totally separate animation systems for every race.

Some races may need extra attachment bones later, such as horns, ears, tails, wings, or crests. Those should extend the shared rig instead of replacing it unless truly necessary.

## Toon Shader Future Goal

Future production art should explore a mobile-friendly toon or stylized shader stack:

- Soft ramp lighting
- Clean readable shadows
- Controlled rim lighting
- Saturated fantasy materials
- Hair highlights that remain readable on small screens
- Armor materials that distinguish cloth, leather, metal, scale, and wood

The current prototype does not include final shaders.

## Placeholder Reminder

System 008 intentionally does not create final models, final armor, final hair, final faces, paid cosmetics, or production art assets.

Use capsules, cubes, spheres, cylinders, and simple material colors until the gameplay and customization architecture are stable. Real Blender, CC4, or custom anime-style models should plug into the attachment and profile system later.