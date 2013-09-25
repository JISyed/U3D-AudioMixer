using UnityEngine;
using System.Collections;

public class PlayAudioMixerTest : MonoBehaviour 
{
	public AudioClip audioToPlay;
	public KeyCode keyToPress;
	public int channelToPlay;
	public AudioMixerChannelTypes typeOfAudio;
	public Material matDefault;
	public Material matPressed;
	
	// Use this for initialization
	void Start () 
	{
		renderer.material = matDefault;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(keyToPress))	
		{
			AudioMixer.Play(audioToPlay, 1, AudioMixerChannelTypes.Sound, false, 1.0f, 1.0f, 128);
			renderer.material = matPressed;
		}
		
		if(Input.GetKeyUp(keyToPress))
		{
			renderer.material = matDefault;
		}
	}
}
