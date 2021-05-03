# Blindspot #

This repo is for project Blindspot, working with CMU D3 Lab to design a transformational game that help raise players' awareness of microaggression towards marginalized group. The final deliverable is a Unity Webgl game, more info about the project such as team and dev blogs could be found at [Blindspot Project Website](https://projects.etc.cmu.edu/blindspot/)

## Repository Structure 
All the assets for this project are placed under the [Assets](https://github.com/utherrrr0317/Blindspot/tree/main/Game%20Project/Assets) folder. Below are some subfolders:
* [Resources](https://github.com/utherrrr0317/Blindspot/tree/main/Game%20Project/Assets/Resources) contains all the art and sound effects used in the game.
* [Scripts](https://github.com/utherrrr0317/Blindspot/tree/main/Game%20Project/Assets/Scripts) contains all the C# scripts used in this game.
* [Materials](https://github.com/utherrrr0317/Blindspot/tree/main/Game%20Project/Assets/Materials) contains all the custom materials used in the game, and the shaders from which these materials are created could be found under the [Shaders](https://github.com/utherrrr0317/Blindspot/tree/main/Game%20Project/Assets/Shaders) folder.
* [StreamingAssets](https://github.com/utherrrr0317/Blindspot/tree/main/Game%20Project/Assets/StreamingAssets) contains all the assets that are loaded during runtime, including json files that hold the dialogue info, and the flashback videos.
* [AstarPathFinding](https://github.com/utherrrr0317/Blindspot/tree/main/Game%20Project/Assets/AstarPathfindingProject) contains the free dependency for path finding in this game. The original website could be found [here](https://arongranberg.com/astar/)
* [TextMeshPro](https://github.com/utherrrr0317/Blindspot/tree/main/Game%20Project/Assets/TextMesh%20Pro) contains the TextMeshPro components that are used in displaying ui text in the game. All the fonts used in the game could be found under the [Font](https://github.com/utherrrr0317/Blindspot/tree/main/Game%20Project/Assets/TextMesh%20Pro/Fonts) folder.
* [Timelines](https://github.com/utherrrr0317/Blindspot/tree/main/Game%20Project/Assets/Timelines) contains all the timeline assets and the signals used in Unity's Playable Director component.

## Prerequisite for building the webgl game
* Use Unity Version **2021.1.0b12** or later.
* Install all the packages used in this game from package manager and unity asset store, including [Dotween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676) and [Universal Render Pipeline](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0/manual/index.html).
* Make sure to include all the custom shaders in build setting.

## Know issues
