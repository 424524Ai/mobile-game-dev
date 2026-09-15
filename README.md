# mobile-game-dev

## Project information
- game title: Forward
- option number: 2

## Phone device information
- Model: HUAWEI SEA-AL10
- Android Version: Android OS 10 / API-29
- Graphics API: Vulkan

## Boot log
[Boot] HUAWEI SEA-AL10 | Android OS 10 / API-29 (HUAWEISEA-AL10/104.0.0.121C00) | Vulkan | 720x1493 @ 320 dpi

## Build Steps

1. Build the Android Development Build in Unity using IL2CPP and ARM64.
2. Install the APK using `adb install -r Builds/MyGame-dev.apk`.
3. Launch the app and check the Unity log using `adb logcat -s Unity`.

## Keystore
- File name: forward-release.keystore
- Location: G:\Keystores\forward-release.keystore
- Alias: forward
- Validity: 50 years

## Android Build Profiles
- Android Dev: Used for development and testing. Development Build and Autoconnect Profiler are enabled.
- Android Release: Used for release builds. Development Build is disabled and the APK is signed with the release keystore.