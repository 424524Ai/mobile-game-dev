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
- Tap Max: 0.3

## Week 2 - Test Pause Menu
- Press Home, wait 10 s, return | Paused, panel visible, audio silent, progress saved ✓
- Pull the notification shade down and up | Paused ✓
- Neighbour calls you, you hang up | Paused, game resumes only on Resume ✓
-  Screen off with the power button, back on | Paused ✓
-  Force stop from Settings, relaunch | Progress restored from the save ✓

Part B
Haptics - Vibration
Added a temporary Test Vibration button to the settings panel to test the haptic feedback on a real Android device. The button calls Haptics.Pulse(), which respects the saved Haptics toggle setting. The vibration test was successful.

Part C
1. Applied "TextScale.cs" script to all TMP Text
2. 3 buttons created
3. Text size fixed
Extra done: Created Settings panel that shows the text size options and haptics toggle, a back button created to go back to pause panel. PausePanel is disable on default, only when settings button is pressed will enable pausepanel (Modified PauseMenu.cs ln:6, 27 - 36)

