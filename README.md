# Space Invaders 3D - CSCI 381 Assignment 1

**Student:** Donovan Gonzalez - 24364879

**Course:** CSCI 381, Video Game Design and Development

**Engine:** Unity 6 (6000.6.0f1), URP

**Starting point:** Started it on my own, then all files were deleted because of mess up in OneDrive. Then used Professor's Space-Invaders-3D project to start over.


AI assistance (Claude) was used while building this, as allowed by the course. No copyrighted assets are used. All art is built from Unity primitives, Unity's built-in particle system, and TextMeshPro.

## Controls
- Left / Right arrow keys: move the ship
- Space bar: shoot a dart

## How I met the project requirements

### Required features

| Requirement | How it is met | Where |
|---|---|---|
| Remaining aliens speed up as others are destroyed | The fleet's speed is interpolated from a slow base speed (0.1) to a fast max speed (0.3) based on the fraction of aliens destroyed. Direction is preserved with `Mathf.Sign(speed)`. | `AlienFleet.FixedUpdate` |
| Detect when the game can no longer be won | The ship only shoots upward, so once any alien is at or below the ship's height nothing can ever hit it. The fleet checks every alien's Y against the ship's Y and ends the game ("GAME OVER") when that happens. | `AlienFleet.CheckCantWin` |
| Prevent the spaceship from moving offscreen | After movement, the ship's X position is clamped between `leftBound` and `rightBound`. | `Spaceship.Update` |
| Find 3+ problems in the professor's code and fix them | See "Problems found in the professor's code" below (5 fixes). | various |

### Problems found in the professor's code (and fixes)

1. **Redundant `GetComponent` in `Alien.OnCollisionEnter`.**
   The original called `fleet.GetComponent<AlienFleet>().aliens.Remove(gameObject)`, but `fleet` is already typed as `AlienFleet`, so the lookup does nothing useful and costs time on every collision. Fixed: `fleet.aliens.Remove(gameObject)`.

2. **Empty `Start()` and `Update()` methods.**
   Unity still calls empty `Update()` methods every frame, which adds up with many aliens. Removed from `Alien.cs`, `AlienBullet.cs` and `Spaceship.cs`.

3. **Hard-coded screen bounds that did not match the camera.**
   `AlienFleet` used fixed bounds of +/-16, but the camera shows less than that, so the right side of the fleet left the screen before turning around. Fixed: the bounds are computed in `Start()` from `Camera.main.orthographicSize * Camera.main.aspect`.

4. **Collisions destroyed anything they touched.**
   `Alien.OnCollisionEnter` and `AlienBullet.OnCollisionEnter` both did `Destroy(coll.gameObject)` with no checks, so aliens popped when hit by each other's bullets, and a bullet that hit the ship would have deleted the ship. Fixed: added physics layers (Alien, AlienBullet, Dart, Player) with the Layer Collision Matrix set so unrelated objects ignore each other, and each handler checks what it hit (the ship loses a life instead of being destroyed).

5. **Dead, commented-out code.**
   `AlienFleet` contained a commented-out `lowerBound` block. Removed, and replaced by the real "game can't be won" check above.

### Extra features (four)

| Extra | How it is met | Where |
|---|---|---|
| Lives with a graphic display | The ship has 3 lives. Each alien bullet that hits costs one. Three red hearts are shown in the bottom-left corner and disappear as lives are lost. | `ShipHealth.cs`, `HeartsUI.cs`, `AlienBullet.cs` |
| End screens | A black GAME OVER screen appears when the player loses (no lives, an alien touches the ship, or the game can't be won). A YOU WIN screen appears when every alien is destroyed. Both are hidden during normal play. | `AlienFleet.GameOver`, `AlienFleet.YouWin`, UI panels in the Canvas |
| Collision animation | A green particle explosion plays where an alien is hit. | `Alien.OnCollisionEnter`, `Explosion` prefab |
| Replaced background | The default sky is replaced by a dark night-sky color with a particle starfield. | Main Camera background, `Stars` particle system |

### Additional polish
- Custom ship model built from Unity primitives (body, cockpit, wings) with a particle-system engine flame.
- Brighter, glowing alien bullets so they stand out from the stars.

## Scripts changed or added
- `AlienFleet.cs` (speed-up, camera-based bounds, can't-win check, game over / win)
- `Alien.cs` (collision cleanup, explosion)
- `AlienBullet.cs` (damages the ship instead of destroying it)
- `Spaceship.cs` (movement clamp, cleanup)
- `ShipHealth.cs` (new - lives)
- `HeartsUI.cs` (new - hearts display)
