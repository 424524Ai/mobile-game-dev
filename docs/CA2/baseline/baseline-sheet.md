
# Week 3 Performance Baseline

| Field                                     | Row 1                                                 |
| ----------------------------------------- | ----------------------------------------------------- |
| Date                                      | 12 October 2026                                       |
| Commit                                    | TBD                                                   |
| Version Name / Code                       | 0.8.0 / 7                                             |
| Build                                     | Android Release (frame times), Android Dev (Profiler) |
| Unity Version                             | 6000.6.0f1                                            |
| Device                                    | HUAWEI SEA-AL10                                       |
| Android Version                           | 10                                                    |
| SoC / GPU                                 | HiSilicon Kirin 980 / Mali-G76 MP10                   |
| RAM                                       | 8GB                                                   |
| Screen Resolution                         | 2340 × 1080                                           |
| Graphics API                              | Vulkan                                                |
| Refresh Rate                              | 60 Hz                                                 |
| Target FPS                                | 60                                                    |
| Menu (Pause Menu): avg / p99              | 16.57 ms / 16.53 ms                                   |
| Steady Gameplay: avg / p99                | 16.57 ms / 16.53 ms                                   |
| Worst Case: avg / p99                     | 16.55 ms / 16.53 ms                                   |
| GC Allocated in Frame                     | 34 B                                                  |
| Allocating Markers                        | PlayerSendFrameStarted (17 B), NewInputUpdate (17 B)  |
| GC.Collect                                | Not observed during the capture                       |
| Peak Total Reserved                       | 361.7 MB                                              |
| TOTAL PSS                                 | 213.6 MiB                                             |
| Worst Case: SetPass / Batches / Triangles | 25 / N/A / ~1.8k                                      |
| Cold Start Median                         | 195 ms (Displayed)                                    |
| Fully Drawn Median                        | 729 ms                                                |
| First Interactive Time                    | Not measured                                          |
| Thermal Delta / Throttling (Week 7)       |                                                       |
| Load Time (Week 5)                        |                                                       |
## Notes

- The Pause Menu was used because the main menu has not been implemented yet.
- Steady gameplay and worst-case gameplay are similar in the current prototype.
- Batches Count was not available in the current Profiler capture.
- Frame times were measured using the Android Release build.
- GC, memory and rendering measurements were taken using the Android Dev build.



## Week 5 Lab A — Object Pooling Performance

### Test Environment
- Platform: Android
- Device: HUAWEI SEA-AL10
- Build: Development Build
- Profiler: Unity CPU Usage

### GC Allocation Results

| Method | GC Alloc |
|---|---|
| TrackGenerator.GenerateNextTrack | 0 B |
| RunnerMovement.Update | 0 B |

### Optimizations
- Implemented TrackPool and CornerPool.
- Added object pool prewarming.
- Replaced Physics.OverlapBox with Physics.OverlapBoxNonAlloc.
- Optimized score text updates to avoid unnecessary allocations.

### Evidence
- Profiler capture: w05-spawner.data

### Frame Time Results

| Metric | Result |
|---|---|
| Mean Frame Time | 16.59 ms |
| P99 Frame Time  | 33.04 ms |

### Thermals
| Field                                    | Value                                                 |
| ---------------------------------------- | ----------------------------------------------------- |
| Build ID and mode                        | Week 7 Lab B Development Build, Balanced              |
| Start temperature (°C)                   | 35.0                                                  |
| End temperature (°C) and 10-minute delta | 35.0, Δ 0.0°C                                         |
| Overlay level at minute 0 / 5 / 10       | AP inactive / AP inactive / AP inactive               |
| First `ThrottlingImminent`               | Not observed in 10 minutes                            |
| `Throttling` observed                    | No event observed; AP inactive                        |
| Frame time before: mean / 99th (ms)      | 16.64 / 16.92                                         |
| Frame time after: mean / 99th (ms)       | 16.58 / 16.53                                         |
| Conditions                               | Balanced mode, 10-minute soak, Huawei SEA-AL10, 60 Hz |

| Startup Screen to Main Menu (Video Estimate)        | ~2.5 s                                                             |
| :-------------------------------------------------- | ------------------------------------------------------------------ |
| PLAY Button Interaction                             | Verified on device                                                 |
| First Interactive Time (ADB Command to Interactive) | Not measured                                                       |
| CPU Frame Time (Sample)                             | 17.74 ms                                                           |
| GPU Frame Time                                      | Not available (Unity Profiler / Vulkan)                            |
| Graphics API                                        | Vulkan                                                             |
| Profiling Build                                     | Android Development Build                                          |
| SetPass Calls (Sample)                              | 33                                                                 |
| Triangles (Sample)                                  | ~69.5k                                                             |
| Vertices (Sample)                                   | ~135.2k                                                            |
| Draw Calls                                          | 0 (Profiler reported; requires verification)                       |
| Batches                                             | Not measured                                                       |
| Profiling Build                                     | Android Development Build                                          |
| GC Alloc (PlayerLoop, Sample)                       | ~1.0 KB                                                            |
| GC Alloc (ScriptRunBehaviourUpdate, Sample)         | 0 B                                                                |
| Main Allocation Source                              | PostLateUpdate.FinishFrameRendering (~0.9 KB)                      |
| Profiling Build                                     | Android Development Build                                          |
| APK Size                                            | 35.33 MiB (37044218 bytes)                                         |
| Cold Start (Android TotalTime)                      | 279 ms                                                             |
| Hot Start (Android TotalTime)                       | 67 ms                                                              |
| SoC / GPU                                           | HiSilicon Kirin 980 / Mali-G76 MP10                                |
| RAM                                                 | 8GB                                                                |
| Version Name / Code                                 | 0.8.0 / 7                                                          |
| Battery Health Estimate                             | Not available (device does not report a battery health percentage) |


## Week 8 Lab B — QA Performance Measurements

| Field                         | Value                            |
| ----------------------------- | -------------------------------- |
| Date                          | 17 November 2026                 |
| Device                        | HUAWEI SEA-AL10                  |
| Android Version               | 10                               |
| Version Name / Code           | 0.8.0 / 7                        |
| Build ID                      | 69678012                         |
| Build                         | Android Release (Signed)         |
| Cold Start (TotalTime)        | 400 ms                           |
| Warm / Hot Start (TotalTime)  | 73 ms                            |
| TOTAL PSS (5-minute gameplay) | 280.35 MiB (287,082 KiB)         |
| Measurement Tools             | ADB am start -W, dumpsys meminfo |

### Notes

- Cold start was measured after force-stopping the application.
- Hot start was measured after returning from the background.
- PSS was captured after approximately five minutes of gameplay.
- Results are single measurements, not medians or peak values.
- First interactive time was not measured.

### Week 8 Lab B – Additional QA Measurements

|Field|Value|
|---|---|
|Date|17 November 2026|
|Device|HUAWEI SEA-AL10|
|Android Version|10|
|Build|Forward-v0.8.0-qa-orientation-fix.apk|
|Cold Start (TotalTime)|227 ms|
|Background Relaunch (TotalTime)|64 ms|
|Background Relaunch State|HOT|
|TOTAL PSS|198,215 KiB (193.57 MiB)|
|TOTAL Swap PSS|187 KiB|
|Measurement Tools|ADB `am start -W`, `dumpsys meminfo`|

**Notes:**

- The cold start was measured after force-stopping the application.
    
- The HOT launch was measured after returning from the background.
    
- PSS was captured using Android Debug Bridge.
    
- These are individual measurements, not medians.
    
- The measurements were taken after rebuilding the application with portrait orientation locked.
    
- The exact time until gameplay became interactive was not measured.