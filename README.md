# TUNIC Rando Tracker

A local Windows app that parses the [TUNIC Randomizer](https://github.com/silent-destroyer/tunic-randomizer) ItemLog.

![image](https://github.com/radicoon/tunic-rando-tracker/assets/131428651/cfc42a6f-ddc4-47e6-b90b-f4698faab3b3)

## Instructions

1. Insure that the TUNIC Randomizer is exporting an ItemLog.json file (this is default behavior as of v0.11) and one exists (start a New Game once ever with 0.11+).
1. [Download the latest release](https://github.com/radicoon/tunic-rando-tracker/releases).
1. Unzip the folder anywhere.
1. Open the "TunicRandoTracker" shortcut (if this doesn't work, dive into the `winui-bs` folder and open `TunicRandoTracker.exe`).
1. Enjoy! You can right-click to adjust the ratio of the grid.

## Known Issues or Shortcomings

* The tracker will not work if the ItemLog.json file is created after opening the tracker (start a New Game once ever with 0.11+ to solve this).
* Healing flasks and flask shards are not shown.

## Versions

### 1.0

- Initial release
- Supports both Randomizer and Hexagon Quest modes