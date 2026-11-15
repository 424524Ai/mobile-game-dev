# Economy Telemetry Map

## Event Map

|Event|Parameters (type)|Fires when|Question it answers|
|---|---|---|---|
|`session_start`|`app_version` (string), `device_model` (string), `android` (string)|Application starts in Boot|Which build and device produced the log?|
|`level_start`|`level_id` (int), `attempt` (int), `coins` (int)|A new gameplay attempt begins|How many attempts have been made, and how many coins does the player have?|
|`level_complete`|`level_id` (int), `time_s` (float), `coins_earned` (int), `score` (int)|Game enters the Won state|How long did a successful attempt last, and how many coins were earned?|
|`level_fail`|`level_id` (int), `time_s` (float), `cause` (id), `coins_earned` (int)|Player loses after hitting an obstacle|How long did the attempt last, why did it end, and how many coins were earned?|
|`upgrade_purchased`|`upgrade_id` (id), `level` (int), `cost` (int), `coins_left` (int)|A speed upgrade is successfully purchased|When are upgrades purchased, and how does spending affect the economy?|

## Data Storage and Privacy

Telemetry events are recorded locally through Unity Debug.Log (visible in Android Logcat) and appended to `telemetry.log` in `Application.persistentDataPath`. No analytics SDK is used, and no telemetry data is transmitted off the device. This local data collection is also disclosed in `docs/CA1/privacy-statement.md`.

**Note:** Forward is an endless runner without a current victory condition. The `level_complete` event is implemented for the Won state but is not triggered during normal gameplay.