using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AudienceAgent : Agent
{
	public AudienceProfileData ProfileData;
	public Chair SetSeat;
	public float TimeSinceLastToilet {get; private set;}
	eIntent AudienceIntent;

	enum eIntent
	{
		None,
		Sit,
		Toilet,
		Clapping,
		StandInLobby,
		RandomWalk
	}
	
	protected override void Start()
	{
		CameraController.AddTarget(transform, weighting:Settings.AudienceCamWeighting);
		SetColour(Color.yellow);
		base.Start();
	}
	
	public void Setup(Chair chair)
	{
		SetSeat = chair;
	}

	protected override void Update()
	{
		TimeSinceLastToilet += Time.deltaTime;
		var intent = GetIntent();

		Vector3 target = transform.position;
		switch (intent)
		{
			case eIntent.StandInLobby:
			{
				target = Theatre.Instance.Lobby.position;
				break;
			}
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
			TimeSinceLastToilet = 0;
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
				if (task is AudienceStandInLobbyTask)
				{
					return eIntent.StandInLobby;
				}
				if (task is AudienceClapTask)
				{
					return eIntent.Clapping;
				}
				if (task is AudienceToiletTask &&
					TimeSinceLastToilet >= 10f)
				{
					return eIntent.Toilet;
				}
			}
		}
		return eIntent.Sit;
	}
}
