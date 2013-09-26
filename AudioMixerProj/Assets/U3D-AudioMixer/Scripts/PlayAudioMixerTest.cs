using UnityEngine;
using System.Collections;

public class PlayAudioMixerTest : MonoBehaviour 
{
	public AudioClip audioToPlay;
	public KeyCode keyToPress;
	public int channelToPlay;
	public AudioMixerChannelTypes typeOfAudio;
	public bool loopChannel = false;
	public float channelPan = 0.0f;
	public float channelPitch = 1.0f;
	public int channelPriority = 128;
	public float channelVolume = 1.0f;
	public Material matDefault;
	public Material matPressed;
	
	// Use this for initialization
	void Start () 
	{
		renderer.material = matDefault;
		
		AudioMixer.ShouldChannelIgnorePause(channelToPlay, false);
		AudioMixer.ShouldChannelIgnoreVolume(channelToPlay, false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// These calls are here for convenient testing of the demo live
		AudioMixer.SetChannelLooping(channelToPlay, loopChannel);
		AudioMixer.SetChannelPan2D(channelToPlay, channelPan);
		AudioMixer.SetChannelPitch(channelToPlay, channelPitch);
		AudioMixer.SetChannelPriority(channelToPlay, channelPriority);
		AudioMixer.SetChannelVolume(channelToPlay, channelVolume);
		
		if(Input.GetKeyDown(keyToPress))
		{
			if(audioToPlay == null)
			{
				Debug.LogWarning("Test Key " + keyToPress.ToString() + " does not have an assigned AudioClip.");
			}
			
			AudioMixer.Play(1, audioToPlay, AudioMixerChannelTypes.Sound);
			renderer.material = matPressed;
		}
		
		if(Input.GetKeyUp(keyToPress))
		{
			renderer.material = matDefault;
		}
	}
}
