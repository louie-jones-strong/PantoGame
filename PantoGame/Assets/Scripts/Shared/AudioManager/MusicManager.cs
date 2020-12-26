using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	const string MenuMusic = "MenuMusic";

	AudioSource CurrentSource;

	void Update()
	{
		if (CurrentSource == null ||
			!CurrentSource.isPlaying)
		{
			CurrentSource = AudioManger.PlayEvent(MenuMusic);
		}
	}
}