U3D-AudioMixer
==============

AudioMixer is a Unity script written in C# that attempts to implement an audio mixer with channels. In Unity, the most typical way to control audio was though priorities, which could be set in a sound's properties. There was never a built-in system to relay audio to a specific channel.

AudioMixer implements audio channeling via a script with the same name that is attached to a GameObject. Entities in your game that need to play a sound (or music) can call AudioMixer to play a sound in a specified channel of your choice. Notice that this is not a Unity extension and thus does **not** make audio channels a built-in feature in Unity.

AudioMixer was made for music and 2D sounds in mind. This is **not** recommended for 3D sounds. 3D sounds require a location, which would be too cumbersome to implement in AudioMixer.

You can read my reason for start this [here](http://jibransyed.wordpress.com/2013/09/27/playing-around-with-audio-in-unity/).

### Demo Scene

The demo scene ( `AM_TestScene.unity` ) features an array of keys on the screen that play sound in multiple channels. By default there are 8 channels and 2 sounds to test per channel for a total of 16 sounds. 

You should be able to play audio from multiple channels at the same time. If you play a sound for channel X and then play another sound that is also for channel X, the first invoked sound stops immediatly and the second sound plays at that channel.

Channel 1 plays looping music. The rest are sound effects or jingles.

The demo project requires Unity 4.2 or greater.

The functionality of the keys can be changed by clicking on a key in the scene and adjusting the inspector.

![demoScreen](https://pbs.twimg.com/media/BVNV156CQAAigbq.png)

### How to Use

Put the [`AudioMixer.cs`](https://github.com/JISyed/U3D-AudioMixer/blob/master/AudioMixerProj/Assets/U3D-AudioMixer/Scripts/AudioMixer.cs) script somewhere in your `/Assets` folder. Then place the prefab [`AudioMixerObject.prefab`](https://github.com/JISyed/U3D-AudioMixer/blob/master/AudioMixerProj/Assets/U3D-AudioMixer/Resources/AudioMixerObject.prefab) into a folder called `/Resources`. This folder *must* be somewhere in the `/Assets` folder, so for example: `/Assets/Resources/AudioMixerObject.prefab`. 

The next step in important if you're placing AudioMixer into a new Unity project. Go to the `AudioMixerObject` prefab and in the inspector, make sure the `AudioMixer` script is attatched. It most likely will not be attatched if you took AudioMixer assets into a new project.

Whenever you want to play a sound in code, you can call the AudioMixer like this:

```csharp
using UnityEngine;
using System.Collections;

public class SoundPlayerScriptExample : Monobehavior
{
    public AudioClip soundEffect;
    public int audioMixerChannel = 1;
    public AudioMixerChannelTypes audioType = AudioMixerChannelTypes.Sound;
    
    void Start()
    {
        AudioMixer.Play(audioMixerChannel, soundEffect, audioType);
    }
}
```

Notice that this is a static call. An instance of the AudioMixerObject prefab will be created when needed, so you don't have to.

Be default, AudioMixer sets up 8 channels (which are actually [`AudioSources`](http://docs.unity3d.com/Documentation/ScriptReference/AudioSource.html)). If you would like more channels, change the constant `NUM_OF_CHANNELS`, which can be found right after AudioMixer's class declaration. Notice that changing this constant will NOT adjust anything in the demo scene.

### Documentation

For documentation, please refer to the [wiki](https://github.com/JISyed/U3D-AudioMixer/wiki).
