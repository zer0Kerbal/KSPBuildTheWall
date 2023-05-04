---
permalink: /ManualInstallation.html
title: Manual Installation
description: the flat-pack Kiea instructions, written in Kerbalese, unusally present
tags: installation,directions,page,kerbal,ksp,zer0Kerbal,zedK
---
<!-- ManualInstallation.md v1.1.0.0
SafetyWall (WALL)
created: 01 Apr 2019
updated: 01 Apr 2023

TEMPLATE: ManualInstallation.md v1.1.9.1
created: 01 Feb 2022
updated: 26 Apr 2023

based upon work by Lisias -->

## [SafetyWall (WALL)][mod]

An add-on that adds a safety fence around the Kerbal Space Center, for Kerbal Space Program.

## Installation Instructions

### Using CurseForge/OverWolf app or CKAN

You should be all good! (check for latest version on CurseForge)

### If Downloaded from CurseForge/OverWolf manual download

To install, place the `godarklight` folder inside your Kerbal Space Program's GameData folder:

* **REMOVE ANY OLD VERSIONS OF THE PRODUCT BEFORE INSTALLING**
  * Delete `<KSP_ROOT>/GameData/godarklight/SafetyWall`
* Extract the package's `godarklight/` folder into your KSP's GameData folder as follows:
  * `<PACKAGE>/godarklight` --> `<KSP_ROOT>/GameData`
    * Overwrite any preexisting folder/file(s).
  * you should end up with `<KSP_ROOT>/GameData/godarklight/SafetyWall`

### If Downloaded from SpaceDock / GitHub / other

To install, place the `GameData` folder inside your Kerbal Space Program folder:

* **REMOVE ANY OLD VERSIONS OF THE PRODUCT BEFORE INSTALLING**
  * Delete `<KSP_ROOT>/GameData/godarklight/SafetyWall`
* Extract the package's `GameData` folder into your KSP's root folder as follows:
  * `<PACKAGE>/GameData` --> `<KSP_ROOT>`
    * Overwrite any preexisting file.
  * you should end up with `<KSP_ROOT>/GameData/godarklight/SafetyWall`

## The following file layout must be present after installation

```markdown
<KSP_ROOT>
  + [GameData]
    + [godarklight]
      + [SafetyWall]
        ...
      + [Plugins]
          SafetyWall.dll
      * #.#.#.#.htm
      * Attributions.htm
      * BSD-2-Clause.txt
      * changelog.md
      * SafetyWall.version
      * ManualInstallation.htm
      * readme.htm
    ...
    * ModuleManager.ConfigCache
  * KSP.log
  ...
```

### Dependencies

* none

THIS FILE: CC BY-ND 4.0 by [zer0Kerbal](https://github.com/zer0Kerbal)
  used with express permission from zer0Kerbal

[mod]: https://www.curseforge.com/kerbal/ksp-mods/SafetyWall "SafetyWall (WALL)"
