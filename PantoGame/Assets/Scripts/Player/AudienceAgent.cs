using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AudienceAgent : Agent
{
	public AudienceProfileData ProfileData;
	public Chair SetSeat;
	float BladderTime = 0;
	eIntent AudienceIntent;

	enum eIntent
	{
		None,
		Sit,
		Toilet,
		Clapping,
		RandomWalk
	}
	
	protected override void Start()
	{
		CameraController.AddTarget(transform, weighting:Settings.AudienceCamWeighting);
		base.Start();
	}
	
	public void Setup(Chair chair)
	{
		SetSeat = chair;
		BladderTime = ProfileData.BladderStartFill;
	}

	void Update()
    {
		BladderTime += Time.deltaTime;
		var intent = GetIntent();

		Vector3 target = transform.position;
		switch (intent)
		{
			case eIntent.Sit:
			{
				target = SetSeat.transform.position;
				break;
			}
			case eIntent.Toilet:
			{
				target = Theatre.Instance.Toilet.position;
				break;
			}
		}

		if (AudienceIntent != intent)
		{
			NavMeshAgent.SetDestination(target);
			AudienceIntent = intent;
		}


		if (AudienceIntent == eIntent.Toilet && (target-transform.position).magnitude <= 1)
		{
			BladderTime = 0;
		}
		PlayerAnimator.SetBool("Clapping", AudienceIntent == eIntent.Clapping);


		UpdateVisuals();
		base.Update();
    }

	eIntent GetIntent()
	{
		var currentScene = Theatre.Instance.CurrentScript.CurrentScene;
		if (currentScene == null)
		{
			return eIntent.Sit;
		}
		
		foreach (var task in currentScene.Tasks)
		{
			if (task.State == eTaskState.CanStart || 
				task.State == eTaskState.InProgress)
			{
				if (task is AudienceClapTask)
				{
					return eIntent.Clapping;
				}
			}
		}


		if (ProfileData.BladderTimeToFill <= BladderTime &&
			Settings.AudienceUseToilet)
		{
			return eIntent.Toilet;
		}
		return eIntent.Sit;
	}
}
