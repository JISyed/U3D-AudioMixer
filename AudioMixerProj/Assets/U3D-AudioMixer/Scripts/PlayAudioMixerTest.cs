using UnityEngine;
using System.Collections;

public class PlayAudioMixerTest : MonoBehaviour 
{
	public AudioClip audioToPlay;
	public KeyCode keyToPress;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(keyToPress))	
		{
			AudioMixer.PlayInChannel(audioToPlay, 1, false);
		}
	}
}
