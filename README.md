# torus-direct-unity-sdk

Torus CustomAuth (aka TorusDirect) SDK for Unity applications.

## How to use

Please watch our [quick tutorial video](https://www.loom.com/share/caa9d6b5655c4c2990adcf719e52de74) or [more in-depth tutorial video](https://www.loom.com/share/fe0cf944958943df9567f29ef2c8092f).

### Install the SDK

- Go to [the latest release](/releases), download the `TorusDirect.unitypackage`

- Import `TorusDirect.unitypackage` into your Unity project (drag and drop it into Project view in Unity Editor)

- Drag and drop `Scripts/TorusDirect.cs` to your Main Camera (or any game object that exists during your login scene)

- Add event handlers to your UI/game and call one or several of following functions to login your user in:

  - `TorusDirect#LoginWithGoogle`
  - `TorusDirect#LoginWithFacebook`
  - `TorusDirect#LoginWithTwitch`
  - `TorusDirect#LoginWithReddit`
  - `TorusDirect#LoginWithGithub`
  - `TorusDirect#LoginWithTwitter`
  - `TorusDirect#LoginWithLinkedIn`
  - `TorusDirect#LoginWithEmailPassword`
  - `TorusDirect#LoginWithEmailPasswordless`
  - `TorusDirect#LoginWithSMSPasswordless`

- Listen to event `TorusDirect#OnPostLogin` to retrieve login result (or error) including your user's keypair.

### Build for Android

Make sure to change the minimum Android API level to 21 (Android 5.0) or above (**Player Settings**/**Other Settings**/**Identification**)

When you change `appRedirectUri` in `TorusDirect` component, make to update the same value to `Android/launcherTemplate.gradle`

### Build for iOS

After Unity generated the Xcode project for your iOS build, you'll have to add the Swift package [torus-direct-swift-sdk](https://github.com/torusresearch/torus-direct-swift-sdk) manually to the project (we could't find any to do this automatically, contribution is welcome):

- Go to **File**/**Swift Packages**/**Add Swift Package***

- Enter https://github.com/torusresearch/torus-direct-swift-sdk, select the lastest version

- Choose to add this package to `UnityFramework` target (not `Unity-iPhone`)

- Done! You should be able to build the project now

When you change `appRedirectUri` in `TorusDirect` component, make to update the corresponding scheme in **Player Settings**/**Other Settings**/**Supported URL schemes**.

## Development

### Develop Android native plugin

- Open `ProjectPlugins/Android` in Android Studio

- Make changes to module `torusdirect`

- Run `build` task, which will compile the module into an AAR and automatically copy it into the Unity project (at `Assets/Plugins/Torus/Android`)

### Develop managed C# scripts

- Open the repository in Unity Hub/Editor 2020

- Make changes to `Assets/Plugins/Torus/**/*` and `Assets/Scripts/TorusDirect.cs`

- Low-level APIs should be written into `Assets/Plugins/Torus/Classes/*` (`namespace Torus.Classes`)

- Component/script APIs should be written into `Assets/Plugins/Torus/Scripts/*` (`namespace Torus`)

- `Assets/Scripts/TorusDirect.cs` is an example script for user to quickly get started

### Release new version

- Open the repository in Unity Hub/Editor 2020

- Select **File**/**Build Settings**/**Android**/**Build** to build the project into an APK and make sure it's working correctly

- Select **Assets**/**Export Package**, unselect **Include dependencies**, select files to export:
  - `Plugins/Torus`
  - `Plugins/Android`
  - `Scripts/TorusDirect.cs`

- Name the package `TorusDirect.unitypackage`

- Create a new Github release and upload `TorusDirect.unitypackage` as a release artifact
