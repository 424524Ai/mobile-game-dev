# Forward — CA2 Reflective Journal

**Coverage:** Week 5 to Week 8 Lab A (Week 9 not yet started)  
**Project:** Forward — Android Endless Runner  
**Platform:** Unity 6000.6.0f1, Android (HUAWEI SEA-AL10)

## Introduction and reflection approach

This journal reflects on my progress developing *Forward*, a mobile endless runner in which the player moves along a track, changes lanes, jumps, and rolls to avoid obstacles. I have organised the reflection around what I intended to achieve, what I implemented and tested, what went wrong, and what I would change next. The period covered is Week 5 through Week 8 Lab A. Week 9 has not started, so I have not presented it as completed work. My main learning during this period was that a playable feature is only one part of a vertical slice: reliable scene transitions, performance evidence, clear controls, persistence, and a testable distribution build also matter.

## Week 5 — Building and testing the playable loop

During Week 5, I initially built a track that allowed the runner to turn at corners, using a track generator and corner triggers. After trying this approach, I decided that a straight track with three lanes offered more interesting moment-to-moment choices and greater obstacle variety. I therefore removed turning from the intended gameplay and focused on lane switching instead. This was an important scope decision: keeping an early mechanic simply because I had implemented it would not necessarily make the game better.

After switching to the three-lane design, I focused on building a repeatable gameplay loop. I established Boot, Menu, Game and Result scenes, and connected gameplay to the result screen. I also developed a greybox content lane with four obstacle arrangements: double obstacles, staggered obstacles, a full-width obstacle requiring a roll, and a single jump obstacle. This gave me a more meaningful basis for playtesting than a track containing only one obstacle type.

I adjusted the difficulty ramp after testing on my Huawei Android device. The starting speed increased from 6 to 8 units per second, the maximum base speed from 12 to 16, and the acceleration interval decreased from 15 to 10 seconds. These changes made the game feel more active, but they also showed me why difficulty should be evaluated on the actual phone rather than only in the Unity Editor. Touch controls, screen size and reaction time affect the experience.

Performance became a second priority. Repeatedly creating and destroying track sections and corner objects was a potential source of runtime allocations. Although the turning mechanic was later removed from the intended gameplay, the pooling work helped me understand object reuse and runtime memory allocation. I implemented `TrackPool` and `CornerPool`, prewarmed reusable objects, and checked that they were reused. I also used `Physics.OverlapBoxNonAlloc` and reduced unnecessary score-text updates. Profiling showed 0 B GC allocation for `TrackGenerator.GenerateNextTrack` and `RunnerMovement.Update` in the sampled calls. The mean frame time was 16.59 ms, while a separate 600-frame sampler reported a p99 of 33.04 ms. Because these figures came from different sampling windows, I cannot claim that pooling alone caused a measured improvement in overall frame time. The stronger conclusion is that the targeted methods avoided allocations in the captured samples.

An asynchronous scene-loading experiment provided another useful lesson. Changing the loading loop to wait for `operation.isDone` while scene activation was disabled left the game on its loading screen. I restored the progress-based condition and confirmed that the Menu scene loaded again. This taught me to consider the state of an asynchronous operation rather than treating every completion flag as interchangeable.

A playtester also struggled to discover the swipe gestures and attempted to jump over the full-width overhead obstacle instead of rolling. The mechanics worked technically, but they were not sufficiently communicated. I therefore identified a short introductory tutorial as a priority for later work.

## Week 6 — Economy, progression and telemetry

In Week 6, I added a soft-currency loop: players collect coins during gameplay, spend them on speed upgrades, and benefit from faster forward movement. The upgrade cost starts at 100 coins and grows with level. Each upgrade adds 10% of the current base movement speed per level, while the game continues to increase its base speed during a run. I also added a simulated 500-coin purchase through a fake purchase provider. It is a development demonstration, not a real-money transaction.

I tested the economy during a continuous three-minute Android run. The first upgrade was purchased after 43 seconds, meeting the target of making it affordable within two minutes. A second upgrade became affordable at 1 minute 45 seconds; the run ended with 297 coins remaining and one upgrade purchased. I did not change coin rewards during this pacing pass because the first-upgrade target was already met. In retrospect, this was a useful initial check rather than proof of long-term balance. More players and runs would be needed to judge whether progression remains interesting.

I then implemented local telemetry for `session_start`, `level_start`, `level_fail`, `level_complete` and `upgrade_purchased`. Events are written to the Unity console and a local `telemetry.log` file. I separated coins earned through gameplay from simulated purchases so that run-level rewards could be interpreted correctly. I also saved wallet coins and upgrade level through `PlayerPrefs` and tested Home/resume and force-stop/relaunch on Android. The wallet values were restored, and the telemetry log could be exported. The `level_complete` event exists in code but is not triggered by normal endless-runner play because there is no victory condition; this is a limitation I need to describe rather than hide.

## Week 7 — Sustained performance and limits of evidence

Week 7 focused on whether the game could sustain its performance during a longer session. I added a thermal overlay and conducted a ten-minute test on the Huawei device in Balanced mode. The recorded battery temperature was 35.0°C at both the beginning and end. Mean frame time changed from 16.64 ms to 16.58 ms, while p99 changed from 16.92 ms to 16.53 ms. These observations suggest stable frame pacing in that test, without a clear sustained slowdown.

However, the Adaptive Performance provider remained inactive, and no throttling event was observed in the captured logs. I therefore cannot conclude that thermal throttling was impossible or that the device's thermal management was fully measured. The test helped me distinguish between an observed result and a stronger claim that the available instrumentation cannot support. I also learned to record the build type, device, test duration and capture conditions so later comparisons are meaningful.

## Week 8 Lab A — Preparing a distributable build

In Week 8 Lab A, I shifted attention from local development to making the game accessible for external testing. I prepared a release-signed ARM64 APK for version 0.8.0, verified installation on the Huawei phone, and recorded the build identifier. I uploaded the APK for distribution and created a QR code that could be used to retrieve it. I also prepared an anonymous feedback form and linked it from the game's About screen.

This work revealed a lifecycle and UI issue: returning to the game after opening the feedback form in a browser could leave the Pause and About panels overlapping. I adjusted pause-state handling to avoid repeating a state transition when the requested state had not changed, and ensured the pause panel did not appear over the About or Settings panels. This reinforced that distribution and external links introduce test cases that are easy to miss during ordinary gameplay.

The APK link and QR download were checked, including a file-hash comparison, but peer feedback and a wider device matrix were not completed during Lab A. I should not present distribution readiness as evidence of successful multi-device testing. Those remain follow-up tasks.

## Scope decisions, next steps and conclusion

My scope evolved during development. I initially experimented with a turning track but later switched to a straight three-lane design because it offered more interesting gameplay and greater obstacle variety. I also introduced the idea of a changing anxiety-dream setting, but kept the core endless-runner mechanics as the priority. I concentrated on one working content lane, obstacle variety, progression and reliable Android operation instead of expanding immediately into multiple environments. These decisions helped me focus on gameplay quality rather than adding features simply because they had been planned or implemented.

For CA3, I intend to improve onboarding for swipe, jump and roll controls, gather external feedback, test on additional devices where possible, and continue polishing obstacles, difficulty and UI. I also need to validate performance with consistent capture methods and avoid interpreting individual profiler frames as whole-run averages. The most important lesson from this period is that evidence changes how I evaluate progress: a feature is stronger when I can explain how it behaves, how it was tested, and what the results do—and do not—prove.

## References and AI-use disclosure

**Development evidence:** *Forward* project journal; Unity Profiler captures; Android ADB startup and device tests; local telemetry logs; thermal-soak records; Week 8 Lab A distribution and QA records.

**AI use:** I used ChatGPT to help interpret Unity and Android profiling results, troubleshoot development issues, and structure this reflection. All implementation details and test results are based on my own project work and development records. I reviewed the document to ensure that it accurately reflects my development process.
