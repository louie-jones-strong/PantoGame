using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
	[SerializeField] int MaxStringLength = 5;
	[SerializeField] Animator Animator;
	[SerializeField] TextMesh Text;
	float TimeBetweenCharactors = 0.1f;
	float TimeBetweenClears = 0.2f;
	bool Talking = false;
	float TimeSinceLastCharactor = 0;

	string[] PossableText = {"!","£","$","%","&","*","@","#"};

	AudioSource CurrentClip;

	void Update()
	{
		if (!Talking)
		{
			return;
		}

		TimeSinceLastCharactor += Time.deltaTime;
		
		if (Text.text.Length >= MaxStringLength)
		{
			if (TimeSinceLastCharactor >= TimeBetweenClears)
			{
				Text.text = "";
			}
		}
		else
		{
			if (TimeSinceLastCharactor >= TimeBetweenCharactors)
			{
				Text.text += PossableText[Random.Range(0,PossableText.Length)];
				TimeSinceLastCharactor = 0;
			}
		}
	}

	public void SetTalking(bool value)
	{
		if (Talking == value)
		{
			return;
		}

		Talking = value;
		Animator.SetBool("Talking", value);
		TimeSinceLastCharactor = 0;

		if (Talking)
		{
			CurrentClip = AudioManger.PlayEvent("Talking", transform);
		}
		else if (CurrentClip != null)
		{
			CurrentClip.Stop();
		}
	}

}