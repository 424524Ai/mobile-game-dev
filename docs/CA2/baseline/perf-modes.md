
# Week 7 Lab A – Performance Modes

## Device Information

- Device: HUAWEI SEA-AL10 (nova 5 Pro)
- Android Version: 10 (API 29)
- Display Resolution: 1080 × 2340
- Supported Refresh Rate: 60 Hz
- Test Build: Android Development Build with Unity Profiler

## Performance Mode Comparison

| Mode     | Target Frame Rate | Render Scale | Frame Time Avg (ms) | 99th (ms) | Bottleneck   | Panel Hz |
| -------- | ----------------- | ------------ | ------------------- | --------- | ------------ | -------- |
| Balanced | 60 FPS            | 1.0          | 16.58               | 16.52     | Not reported | 60       |
| High FPS | 120 FPS           | 0.8          | 16.52               | 16.53     | Not reported | 60       |

## Test Observations

### Balanced Mode

- Tested on the Android device for approximately 30 seconds.
- Gameplay was smooth with no noticeable stuttering or heating.
- Unity Profiler showed frame times generally near 16.67 ms.

### High FPS Mode

- Tested using Unity Profiler.
- Frame times remained generally near 16.67 ms.
- Occasional frame-time spikes were observed.
- No clear frame-rate improvement was observed compared with Balanced mode.

## Device Limitations

ADB confirmed that the device supports only a 60 Hz display mode.
Therefore, the 120 FPS target cannot produce 120 distinct displayed frames per second on this device.

The Adaptive Performance provider was inactive on this Android 10 device.

Logcat output:

`[AP] provider inactive on this device`

No thermal or performance bottleneck events were available from the provider.

## Measurement Notes

- Both modes were profiled using Android Development Builds.
- Development Build profiling introduces some measurement overhead.
- Average and 99th-percentile frame times have not yet been calculated from the saved Profiler data.
- The selected frame times in Profiler screenshots must not be treated as session averages.

## Evidence

- `perf-modes/balanced.data`
- `perf-modes/high-fps.data`
- `perf-modes/display-modes.txt`
