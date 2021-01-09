using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	const string GameMusic = "GameMusic";
	AudioSource CurrentGameSource;

	const string MenuMusic = "MenuMusic";
	AudioSource CurrentMenuSource;

	void Update()
	{
		bool inMenu = true;
		if (Theatre.Instance != null &&
			Theatre.Instance.State != eTheatreState.ShowOver)
		{
			inMenu = false;
		}

		if (inMenu)
		{
			if (CurrentGameSource != null && CurrentGameSource.isPlaying)
			{
				CurrentGameSource.Stop();
			}

			if (CurrentMenuSource == null ||
			!CurrentMenuSource.isPlaying)
			{
				CurrentMenuSource = AudioManger.PlayEvent(MenuMusic);
			}
		}
		else
		{
			if (CurrentMenuSource != null && CurrentMenuSource.isPlaying)
			{
				CurrentMenuSource.Stop();
			}
			
			if (CurrentGameSource == null ||
			!CurrentGameSource.isPlaying)
			{
				CurrentGameSource = AudioManger.PlayEvent(GameMusic);
			}
		}
	}
}