# Week 8 Lab B – QA Test Plan

**Date:** 17 November 2026  
**Game:** Forward  
**Device:** HUAWEI nova 5 Pro (Android 10)  
**Package:** `com.ai.forward`  
**Build:** `Forward-v0.8.0-qa-orientation-fix.apk`

## Test Results

|#|Scenario|Trigger|Expected|Pass/Fail|Evidence|
|---|---|---|---|---|---|
|1|Resume / Background|Home, wait 30 seconds, return; force-stop and relaunch|Game resumes correctly, Pause Menu works and settings persist|Pass (Retest)|Initial 5–7s delay; later retest showed immediate Pause Menu. Force-stop preserved Haptics settings|
|2|No Network|Enable Airplane Mode before launch and during gameplay|Game remains playable without network access|Pass|Both offline tests completed without gameplay issues or new network errors|
|3|Rotation|Rotate device between portrait and landscape|Portrait orientation maintained, UI remains visible|Pass (Fixed)|Initial black screen in landscape; changed Default Orientation to Portrait; retest passed|
|4|Battery Saver|Enable Power Saving Mode and play for 2 minutes|Game remains playable with responsive controls|Pass|Minor frame drops observed; no crashes or new Logcat errors|
|5|Low RAM|Send RUNNING_CRITICAL trim notification, background game and open other apps|Game resumes without black screen or crash|Pass|No issues after memory trim or background multitasking|
|6|Cold / Background Relaunch|Force-stop and launch; Home for 10 seconds and relaunch|Game launches successfully without hanging|Pass|Cold: 227 ms; Hot: 64 ms|

## Issues Identified

**Issue 1 – Background Resume Delay**

The initial build experienced a 5–7 second delay before displaying the Pause Menu after returning from the background. A subsequent test after rebuilding did not reproduce the issue. Further repeated testing is recommended.

**Issue 2 – Screen Orientation**

The initial build allowed landscape rotation, resulting in a partially black screen. The issue was resolved by changing Unity's Default Orientation from Auto Rotation to Portrait.

**Issue 3 – Adaptive Performance Initialisation**

Logcat repeatedly reported Google Android Adaptive Performance initialisation errors. The game remained playable, but the underlying cause has not yet been investigated.

## Performance Measurements

|Metric|Result|
|---|---|
|Total PSS|198,215 KB (193.57 MiB)|
|Total Swap PSS|187 KB|
|Cold Start|227 ms|
|Background Relaunch (HOT)|64 ms|

## Overall Result

All six scenarios completed their initial test procedures. The orientation issue was fixed and verified. The background resume delay was not reproduced in a subsequent test, while minor frame drops under Battery Saver and Adaptive Performance initialisation errors remain documented observations.