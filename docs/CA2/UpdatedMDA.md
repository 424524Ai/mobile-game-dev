# Forward — Updated MDA Design Document (CA2)

**Project:** Forward  
**Genre:** Endless Runner  
**Platform:** Android  
**Version:** 0.8.0  
**Design status:** Updated after Week 8 Lab A

## One-line pitch

An endless runner where a game-loving student tries to escape school, dodging teachers, exams and homework while collecting game consoles to upgrade their speed.

## Theme / Setting

Forward follows a student who loves playing video games but dislikes studying. Instead of staying at school, the student decides to escape and spend more time gaming.

However, escaping is not easy. Teachers, exam banners and homework stand in the student's way as obstacles. The player must switch lanes, jump and roll to avoid them while running as far as possible.

Along the way, the player collects game consoles, which represent more opportunities to play video games. These collectibles act as the game's currency and can be spent on speed upgrades, allowing the student to run faster and attempt to escape school more quickly.

However, there is a twist: Forward is an endless runner. No matter how fast the student runs or how many upgrades they purchase, they can never truly escape school. The goal is simply to survive for as long as possible and achieve a better run.

## Aesthetics (what the player feels)

- **Humour:** The idea of a student endlessly trying to escape school creates a playful and humorous atmosphere.
- **Tension:** Increasing speed and approaching obstacles challenge the player's reaction time.
- **Satisfaction:** Successfully avoiding teachers, exams and homework while collecting game consoles provides a sense of achievement.
- **Progression:** Collecting consoles and purchasing speed upgrades gives players a feeling of becoming faster and more capable.
- **Irony:** Despite getting faster, the student can never actually escape school, reinforcing the endless nature of the game.

## Core mechanics (3 to 5 verbs or systems)

1. **Switch Lanes** — Swipe left or right to move between three lanes.
2. **Jump** — Swipe up to jump over obstacles.
3. **Roll** — Swipe down to pass underneath overhead obstacles.
4. **Collect Game Consoles** — Collect game consoles during runs and use them as currency to purchase speed upgrades.
5. **Speed Upgrades** — Spend collected game consoles to increase the player's movement speed.

**Design change:** The original turning mechanic was removed in favour of a straight three-lane track. The mission system remains planned and is not included in the current version.

## Dynamics (what emerges when the mechanics meet the player)

- Players must choose safe lanes and react to approaching obstacles.
- Different obstacle arrangements require lane switching, jumping or rolling.
- Increasing speed reduces reaction time and raises the difficulty.
- Collecting game consoles encourages players to balance survival with rewards.
- Speed upgrades create a progression loop but also make obstacles more challenging.
- Repeated obstacle patterns can occasionally occur because the spawning system still requires improvement.

## Progression & content

**Session length target:** 2–5 minutes per run.

**Implemented in CA2:**
- One greybox content environment using a straight three-lane track.
- Four obstacle arrangements: double obstacles, staggered obstacles, full-width overhead obstacles and single jump obstacles.
- Progressive running speed and difficulty.
- Game console collection and persistent currency balance.
- Speed upgrades purchased using collected game consoles.
- Simulated purchase of 500 units of in-game currency.
- Main menu, gameplay, pause and result screens.
- Local telemetry events and saved progression.

**Planned for CA3:**
- Improve obstacle spawning to reduce repetitive patterns.
- Polish the game's UI and visual presentation.
- Implement a mission system.
- Add a short tutorial explaining lane switching, jumping and rolling.
- Expand obstacle variety and refine difficulty progression.
- Explore additional school-themed environments if time permits.

## Platform features (Android)

- **Touch model:** Swipe left/right to switch lanes, up to jump and down to roll.
- **Orientation and safe areas:** Portrait orientation with UI adjustments for screen notches and safe areas.
- **Haptics:** Optional vibration feedback with a settings toggle.
- **Text size:** Small, Normal and Large options.
- **Motion:** Screen shake preference toggle exists, but the screen shake effect is not yet implemented.
- **Lifecycle:** Pause/resume and Android application focus handling.
- **Local persistence:** Game console currency balance and upgrade levels saved using PlayerPrefs.
- **Telemetry:** Local events written to the Unity console and `telemetry.log`.
- **Distribution:** Release-signed APK distributed for testing; no store publication.

## Performance budget (your device)

**Primary test device:** HUAWEI SEA-AL10  
**SoC:** HiSilicon Kirin 980  
**Android version:** Android 10  
**Graphics API:** Vulkan

| Metric | Target |
|---|---|
| Frame time | 16.7 ms at 60 FPS |
| 99th percentile frame time | Under 25 ms |
| Memory ceiling | Under 600 MB |
| Cold start | Under 4 seconds |
| APK size | Under 100 MB |

**Measured evidence:**

- Week 5 profiling recorded a mean frame time of 16.59 ms.
- A separate 600-frame sample recorded a p99 of 33.04 ms.
- Targeted profiling samples showed 0 B GC allocation in `TrackGenerator.GenerateNextTrack` and `RunnerMovement.Update`.
- A ten-minute Week 7 test showed mean frame time changing from 16.64 ms to 16.58 ms, with battery temperature remaining at 35.0°C.
- The Adaptive Performance provider was inactive, so thermal throttling could not be fully verified.

These results are from different test sessions and should not be treated as directly comparable measurements.

## Monetisation (if any) & ethics notes

Forward currently uses a soft-currency economy represented by collectible game consoles.

Players collect game consoles during gameplay and spend them on speed upgrades. Upgrade costs increase with each level.

A simulated purchase of 500 units of in-game currency is available through a fake purchase provider for development and testing. No real-money transactions are processed.

The current implementation does not include loot boxes or third-party advertising.

## Risks

- **Obstacle generation:** Repeated obstacle arrangements may reduce gameplay variety.
- **UI quality:** The current interface is functional but visually basic.
- **Mission system:** Adding missions may require additional gameplay and progression logic.
- **Performance:** Increasing obstacle variety and environmental detail may affect Android performance.
- **Difficulty:** Higher running speeds and purchased upgrades may reduce reaction time and require further balancing.
- **Time and scope:** Additional school-themed environments could delay improvements to the core gameplay.

## Cuts list (in the order they get cut)

1. Additional school-themed environments and transitions.
2. Extra visual effects and particle effects.
3. Additional audio effects.
4. Optional power-ups such as shields or temporary boosts.
5. Extra obstacle variations beyond the core set.
6. Additional mission types beyond a basic mission system.

Core gameplay stability, obstacle spawning improvements and essential UI usability take priority over optional content.

## Scope lock and design changes

**Original scope lock:** Wednesday, 16 September 2026.

**Major changes since the original design:**

- Replaced turning at corners with straight three-lane movement.
- Added rolling as an essential obstacle-avoidance action.
- Introduced a game console-based currency system, speed upgrades and simulated purchases.
- Added local telemetry and persistent progression.
- Deferred the mission system to later development.
- Prioritised Android stability, profiling and distribution over additional environments.
- Replaced the original anxiety dream theme with a humorous school escape concept featuring teachers, exam banners, homework and collectible game consoles.

**Current CA2 scope:** One playable three-lane endless-runner environment with school-themed obstacles, increasing difficulty, collectible game consoles, speed upgrades and Android testing evidence.

Further changes will be documented in the development journal.

## Reference

Hunicke, R., LeBlanc, M. and Zubek, R. (2004) *MDA: A Formal Approach to Game Design and Game Research*.