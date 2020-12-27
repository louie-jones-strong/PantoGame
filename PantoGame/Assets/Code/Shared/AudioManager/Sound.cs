using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;


[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/Sound")]
class Sound: ScriptableObject
{
	public string Name;
	public List<AudioClip> Clips;
	public eAudioBusType AudioBus;

	
	public AudioClip GetAudioClip()
	{
		return Clips[UnityEngine.Random.Range(0, Clips.Count)];
	}
}