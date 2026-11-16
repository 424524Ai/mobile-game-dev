# Forward — CA2 Performance Baseline

**Device:** HUAWEI SEA-AL10 (Kirin 980 / Mali-G76 MP10, 8 GB RAM), Android 10, 2340 × 1080 display, 60 Hz. **Engine:** Unity 6000.6.0f1. **Graphics API:** Vulkan. **Target:** 60 FPS. **Battery health:** Not available (device does not report a percentage).

> **Evidence scope:** Measurements below come from different stages/builds. Historical frame-time results are not measurements of the final v0.8.0 APK. Profiler metrics are from Android Development Builds; Android launch timings and APK size refer to v0.8.0. Reported sample values are **single-frame observations**, not averages.

| Metric | Result | Test context / limitation |
|---|---:|---|
| Frame time, steady gameplay (mean / p99) | 16.57 / 16.53 ms | Week 3, Release; recorded values, p99 lower than mean requires verification |
| Frame time (mean / p99) | 16.59 / 33.04 ms | Week 5, Development Build |
| Frame time, before → after (mean / p99) | 16.64 / 16.92 → 16.58 / 16.53 ms | Week 7 thermal test, Balanced mode; not a controlled optimization comparison |
| CPU frame time | 17.74 ms | CA2, Development Build, one sampled frame |
| GPU frame time | Not available | Unity Profiler reports GPU profiling unsupported with Vulkan |
| GC Alloc, PlayerLoop | ~1.0 KB | CA2, Development Build, one sampled frame; ~0.9 KB in `FinishFrameRendering` |
| GC Alloc, script Update | 0 B | CA2 sampled `Update.ScriptRunBehaviourUpdate` |
| Peak reserved memory / TOTAL PSS | 361.7 MB / 213.6 MiB | Week 3, Development Build; historical |
| Reserved memory | ~0.63 GB | CA2, Development Build, sampled Profiler display |
| SetPass / triangles / vertices | 33 / ~69.5k / ~135.2k | CA2, Development Build, one sampled frame |
| Draw calls / batches | Unverified / not measured | Profiler displayed 0 draw calls; not a reliable actual draw-call count |
| Android cold / hot launch `TotalTime` | 279 / 67 ms | CA2 v0.8.0, ADB `am start -W`; second cold sample 233 ms |
| Startup screen → main menu | ~2.5 s | CA2 video estimate; PLAY response verified, precise command-to-interactive time not measured |
| Final Release APK size | 35.33 MiB (37,044,218 bytes) | v0.8.0, version code 7, ARM64 |

**Thermal / throttling (Week 7):** 10-minute Balanced-mode soak; 35.0°C start and end (**Δ 0.0°C**). Adaptive Performance remained inactive at minutes 0, 5 and 10; no throttling or `ThrottlingImminent` event observed.

**Optimization investigated (Week 5):** Track and corner object pools with prewarming; replaced `Physics.OverlapBox` with `Physics.OverlapBoxNonAlloc`; reduced score-text updates. After implementation, `TrackGenerator.GenerateNextTrack` and `RunnerMovement.Update` each showed **0 B GC Alloc** in sampled Development Build profiling. **Limitation:** no equivalent before-optimization allocation measurement was retained, so a quantified before/after improvement cannot be claimed.

**Outstanding measurement limitations:** Precise first-interactive startup time, reliable GPU frame time, batches/draw calls, battery health estimate, and controlled optimization before/after measurements were not available. Physical screen resolution is shown above; Unity internal render scale was not recorded.

*Source: `baseline-sheet.md` (Week 3, Week 5, Week 7 and CA2 entries).*
