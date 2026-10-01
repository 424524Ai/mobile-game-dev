# MDA One-Pager - Forward

**Module:** Mobile Game Development (A12581) · **Student:** Ai Lin, 20099291> · **Project option:** 2 · **Due:** Wed 16 Sep 2026 (Week 2 Lab B)

Keep this to **one page**. It is your scope contract for the semester and is submitted again with CA1.

## One-line pitch
An endless runner where the player races through a changing dream, dodging obstacles while escaping an unknown presence.

## Theme / Setting

Forward takes place inside a recurring anxiety dream. The player is being chased by an unknown presence and must keep moving forward without knowing what is behind them.

The dream constantly changes around the player, moving through places such as city streets, corridors and rooftops. Because it is a dream, the environment does not need to follow normal rules. The player may run across rooftops, move onto power lines or fall from a building and continue into another part of the dream.

## Aesthetics (what the player feels)
- Tension from being chased by an unknown presence and not knowing what is behind them.
- Uncertainty as the dream changes between different environments.
- Satisfaction from successfully dodging obstacles and completing missions.

## Core mechanics (3 to 5 verbs or systems)
1. Change Lanes
2. Jump
3. Dodge obstacles
4. Complete missions

## Dynamics (what emerges when the mechanics meet the player)
- Players must quickly decide whether to jump or change lanes as obstacles approach
- As the speed increases, players need faster reactions and better timing to survive
- Changes in the dream environment can introduce different obstacle layouts while keeping the same core controls.

## Progression & content
- **Session length:** 2 to 5 minutes per run
- **Content in the vertical slice (by Week 6):** 1 biome, 10+ reusable track chunks, basic obstacles, and a mission system.
- **Content by CA3:** Additional biomes, more obstacle variations, difficulty progression, and polished UI.

## Platform features (Android)
- **Touch model:** swipe left or right to change lanes and swipe up to jump
- **Safe areas and orientation:** Portrait orientation with UI adjusted for notches and safe areas.
- **Haptics:** optional vibration feedback, with a toggle in settings panel, tested on a real Android device.
- **Text size:** Small, normal, large settings.
- **Motion:** A screen shake toggle that saves the player's preference. Screen shake is not yet implemented.
- **Lifecycle:** pause/resume and focus loss handled from Week 2
- **Store / testing tracks:** awareness only, no uploads

## Performance budget (your device)
- **Device:** Model: HUAWEI SEA-AL10 
- SoC: HiSilicon Kirin 980 
- Android version: Android OS 10
- **Target frame time:** 16.7 ms at 60 fps; 
- **99th percentile frame time:** Under 25 ms.
- **Memory ceiling:** under 600 MB
- **Cold start:** under 4 s to interactive
- **APK size:** under 100 MB
- High FPS toggle: Not planned.

These are performance targets. Actual performance will be measured using Unity Profiler and Android testing.

## Monetisation (if any) & ethics notes
- No monetisation planned. If published, I would avoid loot boxes or pay-to-win mechanics that pressure players to spend money.

## Risks 
- Performance: Reusable track chunks and obstacles may affect performance on Android. I will profile the game and optimise where needed.
- Time: Creating too many biomes and obstacle types could delay the core gameplay.
- Touch controls: Swipe detection must be reliable on a real phone.
- Difficulty: The game needs to become harder without feeling unfair.

## Cuts list (in the order they get cut)
1. Additional biome variety
2. Extra visual and particle effects
3. Additional Audio Effects
4. power-up system (Shield or temporary boosts)
5. Extra obstacle types
6. Additional mission types

## Scope lock
- **Locked on:** Wed 16 Sep 2026
- **Changes after lock** require a note in the development journal explaining what changed and why.

Week 6 slice: 1 biome, 10+ chunks, a mission system

Reference: Hunicke, LeBlanc and Zubek (2004), *MDA: A Formal Approach to Game Design and Game Research*.




