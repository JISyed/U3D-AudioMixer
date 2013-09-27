U3D-AudioMixer
==============

AudioMixer is a Unity script written in C# that attempts to implement an audio mixer with channels. In Unity, the most typical way to control audio was though priorities, which could be set in a sound's properties. There was never a built-in system to relay audio to a specific channel.

AudioMixer implements audio channeling via a script with the same name that is attatched to a GameObject. Entities in your game that need to play a sound (or music) can call AudioMixer to play a sound in a speficied channel of your choice. Notice that this is not a Unity extention and thus does **not** make audio channels a built-in feature in Unity.

AudioMixer was made for music and 2D sounds in mind. This is **not** recommended for 3D sounds. 3D sounds require a location, which would be too cumbersome to implement in AudioMixer.

### Demo Scene

The demo scene ( `AM_TestScene.unity` ) features an array of keys on the screen that play sound in multiple channels. By default there are 8 channels and 2 sounds to test per channel for a total of 16 sounds. 

You should be able to play audio from multiple channels at the same time. If you play a sound for channel X and then play another sound that is also for channel X, the first invoked sound stops immediatly and the second sound plays at that channel.

Channel 1 plays looping music. The rest are sound effects or jingles.

The demo project requires Unity 4.2 or greater.

![demoScreen](https://pbs.twimg.com/media/BVLxP0SCMAAl7KH.png)
