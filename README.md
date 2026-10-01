# mobile-game-dev

## Project information
- Game title: Forward
- Option number: 2
- Unity version: 6000.6.0f.1
- Package name: `com.ai.forward`
- Version: 0.1.0

## Phone device information
- Model: HUAWEI SEA-AL10
- Android Version: Android OS 10 / API-29
- Graphics API: Vulkan

## Boot log
[Boot] HUAWEI SEA-AL10 | Android OS 10 / API-29 (HUAWEISEA-AL10/104.0.0.121C00) | Vulkan | 720x1493 @ 320 dpi

## Build Steps

### Development Build
1. Open the project in Unity 6.6.
2. Open Build Profiles and select `Android Dev`.
3. Make sure the Android Dev profile is active.
4. Build the APK.
5. Install the APK on the Android device using ADB.
6. Check the Unity log using `adb logcat -s Unity`.
1. Build the Android Development Build in Unity using IL2CPP and ARM64.
2. Install the APK using `adb install -r Builds/MyGame-dev.apk`.
3. Launch the app and check the Unity log using `adb logcat -s Unity`.

### Release Build
1. Open Build Profiles and select `Android Release`.
2. Make sure `Android Release` is the active profile.
3. Make sure Development Build is disabled.
4. In Android Player Settings, set:
   - Scripting Backend: IL2CPP
   - Target Architecture: ARM64 only
5. Make sure the release keystore and alias are selected.
6. Build the APK as:
   `releases/Forward-0.1.0-release-arm64.apk`
7. Install the APK on the device using:
   `adb install -r releases/Forward-0.1.0-release-arm64.apk`
8. Launch and test the Release build on the Android device.

## Signing Notes
The Android Release build is signed using a release keystore.

- Keystore file: `forward-release.keystore`
- Keystore location: `G:\Keystores\forward-release.keystore`
- Alias: `forward`
- Validity: 50 years
- The keystore is stored outside the Git repository.
- Keystore and key passwords are not stored in this repository.
- `*.keystore` and `*.jks` are excluded from Git.

## Android Build Profiles

### Android Dev
Used for development and testing.

- Development Build: Enabled
- Autoconnect Profiler: Enabled
- Scripting Backend: IL2CPP
- Target Architecture: ARM64

### Android Release
Used for the final CA1 Release APK.

- Development Build: Disabled
- Scripting Backend: IL2CPP
- Target Architecture: ARM64 only
- Signed using the release keystore