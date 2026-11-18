# Forward — CA2 Known Issues and Limitations

**Build:** 0.8.0 (Version Code 7)  
**Platform:** Android (IL2CPP, ARM64)  

## Known Gameplay and Feature Limitations

### Known Gameplay Issue

| Item | Current status | Planned follow-up |
|---|---|---|
| Obstacle generation variety | The obstacle generator sometimes spawns the same obstacle type repeatedly in consecutive sections, reducing variety during a run. | Review the random selection logic and test ways to avoid excessive repetition while maintaining fair obstacle patterns. |

### Incomplete Features and Polish
| Item | Current status | Planned follow-up |
|---|---|---|
| Game UI | The current interface is functional but visually basic and needs further polish. | Improve the appearance and layout of the main menu, gameplay HUD, pause screen and other UI elements. |
| Mission system | The planned mission system has not yet been added to the game. | Design and implement missions, then test their integration with the core endless-runner loop. |
| Gesture onboarding | A playtester had difficulty identifying jump and roll gestures and tried to jump over an obstacle that required rolling. | Add a short tutorial explaining swipe controls and obstacle responses. |

## Testing and Evidence Limitations

| Item | Current status | Planned follow-up |
|---|---|---|
| Peer feedback | The Week 8 Lab A APK distribution link, QR code and feedback form were prepared, but peer feedback was not yet collected. | Gather feedback and prioritise changes based on observations. |
| Device coverage | CA2 performance measurements were taken primarily on the HUAWEI SEA-AL10 (Android 10). A wider device compatibility test was not completed for Week 8 Lab A. | Test on additional Android devices where available. |
| Thermal instrumentation | During the ten-minute test, the Adaptive Performance Provider remained inactive. No sustained slowdown was observed, but thermal throttling could not be directly verified through the provider. | Repeat or extend testing with supported instrumentation if available. |

## Additional Notes

- The 500-coin pack uses a **simulated purchase provider**; it does not process real payments.
- Telemetry is written locally to `telemetry.log`; no remote analytics service is integrated.
- The items above distinguish observed gameplay limitations from incomplete test coverage. They should not all be interpreted as confirmed software defects.

## Week 8 Lab B – QA Issues

**Test Date:** 17 November 2026  
**Device:** HUAWEI nova 5 Pro (Android 10)  
**Application:** Forward (`com.ai.forward`)  
**Build:** 0.8.0 (Version Code 7)

### Issue 1 – Screen Orientation Rendering Problem

**Status:** Fixed and Verified

**Scenario:** Rotation Test

**Steps to Reproduce:**

1. Launch Forward with Default Orientation set to Auto Rotation.
    
2. Enable Auto-rotate on the device.
    
3. Rotate the device from portrait to landscape.
    

**Expected Result:**  
The game should remain in portrait orientation without rendering issues.

**Actual Result:**  
A large portion of the screen turned black in landscape orientation, leaving only a small section of the game visible.

**Fix Applied:**  
Changed Unity Player Settings → Resolution and Presentation → Default Orientation from Auto Rotation to Portrait.

**Retest Result:**  
Rebuilt the Android APK and repeated the rotation test. The game remained in portrait orientation with no black screens, freezing or new Logcat errors.

**Evidence:**  
Manual reproduction and successful retest. See `docs/CA3/qa/test-plan.md` and `logcat-w08-testplan.txt`. No specific error line was observed for this rendering issue.

### Issue 2 – Background Resume Delay

**Status:** Needs Further Verification

**Scenario:** Resume / Background Test

**Steps to Reproduce:**

1. Launch Forward and start playing.
    
2. Press Home and wait 30 seconds.
    
3. Return to the game.
    

**Expected Result:**  
The Pause Menu should appear promptly, and the game should remain responsive.

**Actual Result:**  
On the initial build, the Pause Menu took approximately 5–7 seconds to appear. Touch input was unresponsive during this period.

**Retest Result:**  
After rebuilding the application, the Pause Menu appeared immediately and the Resume button worked correctly in one retest.

**Follow-up:**  
Repeat the background resume test to verify whether the delay has been consistently resolved.

**Evidence:**  
Manual timing and observation. No ANR was observed in the monitored Logcat output.

### Issue 3 – Adaptive Performance Initialisation Errors

**Status:** Open – Investigation Required

**Scenario:** Application Startup / QA Testing

**Steps to Reproduce:**

1. Launch Forward on the HUAWEI nova 5 Pro.
    
2. Monitor Android Logcat during startup.
    

**Expected Result:**  
Unity Adaptive Performance should initialise without reporting errors.

**Actual Result:**  
Logcat repeatedly reported Android subsystem initialisation failures.

**Logcat Evidence:**

`11-17 17:22:54.774 E Unity: Unable to start the Android subsystem.`

`UnityEngine.AdaptivePerformance.Google.Android.GoogleAndroidProviderLoader:Initialize()`

A separate `ClassNotFoundException` for `com.google.android.play.core.assetpacks.AssetPackManager` was also observed during startup.

**Impact:**  
The game remained playable despite the errors. Their effect on performance and background resume behaviour has not been established.

**Follow-up:**  
Review the installed Adaptive Performance provider and Android dependencies. Investigate whether the provider is supported on the test device.

**Evidence:**  
`docs/CA3/qa/logcat-w08-testplan.txt`