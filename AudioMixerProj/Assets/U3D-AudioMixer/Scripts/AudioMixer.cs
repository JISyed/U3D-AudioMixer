using UnityEngine;
using System.Collections;

/// <summary> Types of audio. Choose from Music, Jingle, Sound, and Voice. </summary>
public enum AudioMixerChannelTypes
{
	Music,
	Jingle,
	Sound,
	Voice
}

public class AudioMixer : MonoBehaviour 
{
	public const int NUM_OF_CHANNELS = 8;
	
	private AudioSource[] channels;
	private bool[] channelsOccupied;
	private bool[] channelsPaused;
	private AudioMixerChannelTypes[] channelsAudioType;
	private static AudioMixer theInstance;
	private static bool instanceFound = false;
	
	// Initilizer
	void Start () 
	{
		// Create the audio channels
		channels = new AudioSource[NUM_OF_CHANNELS];
		channelsOccupied = new bool[NUM_OF_CHANNELS];
		channelsPaused = new bool[NUM_OF_CHANNELS];
		channelsAudioType = new AudioMixerChannelTypes[NUM_OF_CHANNELS];
		for(int i=0; i<NUM_OF_CHANNELS; i++)
		{
			channels[i] = gameObject.AddComponent<AudioSource>();
			channels[i].playOnAwake = false;
			channelsOccupied[i] = false;
			channelsPaused[i] = false;
			channelsAudioType[i] = AudioMixerChannelTypes.Sound;
		}
	}
	
	// Update loop
	void Update () 
	{
		for(int i=0; i<NUM_OF_CHANNELS; i++)
		{
			// Mark a channel as avaliavble if a sound isn't playing and not paused
			if(!channels[i].isPlaying && !channelsPaused[i])
			{
				channelsOccupied[i] = false;
			}
		}
	}
	
	// Used only to get a Gizmo icon in the editor
	void OnDrawGizmos(){}
	
	// Destroy event
	void OnDestroy()
	{
		// Unassign the instance
		theInstance = null;
		instanceFound = false;
		
		// Destroy the channels
		for(int i=0; i<NUM_OF_CHANNELS; i++)
		{
			Destroy(channels[i]);
		}
	}
	
	// Used to check for invalid channel parameter
	private bool ChannelIsValid(int givenChannel)
	{
		if(givenChannel < 1)
		{
			Debug.LogError("AudioMixer: Given channel was invalid for being 0 or less.");
			return false;
		}
		
		if(givenChannel > NUM_OF_CHANNELS)
		{
			Debug.LogError("AudioMixer: Given channel was invalid for being bigger than avaliable channels.");
			return false;
		}
		
		return true;
	}
	
	// Used to find the AudioMixerObject
	private static void FindAMObject()
	{
		GameObject foundMixerObject = null;
		AudioMixer mixerComponent = null;
		
		if(!instanceFound)
		{
			//Debug.Log("Finding Mixer");
			foundMixerObject = GameObject.Find("AudioMixerObject");
			if(foundMixerObject == null)
			{
				//Debug.Log("Mixer not found. Making one.");
				// Change this path if you move AudioMixerObject.prefab somewhere else.
				// AudioMixerObject.prefab must be in a folder called "Resources" or this won't work.
				foundMixerObject = Instantiate( Resources.Load("AudioMixerObject") ) as GameObject;
				mixerComponent = foundMixerObject.GetComponent<AudioMixer>();
				mixerComponent.Start();
			}
			else
			{
				//Debug.Log("Mixer Found!");
			}
			mixerComponent = foundMixerObject.GetComponent<AudioMixer>();
			AudioMixer.theInstance = mixerComponent;
			instanceFound = true;
		}
		else
		{
			//Debug.Log("Mixer already found");
		}
	}
	
	// Finds the first avaliable channel with a linear scan of channels[]
	// Returns a channel in the user's notation (1-8) not programmer's (0-7)
	private static int FindFirstAvaliableChannel()
	{
		int avaliableChannel = -1;
		bool channelFound = false;
		
		// Find an empty channel to play
		for(int i=0; i<NUM_OF_CHANNELS; i++)
		{
			if(!theInstance.channelsOccupied[i])
			{
				avaliableChannel = i;
				channelFound = true;
				break;
			}
		}
		
		// Couldn't find an empty channel, find the lowest priority channel
		if(!channelFound)
		{
			int[] channelPriorities = new int[NUM_OF_CHANNELS];
			int lowestPriority = 0;	// Hint: 0 is highest priority in Unity.
			int channelWithTheLowestPriority = -1;
			
			for(int j=0; j<NUM_OF_CHANNELS; j++)
			{
				channelPriorities[j] = theInstance.channels[j].priority;
				if(channelPriorities[j] > lowestPriority)
				{
					lowestPriority = channelPriorities[j];
					channelWithTheLowestPriority = j;
				}
			}
			avaliableChannel = channelWithTheLowestPriority;
		}
		
		return avaliableChannel + 1; 	// User index notation, not programmer's
	}
	
	// ------- Audio calls ------------
	
	/// <summary> Returns the number of channels that AudioMixer uses. </summary>
	public static int GetNumberOfChannels()
	{
		FindAMObject();
		
		return NUM_OF_CHANNELS;
	}
	
	/// <summary> Plays the given sound clip in the given channel and marks it as the given audio type. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	/// <param name='soundClip'> The audio file itself. Handled in Unity as an AudioClip. </param>
	/// <param name='audioType'> Used to label the audio to a specific type. Choose from Sound, Music, Jungle, and Voice. </param>
	public static void Play(int channel, AudioClip soundClip, AudioMixerChannelTypes audioType)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		theInstance.channels[channel-1].clip = soundClip;
		theInstance.channelsAudioType[channel-1] = audioType;
		
		theInstance.channelsOccupied[channel-1] = true;
		theInstance.channelsPaused[channel-1] = false;
		theInstance.channels[channel-1].Play();
	}
	
	/// <summary> Plays whatever AudioClip is assigned to the given channel. 
	/// Call SetChannelAudioClip() if no AudioClip was assigned. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	public static void Play(int channel)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		if(theInstance.channels[channel-1].clip == null)
		{
			Debug.LogError("AudioMixer.Play: Given channel does not have a AudioClip set. Use SetChannelAudioClip() first.");
			return;
		}
		
		theInstance.channelsOccupied[channel-1] = true;
		theInstance.channelsPaused[channel-1] = false;
		theInstance.channels[channel-1].Play();
	}
	
	/// <summary> Plays audio from every channel. Will ignore channels where the AudioClip is not set. 
	/// Use SetChannelAudioClip() to set an AudioClip to a specific channel. </summary>
	public static void PlayAll()
	{
		FindAMObject();
		
		for(int i=1; i <= NUM_OF_CHANNELS; i++)
		{
			if(theInstance.channels[i-1].clip == null)
			{
				//Debug.Log("AudioMixer.PlayAll: Channel " + (i+1).ToString() + " does not have an assigned AudioClip!");
				continue;
			}
			Play(i);
		}
	}
	
	/// <summary> Plays audio from every channel with the given audio type. Will ignore channels where the AudioClip is not set. </summary>
	/// <param name='audioType'> Used to label the audio to a specific type. Choose from Sound, Music, Jungle, and Voice. </param>
	public static void PlayAllOfType(AudioMixerChannelTypes audioType)
	{
		FindAMObject();
		
		for(int i=1; i <= NUM_OF_CHANNELS; i++)
		{
			if(theInstance.channelsAudioType[i-1] == audioType)
			{
				if(theInstance.channels[i-1].clip == null)
				{
					//Debug.Log("AudioMixer.PlayAllOfType: Channel " + (i+1).ToString() + " does not have an assigned AudioClip!");
					continue;
				}
				Play(i);
			}
		}
	}
	
	/// <summary> Mutes the given channel. To stop muting, use Unmute(). </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	public static void Mute(int channel)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		theInstance.channels[channel-1].mute = true;
	}
	
	/// <summary> Mutes every channel. To stop muting every channel, use UnmuteAll(). </summary>
	public static void MuteAll()
	{
		FindAMObject();
		
		for(int i=1; i <= NUM_OF_CHANNELS; i++)
		{
			Mute(i);
		}
	}
	
	/// <summary> Mutes only the channels assigned the given audio type. To stop muting, use UnmuteAllOfType(). </summary>
	/// <param name='audioType'> Used to label the audio to a specific type. Choose from Sound, Music, Jungle, and Voice. </param>
	public static void MuteAllOfType(AudioMixerChannelTypes audioType)
	{
		FindAMObject();
		
		for(int i=1; i <= NUM_OF_CHANNELS; i++)
		{
			if(theInstance.channelsAudioType[i-1] == audioType)
			{
				Mute(i);
			}
		}
	}
	
	/// <summary> Stops muting the given channel. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	public static void Unmute(int channel)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		theInstance.channels[channel-1].mute = false;
	}
	
	/// <summary> Stops muting every channel. </summary>
	public static void UnmuteAll()
	{
		FindAMObject();
		
		for(int i=1; i <= NUM_OF_CHANNELS; i++)
		{
			Unmute(i);
		}
	}
	
	/// <summary> Stops muting every channel assigned the given audio type. </summary>
	/// <param name='audioType'> Used to label the audio to a specific type. Choose from Sound, Music, Jungle, and Voice. </param>
	public static void UnmuteAllOfType(AudioMixerChannelTypes audioType)
	{
		FindAMObject();
		
		for(int i=1; i <= NUM_OF_CHANNELS; i++)
		{
			if(theInstance.channelsAudioType[i-1] == audioType)
			{
				Unmute(i);
			}
		}
	}
	
	/// <summary> Pauses the given channel. To unpause, use Play(). </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	public static void Pause(int channel)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		if(theInstance.channels[channel-1].ignoreListenerPause)
		{
			Debug.Log("Channel " + channel.ToString() + " ignored the call to pause because it was flagged to ignore pause calls.");
			return;
		}
		
		theInstance.channelsOccupied[channel-1] = true;
		theInstance.channelsPaused[channel-1] = true;
		theInstance.channels[channel-1].Pause();
	}
	
	/// <summary> Pauses all channels with an assigned AudioClip. To unpause all channels, use PlayAll(). </summary>
	public static void PauseAll()
	{
		FindAMObject();
		
		for(int i=1; i <= NUM_OF_CHANNELS; i++)
		{
			if(theInstance.channels[i-1].clip == null)
			{
				//Debug.Log("AudioMixer.PauseAll: Channel " + (i+1).ToString() + " does not have an assigned AudioClip!");
				continue;
			}
			Pause(i);
		}
	}
	
	/// <summary> Pauses every channel assigned the given audio type. Ignores any channel without an assigned AudioClip.
	/// To unpause all channels of the given audio type, use PlayAllOfType(). </summary>
	/// <param name='audioType'> Used to label the audio to a specific type. Choose from Sound, Music, Jungle, and Voice. </param>
	public static void PauseAllOfType(AudioMixerChannelTypes audioType)
	{
		FindAMObject();
		
		for(int i=1; i <= NUM_OF_CHANNELS; i++)
		{
			if(theInstance.channelsAudioType[i-1] == audioType)
			{
				if(theInstance.channels[i-1].clip == null)
				{
					//Debug.Log("AudioMixer.PauseAllOfType: Channel " + (i+1).ToString() + " does not have an assigned AudioClip!");
					continue;
				}
				Pause(i);
			}
		}
	}
	
	/// <summary> Stops the audio playback if the given channel. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	public static void Stop(int channel)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		theInstance.channelsOccupied[channel-1] = false;
		theInstance.channelsPaused[channel-1] = false;
		theInstance.channels[channel-1].Stop();
	}
	
	/// <summary> Stops the audio playback of all channels. </summary>
	public static void StopAll()
	{
		FindAMObject();
		
		for(int i=1; i <= NUM_OF_CHANNELS; i++)
		{
			/*
			if(theInstance.channels[i-1].clip == null)
			{
				//Debug.Log("AudioMixer.StopAll: Channel " + (i+1).ToString() + " does not have an assigned AudioClip!");
				continue;
			}
			*/
			Stop(i);
		}
	}
	
	/// <summary> Stops the audio playback in the channels assigned the given audio type. </summary>
	/// <param name='audioType'> Used to label the audio to a specific type. Choose from Sound, Music, Jungle, and Voice. </param>
	public static void StopAllOfType(AudioMixerChannelTypes audioType)
	{
		FindAMObject();
		
		for(int i=1; i <= NUM_OF_CHANNELS; i++)
		{
			if(theInstance.channelsAudioType[i-1] == audioType)
			{
				/*
				if(theInstance.channels[i-1].clip == null)
				{
					//Debug.Log("AudioMixer.StopAllOfType: Channel " + (i+1).ToString() + " does not have an assigned AudioClip!");
					continue;
				}
				*/
				Stop(i);
			}
		}
	}
	
	/// <summary> Sets the AudioClip to the given channel. This does not play a sound. To do that, use Play(). </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	/// <param name='soundClip'> The audio file itself. Handled in Unity as an AudioClip. </param>
	public static void SetChannelAudioClip(int channel, AudioClip soundClip)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		if(soundClip == null)
		{
			Debug.LogWarning("AudioMixer.SetChannelAudioClip: Given sound clip is null.");
			return;
		}
		
		theInstance.channels[channel-1].clip = soundClip;
	}
	
	/// <summary> Sets the audio type label of the channel. Useful for dedicating a channel to music, for example. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	/// <param name='audioType'> Used to label the audio to a specific type. Choose from Sound, Music, Jungle, and Voice. </param>
	public static void SetChannelAudioType(int channel, AudioMixerChannelTypes audioType)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		theInstance.channelsAudioType[channel-1] = audioType;
	}
	
	/// <summary> Adjusts the volume of the given channel. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	/// <param name='volume'> The volume level. Should be between 0.0f and 1.0f. 1.0f is the max volume. </param>
	public static void SetChannelVolume(int channel, float volume)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		// Between 0.0f and 1.0f
		if(volume < 0.0f)
		{
			Debug.LogWarning("AudioMixer.SetChannelVolume: Given volume too small. Setting to 0.0f.");
			volume = 0.0f;
		}
		
		if(volume > 1.0f)
		{
			Debug.LogWarning("AudioMixer.SetChannelVolume: Given volume too big. Setting to 1.0f.");
			volume = 1.0f;
		}
		
		theInstance.channels[channel-1].volume = volume;
	}
	
	/// <summary> Adjusts the pitch scale of the given channel. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	/// <param name='pitch'> The pitch scale. Should be greater than 0.0f. A pitch of 1.0f is the normal pitch level. </param>
	public static void SetChannelPitch(int channel, float pitch)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		// Greater than 0.0f
		if(pitch < 0.0f)
		{
			Debug.LogWarning("AudioMixer.SetChannelPitch: Given pitch too small. Setting to 0.0f.");
			pitch = 0.0f;
		}
		
		theInstance.channels[channel-1].pitch = pitch;
	}
	
	/// <summary> Flags a channel as looping playback or single playback. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	/// <param name='shouldChannelLoop'> The channel will loop if true, or play once if false. </param>
	public static void SetChannelLooping(int channel, bool shouldChannelLoop)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		theInstance.channels[channel-1].loop = shouldChannelLoop;
	}
	
	/// <summary> Sets a channel's playback priority. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	/// <param name='priority'> The priority level. Should be between 0 and 255. 0 is highest priority. </param>
	public static void SetChannelPriority(int channel, int priority)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		// Between 0 and 255
		if(priority < 0)
		{
			Debug.LogWarning("AudioMixer.SetChannelPriority: Priority cannot be less than 0.");
			priority = 0;
		}
		if(priority > 255)
		{
			Debug.LogWarning("AudioMixer.SetChannelPriority: Priority cannot be more than 255.");
			priority = 255;
		}
		
		theInstance.channels[channel-1].priority = priority;
	}
	
	/// <summary> Sets the 2D panning of the given channel. Only works for mono or stereo audio files. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	/// <param name='pan'> The panning level. Should be between -1.0f and 1.0f. -1 if left, 1 if right, and 0 is centered. </param>
	public static void SetChannelPan2D(int channel, float pan)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		// Between -1.0f and 1.0f
		if(pan < -1.0f)
		{
			Debug.LogWarning("AudioMixer.SetChannelPan2D: Given pan has to be between -1.0f and 1.0f");
			pan = -1.0f;
		}
		if(pan > 1.0f)
		{
			Debug.LogWarning("AudioMixer.SetChannelPan2D: Given pan has to be between -1.0f and 1.0f");
			pan = 1.0f;
		}
		
		theInstance.channels[channel-1].pan = pan;
	}
	
	/// <summary> Flags a channel whether to ignore calls to pause or not. 
	/// Calling any pause function on this channel will be ignored if set to true. 
	/// Refer to AudioSource.ignoreListernerPause in Unity's documentation. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	/// <param name='ignorePause'> The channel will ignore any calls to pause if true. </param>
	public static void ShouldChannelIgnorePause(int channel, bool ignorePause)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		theInstance.channels[channel-1].ignoreListenerPause = ignorePause;
	}
	
	/// <summary> Flags a channel whether to ignore volume adjustment or not.
    /// Calling SetChannelVolume() on the given channel will be ignored if this is set to true. 
    /// Refer to AudioSource.ignoreListenerVolume in Unity's documentation. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	/// <param name='ignoreVolume'> The channel will ignore any volume adjustments if true. </param>
	public static void ShouldChannelIgnoreVolume(int channel, bool ignoreVolume)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return;
		
		theInstance.channels[channel-1].ignoreListenerVolume = ignoreVolume;
	}
	
	/// <summary> Returns true if the given channel is paused. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	public static bool IsChannelPaused(int channel)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return false;
		
		return theInstance.channelsPaused[channel-1];
	}
	
	/// <summary> Returns true if the given channel is muted. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	public static bool IsChannelMuted(int channel)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return false;
		
		return theInstance.channels[channel-1].mute;
	}
	
	/// <summary> Returns true if the given channel is playing a sound. Returns false if paused or stopped. </summary>
	/// <param name='channel'> The AudioMixer channel. Should be a number between 1 and NUM_OF_CHANNELS </param>
	public static bool IsChannelPlaying(int channel)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel) ) return false;
		
		return theInstance.channels[channel-1].isPlaying;
	}
	
}
