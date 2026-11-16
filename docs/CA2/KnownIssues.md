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
