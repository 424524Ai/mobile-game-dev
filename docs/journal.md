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

## Scope Change - Dream Setting
After the initial scope lock, I developed a clearer theme for Forward.

Forward will take place inside a changing anxiety dream. The player is being chased by an unknown presence and must keep moving forward. The dream can change between environments such as city streets, corridors and rooftops.

This changes the theme and presentation of the game but does not change the main scope or core gameplay. The game is still an endless runner based on changing lanes, jumping and avoiding obstacles.
