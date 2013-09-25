using UnityEngine;
using System.Collections;

public class AudioMixer : MonoBehaviour 
{
	public enum ChannelTypes
	{
		Music,
		Jingle,
		Sound,
		Voice
	}
	
	public const int NUM_OF_CHANNELS = 8;
	
	private AudioSource[] channels;
	private bool[] channelsOccupied;
	private static AudioMixer theInstance;
	private static bool instanceFound = false;
	
	// Initilizer
	void Start () 
	{
		// Create the audio channels
		channels = new AudioSource[NUM_OF_CHANNELS];
		channelsOccupied = new bool[NUM_OF_CHANNELS];
		for(int i=0; i<NUM_OF_CHANNELS; i++)
		{
			channels[i] = gameObject.AddComponent<AudioSource>();
			channels[i].playOnAwake = false;
			channelsOccupied[i] = false;
		}
	}
	
	// Update loop
	void Update () 
	{
		for(int i=0; i<NUM_OF_CHANNELS; i++)
		{
			
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
		
		if(!instanceFound)
		{
			Debug.Log("Finding Mixer");
			foundMixerObject = GameObject.Find("AudioMixerObject");
			if(foundMixerObject == null)
			{
				Debug.Log("Mixer not found. Making one.");
				// Change this path if you move AudioMixerObject.prefab somewhere else.
				foundMixerObject = Resources.Load("/U3D-AudioMixer/Prefabs/AudioMixerObject") as GameObject;
			}
			else
			{
				Debug.Log("Mixer Found!");
			}
			AudioMixer mixerComponent = foundMixerObject.GetComponent<AudioMixer>();
			AudioMixer.theInstance = mixerComponent;
			instanceFound = true;
		}
		else
		{
			Debug.Log("Mixer already found");
		}
	}
	
	// ------- Audio calls ------------
	
	public static void Play(AudioClip soundClip)
	{
		FindAMObject();
		// WIP
	}
	
	public static void Play(AudioClip soundClip, bool willLoop)
	{
		FindAMObject();
		// WIP
	}
	
	public static void PlayInChannel(AudioClip soundClip, int channel)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel))
		{
			return;
		}
		
		theInstance.channels[channel-1].clip = soundClip;
		theInstance.channelsOccupied[channel-1] = true;
		theInstance.channels[channel-1].Play();
	}
	
	public static void PlayInChannel(AudioClip soundClip, int channel, bool willLoop)
	{
		FindAMObject();
		if(! theInstance.ChannelIsValid(channel))
		{
			return;
		}
		
		theInstance.channels[channel-1].loop = willLoop;
		
		theInstance.channels[channel-1].clip = soundClip;
		theInstance.channelsOccupied[channel-1] = true;
		theInstance.channels[channel-1].Play();
	}
}
