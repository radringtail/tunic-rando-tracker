# TUNIC Rando Tracker

A local Windows app that parses the [TUNIC Randomizer](https://rando.tunic.run) ItemLog.

![image](https://github.com/radicoon/tunic-rando-tracker/assets/131428651/cfc42a6f-ddc4-47e6-b90b-f4698faab3b3)

## Instructions

1. Insure that the TUNIC Randomizer is exporting an ItemLog.json file (this is default behavior as of v0.11) and one exists (start a New Game once ever with 0.11+).
1. [Download the latest release](https://github.com/radicoon/tunic-rando-tracker/releases/latest).
1. Unzip the folder anywhere.
1. Open the "TunicRandoTracker" shortcut (if this doesn't work, dive into the `winui-bs` folder and open `TunicRandoTracker.exe`).
1. Enjoy! You can right-click to adjust the ratio of the grid.

## Known Issues or Shortcomings

* The tracker will not work if the ItemLog.json file is created after opening the tracker (start a New Game once ever with 0.11+ to solve this).
* Healing flasks and flask shards are not shown.
* The text shadow sometimes renders wrong but it goes away quickly.

## Versions

### 1.1

- Icons only re-draw when images are replaced (i.e., much less app flickering)
- Replaced quantity font with Odin Rounded (the menu font in TUNIC)
- Added a second theme: circles (to imitate the look of TUNIC's UI)
- Added a background color selection (to allow keying for the cicles theme)
- Added a 24x1 arrangement option
- Rearranged icons a bit to match ending screen (sort of) and be better grouped (maybe)

### 1.0

- Initial release
- Supports both Randomizer and Hexagon Quest modes