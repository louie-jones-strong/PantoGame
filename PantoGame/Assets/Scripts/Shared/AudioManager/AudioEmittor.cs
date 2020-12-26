using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioEmittor : MonoBehaviour
{
	[SerializeField] string EventPath;
	public void Play()
	{
		AudioManger.PlayEvent(EventPath, transform);
	}
}