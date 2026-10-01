## Device and test environment
- Device: HUAWEI nova 5 Pro
- Orientation: Portrait
- Simulator: 1080 x 2340
- Text sizes: Small (0.85), Normal (1.0), Large(1.25)

## Accessibility Checklist
| Check                             | Finding / Fix                                                                                                                                                                                                        |
| :-------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| One-handed reach                  | All controls are reachable with one hand, some buttons were initially too small, so their touch targets were enlarged. Tapping accuracy improved.                                                                    |
| Minimum touch target 48dp         | Not tested.                                                                                                                                                                                                          |
| Maximum Android display/text size | Tested with maximum Android font and display sizes. All UI elements remained visible and within the safe area. No issues found.                                                                                      |
| Monochromacy                      | Tested monochromacy, deuteranomaly, protanomaly and tritanomaly on an Android device. All current UI elements remained readable. No issues found.                                                                    |
| Volume at zero                    | Tested with the phone muted. All current menu controls have text labels and remain understandable without sound. No issues found. Gameplay feedback will need further testing once implemented. Done for current UI. |
| Text contrast                     | All button labels and the Haptics Toggle label use dark grey text (#323232) on white backgrounds (#FFFFFF). Contrast ratio: approximately 12.8:1. Passed.                                                            |
| Motion and screen shake           | Added a Screen Shake toggle that saves the user's preference using PlayerPrefs. No screen shake is currently implemented.                                                                                            |
| Timed interactions                | No timed interactions in the current menus. Players can adjust settings at their own pace. Gameplay will be reviewed once implemented. Done for current UI.                                                          |
| Haptics disabled                  | Tested with haptics disabled. All current controls remain usable and no essential information is lost.                                                                                                               |
| 60-second observation test        | Not tested.                                                                                                                                                                                                          |
 



