## 14 September 2026

- Successfully installed Unity 6.6 with Android Build Support, Android SDK & NDK Tools, and OpenJDK.

- Successfully built and installed a Development Build on my HUAWEI SEA-AL10 using ADB. The game launched correctly and the `[Boot]` information was retrieved using `adb logcat`.

- Unity's Build And Run did not detect my phone under Run Device, although the device was successfully detected and used through ADB.

## Project shortlist
I chose Option 2 - Endless Runner for my project, called Forward. The main action is dodging obstacles while the player moves forward. If I do not have enough time, I will remove extra biomes and focus on the main gameplay and mission system.

## Profiler
- CPU Main Thread : 16.85ms
- SetPass calls: 20  
- GC allocated in frame: 51 B

## Week 2 - Touch Input
- Swipe Dp: 50
- Tap Max: 0.3 seconds

## Week 2 - Test Pause Menu
| Test                                      | Expected                                              |
| :---------------------------------------- | ----------------------------------------------------- |
| Press Home, wait 10 s, return             | Paused, panel visible, audio silent, settings saved ✓ |
| Pull the notification shade down and up   | Game paused ✓                                         |
| Neighbour calls you, you hang up          | Paused, game resumes only on Resume ✓                 |
| Screen off with the power button, back on | Paused ✓                                              |
| Force stop from Settings, relaunch        | Saved PlayerPrefs settings restored correctly ✓       |

Part B
Haptics - Vibration
Added a temporary Test Vibration button to the settings panel to test the haptic feedback on a real Android device. The button calls Haptics.Pulse(), which respects the saved Haptics toggle setting. The vibration test was successful.

Part C
1. Applied "TextScale.cs" script to all TMP Text
2. Created three text size options.
3. Tested and adjusted the text sizes.
4. Created a Settings panel with text size options and a Haptics toggle.
5. Added a Back button to return from the Settings panel to the Pause panel.
6. Added a Screen Shake toggle and saved its settings using PlayerPrefs.

## CA1 Release Build
Created a Release build for CA1 with the following settings:
- Scripting Backend: IL2CPP
- Target Achitecture: ARM64
- Development Build: Off
- Version: 0.1.0
- Package name: `com.ai.forward`
- Release signing configured using my Android keystore
	The Release APK was successfully installed and tested on my HUAWEI SEA-AL10 using `adb install -r`.

## 06 October 2026 - Lab 3 A
### Game environment setup
- Use Main camera as player for today
- Main camera's Y transform is set to 1.6
- Main camera is dragged to an empty Gameobject "Runner"
- Create "RunnerMovement.cs" to control player movement
- Set running speed to 6f per update
- tested script, in game view, the runner runs no problem
- Created a cube obstacle in scene view
- Make cube obstacle as prefab
- Create obstacle spawner script and empty gameObj
- Tested spawner in play mode
- Build dev apk to android device
- record profiler with game running on android device
	Selected frame: 43678
	CPU frame time: 25.08 ms
	PlayerLoop: 24.99 ms
- Tallest child marker
	PostLateUpdate.FinishFrameRendering: 22.82 ms


**Top three entries with their self time**

| Entry                                  | Time ms | Self ms |
| -------------------------------------- | ------: | ------: |
| PostLateUpdate.FinishFrameRendering    | 22.81ms |  0.26ms |
| PostLateUpdate.ProfilerEndFrame        |  0.64ms |  0.00ms |
| PostLateUpdate.PlayerSendFrameComplete |  0.23ms | 0.00 ms |

### GC
- GC.Collect: Not observed during the capture.
- Only very small incremental GC activity was observed.

### Rendering - Frame 43678
- SetPass Calls: 23
- Triangles: approximately 1.87k
- Vertices: approximately 5.44k
- Batches: not directly reported in the Unity 6.6 Rendering details

- Gfx.WaitForPresentOnGfxThread: 12.18 ms

### Memory - Frame 43678
- Total Reserved: 36.2MB
- GC Allocated in Frame: 3 count, 51B
- Textures: 104 count, 26.8MB
- Meshes: 2 count, 5.0KB
- Audio: 1.1MB

### Take Sample
**3 largest categories**
RenderTexture: 9.9MB
Shader: 4.1MB
CubeMap: 3.3MB

**single largest asset**
CameraDepthAttachment_576x1194_D24_UNorm_S8_UInt_Tex2D: 3.2 MB

### Probe
|Render Scale|Main-thread ms|`Gfx.WaitForPresentOnGfxThread`|
|---|--:|--:|
|**0.8**|**16.19 ms**|**8.79 ms**|
|**0.5**|**16.47 ms**|**0.00 ms**|
Verdict: CPU-Bound
**Candidate fix: Investigate `PostLateUpdate.FinishFrameRendering`, which was the tallest marker in the bad frame from Part B (~22.82 ms).**

### Frame Debugger
- Target: HUAWEI SEA-AL10 - Forward
- Total render events: 26
- Pass with the most draw events: BloomDownsample
- Draw events in pass: 10


## 06 October 2026 - Lab 3 B

### Test Setup

- Device: HUAWEI SEA-AL10
- Android version: 10
- Graphics API: Vulkan
- Refresh rate: 60 Hz
- Target frame rate: 60 FPS
- Optimized Frame Pacing: Enabled
- Unity version: 6000.6.0f1
- Release version: 0.1.1 (Version Code 3)
- Used Android Release for frame time measurements and Android Dev for profiling.
- The Pause Menu was used because the Main Menu has not been implemented yet.

Frame Time Sampler Test Expected: flat 16 to 17 ms trace in the menu. ✓

Steady Gameplay
Average: 16.57ms
p99: 16.53ms

Pause Menu
Average: 16.57 ms
p99: 16.53 ms
## 12 October 2026

Worst Case
Average: 16.55ms
p99: 16.53ms

### Performance Result
- Target frame rate: 60 FPS (16.67 ms per frame).
- Worst-case p99: 16.53 ms.
- The worst-case p99 is within the 60 FPS frame budget.
- Steady gameplay and worst-case gameplay were similar in the current prototype.

### Part C

| Metric                                | Result       |
| :------------------------------------ | :----------- |
| GC Allocated in Frame                 | 34B          |
| PostLateUpdate.PlayerSendFrameStarted | 17 B         |
| PreUpdate.NewInputUpdate              | 17 B         |
| GC.Collect                            | Not observed |

### Part D
Problem: Couldn't find Total Reserved Memory in Profiler Memory Module. Asked ChatGPT for help. Solution: Added a custom module with Total Reserved Memory attribute from profiler module menu.

| Metric              | Result                                    |
| :------------------ | ----------------------------------------- |
| Peak Total Reserved | 361.7 MB                                  |
| SetPass Calls       | 25                                        |
| Batches             | Not available in current profiler capture |
| Triangles           | 1.8K                                      |
| TOTAL PSS           | 213.6 MiB                                 |

### Part E

| Test   | Displayed Time (ms) |
| :----- | :------------------ |
| 1      | 208                 |
| 2      | 195                 |
| 3      | 174                 |
| Median | 195                 |
APK size: 30.66 MiB (32,152,329 bytes)

### Throttling

- Frame times were mostly around 16–17 ms during the short profiling session.
- Some frame time spikes were observed.
- No clear sustained performance drop was observed.
- Thermal throttling has not been formally tested yet.


## Scope Change - Dream Setting
After the initial scope lock, I developed a clearer theme for Forward.

Forward will take place inside a changing anxiety dream. The player is being chased by an unknown presence and must keep moving forward. The dream can change between environments such as city streets, corridors and rooftops.

This changes the theme and presentation of the game but does not change the main scope or core gameplay. The game is still an endless runner based on changing lanes, jumping and avoiding obstacles.



## 18 October 2026
Lab 5 A


1. changed obstacle place from spawning left/middle/right on the track to spawning in the middle only. 
2. Added `Floor2`to the game scene, for turning.
3. Added turning function to game(`RunnerMovement.cs`)
4. Set when player is able to turn, if turn too early or late, GAMEOVER.

5. Implement jump action into the game.