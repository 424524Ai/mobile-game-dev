# Data-Use and Privacy Statement
## Current Data Use
Forward is currently an offline Android game prototype. The game does not require a user account or login.

The current version does not collect, transmit, or share personal information such as the player's name, email address, location, photos, contacts, or other personal data.

The game does not currently use:
- User accounts or login systems
- Online servers, databases or APIs
- Analytics services
- Advertising services
- Third-party data collection SDKs

## Local Data

Forward uses Unity PlayerPrefs to store game settings and economy progress locally on the player's device.

The locally stored data includes:

- Haptics preference
    
- Screen shake preference
    
- `wallet.coins` — The player's current coin balance
    
- `wallet.level` — The player's purchased upgrade level
    

This information is used to remember player settings and preserve economy progress between game sessions. It is stored locally on the device and is not transmitted to a server or shared with third parties.

The game includes a simulated in-app purchase feature for educational purposes. No real payments are processed, and no payment information is collected.

Players can remove locally stored settings and economy progress by uninstalling the app. Uninstalling normally removes the app's local data.

### Local Telemetry

Forward records gameplay telemetry locally for testing and economy balancing. Events include session starts, gameplay attempts, game results and upgrade purchases.

Telemetry is written to Unity's Debug Log and stored in a `telemetry.log` file within the application's persistent data directory (`Application.persistentDataPath`).

No third-party analytics SDK is used, and telemetry data is not automatically transmitted to external servers. The log file persists between app launches and may grow as additional events are recorded.

## Google Play Data Safety
If Forward were published on Google Play in its current state, the Data safety declaration would reflect that the game does not currently collect or share personal user data.

The declaration would be reviewed and updated before publication if future versions of the game add features that change its data use, such as analytics, advertising, online services, user accounts, or other third-party services.

## Future Changes
Forward is still in development. Any new feature that collects, transmits, or shares user data would require this privacy statement and the Google Play Data safety declaration to be reviewed and updated.

## Permissions Requested
The current Android APK requests the following permissions:
- **INTERNET:** Allows network access. The game currently has no intended online features or server communication.
- **VIBRATE:** Used for haptic feedback when vibration is enabled.
Both permissions were confirmed using ADB on the installed Release APK.

## Network Activity
Forward does not currently include any online gameplay features or network requests in its game scripts. The Android APK requests the INTERNET permission, but the game does not intentionally use it to communicate with online servers.

## Third-Party SDKs
No advertising, analytics, or third-party data collection SDKs were identified in the current Unity project.