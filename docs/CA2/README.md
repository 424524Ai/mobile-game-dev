# Forward — CA2 Submission

**Game:** Forward (Option 2 — Endless Runner)  
**Platform:** Android  
**Unity:** 6000.6.0f1  
**Package:** `com.ai.forward`  
**Release:** 0.8.0 (Version Code 7)  
**Build:** Release-signed APK, IL2CPP, ARM64

## Contents

- `APK/Forward-0.8.0-arm64.apk` — Android installation package
- `Video/Gameplay_Demo.mp4` — gameplay demonstration
- `Docs/Performance_Baseline.md` — device and performance measurements
- `Docs/Economy_Telemetry_Map.md` — economy loop and logged events
- `Docs/Reflective_Journal.md` — development reflection (through Week 8 Lab A)
- `KnownIssues.md` — known limitations and outstanding testing
- `LINKS.txt` — repository and tagged-release links

## Install and run

1. Transfer the APK in `APK/` to an Android device and install it, allowing installation from the chosen source if prompted.
2. Alternatively, enable USB debugging, connect the device and run:

   ```bash
   adb devices
   adb install -r APK/Forward-0.8.0-arm64.apk
   ```

3. Launch **Forward** from the device's app launcher.
4. Start a run from the menu. Use swipe gestures to change lanes, jump and roll; avoid obstacles, collect coins and use available upgrades.
5. Use the in-game pause menu to pause or resume play.

**Installation note:** An existing build signed with a different key, or a newer installed version, may prevent installation. Uninstalling the existing app can delete locally saved data.

## Testing and scope

Primary development and performance testing used **HUAWEI SEA-AL10**, Android 10, Vulkan. The project targets Android 8.0 (API 26) or later; this minimum is a build setting, not a guarantee of compatibility on every device. 
## Notes

- The game records local telemetry in `Application.persistentDataPath/telemetry.log`; no remote analytics service is integrated.
- The optional coin-pack purchase is simulated and does not process real payments.
- See `KnownIssues.md` for limitations, including the currently untriggered `level_complete` event.
- Development used ChatGPT for technical explanations, troubleshooting and documentation support; implementation and test results are described in the accompanying journal.

### AI Assistance & Asset Credits

Claude Code were used to create the following game assets:

- Player character
- Track models
- Obstacle models
- Collectible game console model

These AI-generated assets were integrated into the Unity project.

### Third-Party Assets

- **Skybox:** [Unity Asset Store](https://assetstore.unity.com/packages/2d/textures-materials/sky/farland-skies-cloudy-crown-pro-71105)
