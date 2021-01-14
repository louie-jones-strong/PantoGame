using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AudienceAgent : Agent
{
	public AudienceProfileData ProfileData;
	[SerializeField] ColourCurve ColourCurve;
	public float TimeSinceLastToilet {get; private set;}

	Chair SetSeat;
	eAudienceIntent AudienceIntent;
	eAudienceIntent TargetIntent;
	float TargetIntentTime;

	bool CollidingWithPlayer;
	AudioSource CurrentAudioSource;
	
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
			ProfileData.TimeHitByPlayer += Time.deltaTime;
		}

		CheckSeeingPlayerOnStage();

		var intentChanged = GetIntent();

		if (intentChanged)
		{
			Vector3 target = transform.position;
			switch (AudienceIntent)
			{
				case eAudienceIntent.StandInLobby:
				{
					target = Theatre.Instance.Lobby.position;
					break;
				}
				case eAudienceIntent.WatchShow:
				{
					target = SetSeat.transform.position;
					break;
				}
				case eAudienceIntent.Toilet:
				{
					target = Theatre.Instance.Toilet.position;
					break;
				}
			}

			NavMeshAgent.SetDestination(target);
		}


		if (AudienceIntent == eAudienceIntent.Toilet && 
			(Theatre.Instance.Toilet.position-transform.position).magnitude <= 1)
		{
			TimeSinceLastToilet = 0;
		}
		PlayerAnimator.SetBool("Clapping", AudienceIntent == eAudienceIntent.Clapping);

		if (CurrentAudioSource == null || !CurrentAudioSource.isPlaying)
		{
			if (AudienceIntent == eAudienceIntent.Laughing)
			{
				CurrentAudioSource = AudioManger.PlayEvent("Laughing", transform);
			}
			if (AudienceIntent == eAudienceIntent.Clapping)
			{
				CurrentAudioSource = AudioManger.PlayEvent("Clap", transform);
			}
		}

		UpdateRatingVisual();
		UpdateVisuals();
		base.Update();
	}

	void UpdateRatingVisual()
	{
		var rating = ProfileData.GetRatingValue();

		var normalizedRating = (rating - 5) / 5;
		PlayerAnimator.SetFloat("Ratting", normalizedRating);

		SetColour(ColourCurve.GetColor(rating));
	}

	bool GetIntent()
	{
		var intent = eAudienceIntent.WatchShow;
		TargetIntentTime -= Time.deltaTime;

		var currentScene = Theatre.Instance.CurrentScript.CurrentScene;
		if (currentScene != null)
		{
			foreach (var task in currentScene.Tasks)
			{
				if (task.State == eTaskState.CanStart || 
					task.State == eTaskState.InProgress)
				{
					var intentTask = task as AudienceIntentTask;
					if (intentTask != null)
					{
						intent = intentTask.TargetIntent;
					}
				}
			}
		}

		if (intent != TargetIntent)
		{

			TargetIntent = intent;
			TargetIntentTime = ProfileData.GetTransitionTime(TargetIntent, 0f);
		}

		if (AudienceIntent == TargetIntent)
		{
			return false;
		}

		if (TargetIntentTime <= 0)
		{
			AudienceIntent = TargetIntent;
			return true;
		}
		
		return false;
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
				ProfileData.TimeSeeingPlayerOnStage += Time.deltaTime;
			}

			Debug.DrawRay(startPos, delta, canSeePlayer ? Color.green : Color.red);
		}
	}
}
