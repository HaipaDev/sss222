# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.1.6] - 2020-04-29

### Changed 
- GameSDK 3.2 uses a wider range of temperature levels and maximum temperature level is changed to level 10. 
- GameSDK 3.2 has a different behaviour when setting frequency levels and warning level 2 (throttling) is reached and you are always in control of the CPU/GPU level.

## [1.1.4] - 2020-03-26

### Removed
- Game SDK 3.1 initialization due to issues in GameSDK 3.1. Any other GameSDK version is still supported.   

## [1.1.3] - 2020-03-18

### Fixed 
- Avoids that callbacks in GameSDK 3.1, such as Listener.onHighTempWarning(), are not called when onRefreshRateChanged() is not implemented. This is only present on devices supporting VRR

### Changed 
-With GameSDK 3.1 it's not necessary to (un)register listeners during OnPause and OnResume as it's handled in the GameSDK

## [1.1.2] - 2020-03-13

### Added 
- Updated GameSDK from 3.0 to 3.1 

### Fixed 
- Avoid onRefreshRateChanged() crash on S20 during Motion smoothness change (60Hz <-> 120Hz) while app is in background and resumed

### Improvement 
- GameSDK 3.1 introduced setFrequLevel callback for temperature mitigation to avoid overheating when no additional scale factors are used. This replaces SetLevelWithScene in GameSDK 3.1   

## [1.1.1] - 2020-02-13

### Changed 
- Package name from AP Samsung Android to Adaptive Performance Samsung Android as the Unity Package Manager naming limit was raised to 50 characters

## [1.1.0] - 2020-01-30

### Fixed 
- Compatibility with .net 3.5 in Unity 2018.4

## [1.0.1] - 2019-08-29

### Changed
- Compatibility with Subsystem API changes in Unity 2019.3

## [1.0.0] - 2019-08-19

### Added
- Support for Samsung GameSDK 3.0

## [0.2.0-preview.1] - 2019-06-19

### This is the first preview release of the *Adaptive Performance Samsung (Android)* package for *Adaptive Performance* which was integrated within Adaptive Performance previously.
