U3D-AudioMixer
==============

AudioMixer is a Unity script written in C# that attempts to implement an audio mixer with channels. In Unity, the most typical way to control audio was though priorities, which could be set in a sound's properties. There was never a built-in system to relay audio to a specific channel.

AudioMixer implements audio channeling via a script with the same name that is attatched to a GameObject. Entities in your game that need to play a sound (or music) can call AudioMixer to play a sound in a speficied channel of your choice. Notice that this is not a Unity extention and thus does **not** make audio channels a built-in feature in Unity.

AudioMixer was made for music and 2D sounds in mind. This is **not** recommended for 3D sounds. 3D sounds require a location, which would be too cumbersome to implement in AudioMixer.
