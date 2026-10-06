
# What
A slow frame was observed during normal gameplay on the Android device.

# Where
`PostLateUpdate.FinishFrameRendering` was the tallest marker in the bad frame, and `BloomDownsample` had the longest list of draws in the Frame Debugger with 10 draw events.

# Numbers
- Main-thread time: 24.99 ms
- SetPass calls: 23
- GC allocated in frame: 51 B
- Full render scale (0.8): 16.19 ms
- Half render scale (0.5): 16.47 ms

# Verdict
CPU-bound because lowering the render scale from 0.8 to 0.5 did not reduce the frame time.

# Fix to try
Investigate `PostLateUpdate.FinishFrameRendering`, the tallest marker found in the bad frame.

Evidence:

- [Bad frame screenshot](w03-bad-frame.png)
- [GPU pass screenshot](w03-gpu-pass.png)
- [Profiler capture](w03-profile.data)
