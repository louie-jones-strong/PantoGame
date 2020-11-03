using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorAgent : Agent
{
	ActorTask CurrentTask;
	
	public void Setup()
	{
		CameraController.AddTarget(transform, weighting:Settings.ActorCamWeighting);
	}

	void Update()
    {
		UpdateVisuals();
    }
}
