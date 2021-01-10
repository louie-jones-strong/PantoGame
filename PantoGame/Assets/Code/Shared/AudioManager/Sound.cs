using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;


[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/Sound")]
class Sound: ScriptableObject
{
	public string Name;
	public eAudioBusType AudioBus;
	public bool LoopClip;
	public List<ClipAndVolume> Clips;
	
	[Serializable]
	public class ClipAndVolume
	{
		public AudioClip Clip;
		public float Volume = 0.5f;
	}

	
	public ClipAndVolume GetAudioClip()
	{
		return Clips[UnityEngine.Random.Range(0, Clips.Count)];
	}
}