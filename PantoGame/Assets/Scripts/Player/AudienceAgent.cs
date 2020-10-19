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
		RandomWalk
	}
	
	public void Setup(Chair chair)
	{
		SetSeat = chair;
		CameraController.AddTarget(transform, weighting:Settings.AudienceCamWeighting);
		BladderTime = ProfileData.BladderStartFill;
	}

	void Update()
    {
		BladderTime += Time.deltaTime;
		var intent = GetIntent();

		Vector3 target = Vector3.zero;
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
			default:
			{
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
		UpdateVisuals();
    }

	eIntent GetIntent()
	{
		if (ProfileData.BladderTimeToFill <= BladderTime)
		{
			return eIntent.Toilet;
		}
		return eIntent.Sit;
	}
}
