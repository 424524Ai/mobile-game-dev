
# Week 3 Performance Baseline

| Field | Row 1 |
|---|---|
| Date | 12 October 2026 |
| Commit | TBD |
| Version Name / Code | 0.1.1 / 3 |
| Build | Android Release (frame times), Android Dev (Profiler) |
| Unity Version | 6000.6.0f1 |
| Device | HUAWEI SEA-AL10 |
| Android Version | 10 |
| SoC / GPU | TBD |
| Graphics API | Vulkan |
| Refresh Rate | 60 Hz |
| Target FPS | 60 |
| Menu (Pause Menu): avg / p99 | 16.57 ms / 16.53 ms |
| Steady Gameplay: avg / p99 | 16.57 ms / 16.53 ms |
| Worst Case: avg / p99 | 16.55 ms / 16.53 ms |
| GC Allocated in Frame | 34 B |
| Allocating Markers | PlayerSendFrameStarted (17 B), NewInputUpdate (17 B) |
| GC.Collect | Not observed during the capture |
| Peak Total Reserved | 361.7 MB |
| TOTAL PSS | 213.6 MiB |
| Worst Case: SetPass / Batches / Triangles | 25 / N/A / ~1.8k |
| Cold Start Median | 195 ms (Displayed) |
| Fully Drawn Median | 729 ms |
| First Interactive Time | Not measured |
| APK Size | 30.66 MiB (32,152,329 bytes) |
| Thermal Delta / Throttling (Week 7) | |
| Load Time (Week 5) | |

## Notes

- The Pause Menu was used because the main menu has not been implemented yet.
- Steady gameplay and worst-case gameplay are similar in the current prototype.
- Batches Count was not available in the current Profiler capture.
- Frame times were measured using the Android Release build.
- GC, memory and rendering measurements were taken using the Android Dev build.


