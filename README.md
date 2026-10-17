# mobile-game-dev

## Project information
- Game title: Forward
- Option number: 2
- Unity version: 6000.6.0f.1
- Package name: `com.ai.forward`
- Version: 0.2.0
- Bundle Version Code: 2

## Phone device information
- Model: HUAWEI SEA-AL10
- Android Version: Android OS 10 / API-29
- Graphics API: Vulkan

## Boot log
[Boot] HUAWEI SEA-AL10 | Android OS 10 / API-29 (HUAWEISEA-AL10/104.0.0.121C00) | Vulkan | 720x1493 @ 320 dpi

## Device Targets
Minimum API Level: Android 8.0 (API level 26)
Target API Level: Automatic (highest installed)
Google Play target API requirements: https://support.google.com/googleplay/android-developer/answer/11926878
Tested devices:
HUAWEI SEA-AL10 — Android 10
PHB110 — Android 16

The Release APK was successfully installed on both devices. The game ran normally on the PHB110, with no display issues or crashes observed.

See docs/CA1/device-matrix.md for the test results.

## Requirements
- Unity 6000.6.0f1
- Android Build Support (SDK, NDK and OpenJDK)
- Android device with USB debugging enabled
- ADB
- Release keystore for signed builds

## Build Steps

### Development Build
1. Clone the repository and open the project in Unity 6000.6.0f1.
2. Open File > Build Profiles.
3. Select and activate `Android Dev`.
4. Make sure Development Build is enabled.
5. Check that Scripting Backend is IL2CPP and ARM64 is selected.
6. Build the Development APK.
7. Install the APK using `adb install -r` followed by the APK path.
8. Launch the game and check the Unity log using `adb logcat -s Unity`.

### Release Build
1. Clone the repository and open the project in Unity 6000.6.0f1.
2. Open File > Build Profiles and activate Android Release.
3. Open Edit > Project Settings > Player > Android.
4. Check the following settings:
   - Package Name: com.ai.forward
   - Version: 0.2.0
   - Bundle Version Code: 2
   - Minimum API Level: Android 8.0 (API 26)
   - Target API Level: Automatic (highest installed)
   - Scripting Backend: IL2CPP
   - Target Architecture: ARM64
5. Make sure Development Build and Build App Bundle are disabled.
6. In Publishing Settings, select the release keystore and alias forward.
7. Build the APK to `releases/Forward-0.2.0-arm64.apk`.
8. Connect an Android device with USB debugging enabled.
9. Check the connection using `adb devices`.
10. Install the APK:
   `adb install -r releases/Forward-0.2.0-arm64.apk`
11. Verify the installed version:
   `adb shell dumpsys package com.ai.forward`
12. Confirm that the output contains versionCode=2 and versionName=0.2.0.
13. Launch the game and test it on the device.
If an incompatible signature or version downgrade prevents installation, uninstall the existing app before installing the Release APK. Uninstalling may remove locally stored app data.

## Signing Notes
The Android Release build is signed using a release keystore.

- Keystore file: `forward-release.keystore`
- Keystore location: `G:\Keystores\forward-release.keystore`
- Backup: OneDrive Personal Vault.
- Alias: `forward`
- The keystore is stored outside the Git repository.
- Keystore and key passwords are not stored in this repository.
- `*.keystore` and `*.jks` are excluded from Git.
- Certificate validity: 15 September 2026 – 2 September 2076 (approximately 50 years).
- SHA-256 fingerprint: 8B:1A:8A:2E:86:BC:7D:47:7E:B2:2F:FB:27:45:9A:FF:42:C5:C7:6A:4C:72:D1:1D:BF:C4:D5:1E:6E:16:35:56

## Android Build Profiles

### Android Dev
Used for development and testing.

- Development Build: Enabled
- Autoconnect Profiler: Enabled
- Scripting Backend: IL2CPP
- Target Architecture: ARM64

### Android Release
Used for signed Android Release builds.

- Development Build: Disabled
- Scripting Backend: IL2CPP
- Target Architecture: ARM64 only
- Signed using the release keystore

### AI Assistance
ChatGPT was used as a learning and support tool during development, mainly to help understand technical concepts, troubleshoot issues, and review documentation.

The game was developed, configured, and tested by me. I reviewed and verified the AI suggestions before applying them to the project.