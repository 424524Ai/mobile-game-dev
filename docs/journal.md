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

## 06 October 2026 - Lab 3
### Game environment setup
- Use Main camera as player for today
- Main camera's Y transform is set to 1.6
- Main camera is draged to an empty Gameobject "Runner"
- Create "RunnerMovement.cs" to control player movement
- Set running speed to 6f per update
- tested script, in game view, the runner runs no problem
- Created a cube obstacle in scene view
- Make cube obstacle as prefab
- Create obstacle spawner script and empty gameObj
- Tested spwaner in play mode
- Build dev apk to android device
- record profiler with game running on android device
	Selected frame: 43678
	CPU frame time: 25.08 ms
	PlayerLoop: 24.99 ms
	
- Tallest child marker
    Gfx.WaitForPresentOnGfxThread: 22.82 ms
	

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
- Longest pass: BloomDownsample
- Draw events in pass: 10



## Scope Change - Dream Setting
After the initial scope lock, I developed a clearer theme for Forward.

Forward will take place inside a changing anxiety dream. The player is being chased by an unknown presence and must keep moving forward. The dream can change between environments such as city streets, corridors and rooftops.

This changes the theme and presentation of the game but does not change the main scope or core gameplay. The game is still an endless runner based on changing lanes, jumping and avoiding obstacles.
