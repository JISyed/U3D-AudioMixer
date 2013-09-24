using UnityEngine;
using System.Collections;

public class AudioMixer : MonoBehaviour 
{
	public const int NUM_OF_CHANNELS = 8;
	private AudioSource[] channels;
	
	// Initilizer
	void Start () 
	{
		// Create the audio channels
		channels = new AudioSource[NUM_OF_CHANNELS];
		for(int i=0; i<NUM_OF_CHANNELS; i++)
		{
			channels[i] = gameObject.AddComponent<AudioSource>();
		}
	}
	
	// Update loop
	void Update () 
	{
		
	}
	
	// Used only to get a Gizmo icon in the editor
	void OnDrawGizmos(){}
}
