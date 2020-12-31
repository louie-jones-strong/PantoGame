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
	float TimeHitByPlayer;
	float TimeSeeingPlayerOnStage;
	bool CollidingWithPlayer;

	enum eIntent
	{
		None,
		Sit,
		Toilet,
		Clapping,
		StandInLobby,
		RandomWalk
	}
	
	protected override void OnTriggerEnter(Collider collider)
	{
		var playerAgent = collider.GetComponent<PlayerAgent>();
		if (playerAgent != null)
		{
			CollidingWithPlayer = true;
		}
		base.OnTriggerEnter(collider);
	}

	protected override void OnTriggerExit(Collider collider)
	{
		var playerAgent = collider.GetComponent<PlayerAgent>();
		if (playerAgent != null)
		{
			CollidingWithPlayer = false;
		}
		base.OnTriggerExit(collider);
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

		if (CollidingWithPlayer)
		{
			TimeHitByPlayer += Time.deltaTime;
		}

		CheckSeeingPlayerOnStage();

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

		UpdateRatingVisual();
		UpdateVisuals();
		base.Update();
	}

	void UpdateRatingVisual()
	{
		var currentScript = Theatre.Instance.CurrentScript;

		var rating = currentScript.Rating;
		rating -= TimeHitByPlayer;
		rating -= TimeSeeingPlayerOnStage;

		if (rating <= -0.5f)
		{
			SetColour(Color.red);
		}
		else if (rating >= 0.5f)
		{
			SetColour(Color.green);
		}
		else
		{
			SetColour(Color.yellow);
		}
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

	void CheckSeeingPlayerOnStage()
	{
		var startPos = EyesPoint.position;

		foreach (var kvp in PlayerManger.Players)
		{
			var player = kvp.Value;
			if (player == null)
			{
				continue;
			}

			//check if player on stage
			if (!Theatre.Instance.IsPointOnStage(player.transform.position))
			{
				continue;
			}

			var delta = player.BodyCenter.position - startPos;
			Physics.Raycast(startPos, delta, out RaycastHit hitInfo, delta.magnitude);
			
			bool canSeePlayer = hitInfo.transform == player.transform;
			if (canSeePlayer)
			{
				TimeSeeingPlayerOnStage += Time.deltaTime;
			}

			Debug.DrawRay(startPos, delta, canSeePlayer ? Color.green : Color.red);
		}
	}
}
