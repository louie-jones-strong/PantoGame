using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;


[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/Sound")]
class Sound: ScriptableObject
{
	public string Name;
	public eAudioBusType AudioBus;
	public bool LoopClip;
	public List<AudioClip> Clips;

	
	public AudioClip GetAudioClip()
	{
		return Clips[Random.Range(0, Clips.Count)];
	}
}