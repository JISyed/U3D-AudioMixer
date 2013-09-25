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
	
	// Initilizer
	void Start () 
	{
		// Assign the instance
		theInstance = this;
		
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
		
	}
	
	// Used only to get a Gizmo icon in the editor
	void OnDrawGizmos(){}
	
	// Destroy event
	void OnDestroy()
	{
		// Unassign the instance
		theInstance = null;
		
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
	
	// ------- Audio calls ------------
	
	public static void Play(AudioClip soundClip)
	{
		// WIP
	}
	
	public static void Play(AudioClip soundClip, bool willLoop)
	{
		// WIP
	}
	
	public static void PlayInChannel(AudioClip soundClip, int channel)
	{
		if(! theInstance.ChannelIsValid(channel))
		{
			return;
		}
	}
	
	public static void PlayInChannel(AudioClip soundClip, int channel, bool willLoop)
	{
		if(! theInstance.ChannelIsValid(channel))
		{
			return;
		}
	}
}
