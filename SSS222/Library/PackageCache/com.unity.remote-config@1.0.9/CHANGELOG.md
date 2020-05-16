# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.9] - 2020-02-12

## This is v1.0.9 release of *Unity Remote Config*
- Adjusted the delete button for settings to the right most column; previously it was on the left.
- Fixed bug where float values were parsed incorrectly
- Added info tooltip to Rules Condition Label, and help button linking to JEXL syntax documentation

## [1.0.8] - 2019-11-14

## This is v1.0.8 release of *Unity Remote Config*
- Added confirmation dialog for unsaved changes on closing Remote Config window and changing environments
- Fixed bug where `RemoteConfigDataManager` would try set the `RemoteConfigDataStoreAsset` dirty when it doesn't exist
- Added documentation for `unity.model` predefined attribute
- The Remote Config window now supports having duplicate setting key names, but the backend will still reject duplicate setting keys
- On a failed push to servers, the Remote Config window will re-fetch everything, so that users see the state of their environment on the server
- Added a button to the Remote Config window that will take you to the Remote Config dashboard
- Now the release environment will have "Default" next to it in the environment dropdown
- The cursor now turns into an editing cursor when hovering over editable fields in the Remote Config window
- Changed "-" remove button to trashcan icon
- Fixed bug on Windows which would cause the name of a rule to wrap onto the lower line when the Remote Config window is small

## [1.0.7] - 2019-10-10

## This is v1.0.7 release of *Unity Remote Config*
- Boolean checkbox is now a dropdown with `True` and `False`
- The "Rollout Percentage" slider label is now an editable text field as well
- Added support for more unity attributes : `unity.cpu`, `unity.graphicsDeviceVendor` and `unity.ram`
- Fixed floating point rounding issue
- The editor window now saves the last fetched environment, configs and rules on editor close, and on playmode enter

## [1.0.6] - 2019-09-18

## This is v1.0.6 release of *Unity Remote Config*
- Updated with Privacy ToS
- Updated Documentation to clarify install and integration instructions
- Updated a Screenshot in Documentation
- Fixed formatting in UI onboarding text

## [1.0.5] - 2019-09-14

## This is v1.0.5 release of *Unity Remote Config*
- Updated License Documentation
- Removed unneeded markdown files to clean up for release

## [1.0.4] - 2019-09-04

## This is v1.0.4 release of *Unity Remote Config*
- Added a label under start & end date/time to show the format that is expected.
- Added some warning messages in the RC management window:
	- Warning for when there are no settings
	- Warning for when there are no settings in a rule
- Changed the name of the "Default Config" to "Settings Config".
- Updated documentation to reflect the change of string value character limits from 1024 to 10000 characters.
- The enable/disable, delete, and priority fields are now hidden for the "Settings Config" in RC Management since they cannot be edited.

## [1.0.3] - 2019-08-22

## This is v1.0.3 release of *Unity Remote Config*
- Fixed instability with the RC Management Window that would keep it in a loading state after pushing a new rule.


## [1.0.2] - 2019-08-21

## This is v1.0.2 release of *Unity Remote Config*
- This is a version bump to prepare the package for verification for 2019.3

## [0.3.2] - 2019-07-26

## This is v0.3.2-preview release of *Unity Remote Config*
- The input field on each setting is now the type of that setting, so developers don't have to worry about having incorrect values.
- Added warning when a setting key name reached 255 characters.

## [0.3.1] - 2019-07-11

## This is v0.3.1-preview release of *Unity Remote Config*
- Fixed bug that caused the Remote Config Management Window to not display correctly after a domain reload.
- Added support for settings of type 'long'
- Added slider control for rollout percentage
- The UI will now properly recover from any server-side errors.
- The UI will now reject duplicate rule names rather than depending on the Service API
- Moved Remote Config configuration requests to new URIs in the API Gateway
- Removed reliance on API Gateway URL that needs `-prd` at the end, so developers _always_ interact with production backend services.
- The "All Users" pseudo-rule, is now named what it actually is, "Default Config."

## [0.3.0] - 2019-07-01

## This is v0.3.0 preview release of *Unity Remote Config*.
- New runtime wrapper added for easier integration of Unity Remote Config. Please see documentation for more info.
    - New runtime classes are: `Unity.RemoteConfig.ConfigMananger` and `Unity.RemoteConfig.RuntimeConfig`.
    - `ConfigManager` is meant the be the primary way developers interact with Unity Remote Config.
- Fixed a bug where deleting a setting also deletes it from the rules that reference the deleted setting. Now, deleting a setting will not delete it from the corresponding rules.
- Removed errant Debug Logs.
- Window loader will now appear when settings are pushed (previously it was only happening if a rule was pushed as well).

## [0.2.1] - 2019-05-28

## This is the tenth early preview release of *Unity Remote Config*.
- Analytics no longer needs to be enabled in order to use Unity Remote Config. We now only require the project to be assiociated with an organization. In order to do so, go to Window > Services and follow the prompts.
- General UI and stability fixes
- Fixed bug which allowed settings to be deleted when they are in an active rule

## [0.2.0] - 2019-05-08

## This is the ninth internal release of *Unity Remote Config*.
- Package name has changed to Unity Remote Config.
- Name spaces are now: `Unity.RemoteConfig`
   - `UnityEditor.RemoteSettings` -> `Unity.RemoteConfig.Editor` for example

## [0.1.0] - 2019-05-06

### This is the eighth internal release of *Unity Package \ Remote Settings*.
- startDate and endDate have been added to rules to allow for calendarization
- startDate and endDate Default is null
- Supported format is ISO8601 in the format "yyyy-MM-dd'T'HH:mm:ssZ" example: "2019-04-29T15:01:43Z"
- .AI prefix removed from codebase, new namespaces name is now UnityEditor.RemoteSettings
- bug fixes
- better handling of server response errors
- The type field for Remote Settings now have a dropdown containing all supported types
- Added a rule priority field, which determines which rules will overwrite the values of other rules.
    - Rule priority ranges from 0-1000, 0 being the highest priority rule (will overwrite all other rule values), and 1000 is the lowest priority rule.
    - If two or more rules have the same priority, they will be evaluated from newest to oldest (the oldest rule will overwrite the matching keys of newer rules)
    - The rule priority can be set by the new column in the RS Management window in the list of rules.
- The workflow for adding a Remote Setting to a rule has changed: No more dropdown. Now, on a Rule, all Remote Settings will be visible, and in order to add it to a rule, just click the checkbox to the left of the key. And to remove it, just uncheck the same box.
- GUI performance improvements, and general stability.
- Removed ability to sort from all RS Management window headers, since it didn't work

## [0.0.6] - 2019-04-19

### This is the sixth internal release of *Unity Package \ Remote Settings*.
- The Rules Management and Remote Settings Management Windows have been merged. The new window can be found through Window > Remote Settings > Remote Settings Management
- Deafult Remote Settings are now under the "All Users" rule.
- Pull/Push now syncs both Rules and Remote Settings
- The Remote Settings Data Stores are now all merged into one RemoteSettingsDataStore
- RemoteSettingsDataManager can be used to access the RemoteSettingsDataStore in a secure way
- Major stability fixes.
- Updated message when Analytics is disabled to reflect no longer needing a project secret key.

## [0.0.5] - 2019-04-17

### This is the fifth internal release of *Unity Package \ Remote Settings*.
- UI is now blocked while web operations are in progress
- Stability and bug fixes

## [0.0.4] - 2019-03-29

### This is the fourth internal release of *Unity Package \ Remote Settings*.
- Added the capability to add, remove, and edit rules
- Updated namespaces: all editor code is now under `UnityEditor.AI.RemoteSettings`, and runtime code is now under `UnityEngine.AI` instead of `UnityEngine.AI.RemoteSettings`.
- General stability and bug fixes

## [0.0.3] - 2019-03-19

### This is the third internal release of *Unity Package \ Remote Settings*.
- Added more Unit Tests throughout UI code
- Implemented TreeView in the Remote Settings Editor Window
- General optimizations
- Added the ability to push Remote Settings to any environment from the Editor
- Moved UI under Window > Remote Settings > Remote Settings Management

## [0.0.2] - 2019-02-26

### This is the second internal release of *Unity Package \ Remote Settings*.

### Added
- Local overrides checkbox in RS Editor Window, that will force the editor to use local values instead of the cloud.
- Created initial RemoteSettings runtime API wrapper under `UnityEngine.AI.RemoteSettings`
- Added Unit Tests through Unity Test Runner. Each test class needs to run individually due to a limitation in the PrebuildSetup step of Unity Test Runner. Note: The RS Editor window needs to be closed in order to run the tests.

## [0.0.1] - 2019-02-20

### This is the first internal release of *Unity Package \ Remote Settings*.

### Added
- UI under Window > Unity Analytics > Remote Settings
- Added buttons for pushing settings, but not hooked up to APIs (not ready)
- Added ability to create and delete keys locally
- Added ability to update key names, types, and values locally
