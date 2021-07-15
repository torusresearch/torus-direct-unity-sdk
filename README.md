# torus-direct-unity-sdk

Torus CustomAuth (aka TorusDirect) SDK for Unity applications.

## How to use

Please watch [our tutorial video](https://www.loom.com/share/fe0cf944958943df9567f29ef2c8092f).

## Development

### Develop Android native plugin

- Open `ProjectPlugins/Android` using Android Studio

- Make changes to module `torusdirect`

- Run `build` task, which will compile the module into an AAR and automatically copy it into the Unity project (at `Assets/Plugins/Torus/Android`)

### Develop managed C# scripts

- Open the repository with Unity Hub/Editor 2020

- Make changes to `Assets/Plugins/Torus/**/*` and `Assets/Scripts/TorusDirect.cs`

- Low-level APIs should be written into `Assets/Plugins/Torus/Classes/*` (`namespace Torus.Classes`)

- Component/script APIs should be written into `Assets/Plugins/Torus/Scripts/*` (`namespace Torus`)

- `Assets/Scripts/TorusDirect.cs` is an example script for user to quickly get started

### Release

- Open the repository with Unity Hub/Editor 2020

- In Unity Edtior, select **File**/**Build Settings**/**Android**/**Build** to build the project into an APK and make sure it's working correctly

- In Unity Editor, select **Assets**/**Export Package**, unselect **Include dependencies**, check only `Plugins/Torus` and `Scripts/TorusDirect.cs` to export, name the package `TorusDirect.unitypackage`

- Create a new Github release and upload `TorusDirect.unitypackage` as a release artifact
