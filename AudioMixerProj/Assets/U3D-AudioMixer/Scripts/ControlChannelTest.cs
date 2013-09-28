using UnityEngine;
using System.Collections;

public class ControlChannelTest : MonoBehaviour 
{
	// Note this enum is for testing purposes and thus is NOT part of AudioMixer
	public enum AudioMixerControls
	{
		Play,
		Pause,
		Mute,
		Unmute,
		Stop
	}
	
	public AudioMixerControls audioControl;
	public bool controllAllChannels = false;
	public int channelToControl = 0;
	public bool controlAllAudioTypes = false;
	public AudioMixerChannelTypes audioTypeToControl;
	public KeyCode keyToPress;
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
			PerformAudioControl(audioControl);
			
			renderer.material = matPressed;
		}
		
		if(Input.GetKeyUp(keyToPress))
		{
			renderer.material = matDefault;
		}
	}
	
	private void PerformAudioControl(AudioMixerControls audioControlType)
	{
		switch(audioControlType)
		{
			case AudioMixerControls.Play: PerformPlay(); break;
			case AudioMixerControls.Pause: PerformPause(); break;
			case AudioMixerControls.Mute: PerformMute(); break;
			case AudioMixerControls.Unmute: PerformUnmute(); break;
			case AudioMixerControls.Stop: PerformStop(); break;
		}
	}
	
	private void PerformPlay()
	{
		if(controllAllChannels)
		{
			if(controlAllAudioTypes)
			{
				AudioMixer.PlayAll();
			}
			else
			{
				AudioMixer.PlayAllOfType(audioTypeToControl);
			}
		}
		else
		{
			AudioMixer.Play(channelToControl);
		}
	}
	
	private void PerformPause()
	{
		if(controllAllChannels)
		{
			if(controlAllAudioTypes)
			{
				AudioMixer.PauseAll();
			}
			else
			{
				AudioMixer.PauseAllOfType(audioTypeToControl);
			}
		}
		else
		{
			AudioMixer.Pause(channelToControl);
		}
	}
	
	private void PerformMute()
	{
		if(controllAllChannels)
		{
			if(controlAllAudioTypes)
			{
				AudioMixer.MuteAll();
			}
			else
			{
				AudioMixer.MuteAllOfType(audioTypeToControl);
			}
		}
		else
		{
			AudioMixer.Mute(channelToControl);
		}
	}
	
	private void PerformUnmute()
	{
		if(controllAllChannels)
		{
			if(controlAllAudioTypes)
			{
				AudioMixer.UnmuteAll();
			}
			else
			{
				AudioMixer.UnmuteAllOfType(audioTypeToControl);
			}
		}
		else
		{
			AudioMixer.Unmute(channelToControl);
		}
	}
	
	private void PerformStop()
	{
		if(controllAllChannels)
		{
			if(controlAllAudioTypes)
			{
				AudioMixer.StopAll();
			}
			else
			{
				AudioMixer.StopAllOfType(audioTypeToControl);
			}
		}
		else
		{
			AudioMixer.Stop(channelToControl);
		}
	}
}
