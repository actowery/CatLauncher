# Cat Launcher

![Workshop preview](About/Preview.png)

A standalone RimWorld mod by **konduct** for RimWorld **1.6** and **1.7 unstable**.

Build a reusable cat launcher at the machining table after researching Machining. It fires a visibly spinning, randomly tinted cat shell with frag-grenade-equivalent impact damage: 1.9 tiles of Bomb damage, a 24-tile range, 2.5 seconds of warmup, and a 3-second cooldown.

## What to expect

- Every ordinary shot is entirely visual: it does not consume, hurt, or create a real cat.
- Impacts leave cleanable vanilla blood and short-lived, non-selectable cat-shaped visual fragments.
- On a 1% roll, a wounded wild cat appears in a nearby safe cell after the explosion.
- The launcher costs 80 steel, 4 components, and 50 chemfuel to make.

## Installation

Subscribe on Steam Workshop, or unpack a release archive into RimWorld's `Mods` folder. Enable Cat Launcher in the mod list; it has no settings and no required mod dependencies.

## Development and verification

`scripts/validate-package.ps1 -RequireBinaries` validates metadata, required definitions, version folders, and preview size. `scripts/package.ps1` creates the Workshop-ready archive in `outputs/`. The CI runs the test suite for both supported game targets, publishes Cobertura coverage to Codecov, performs CodeQL analysis, and checks dependency changes.

See [MANUAL_TESTS.md](MANUAL_TESTS.md) for the in-game verification checklist.
