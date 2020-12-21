using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorAgent : Agent
{
	ActorTask CurrentTask;
	
	protected override void Start()
	{
		CameraController.AddTarget(transform, weighting:Settings.ActorCamWeighting);
		base.Start();
	}

	protected override void Update()
	{
		if (CurrentTask == null || CurrentTask.State == eTaskState.Completed)
		{
			CurrentTask = GetTarget();
			if (CurrentTask != null)
			{
				NavMeshAgent.SetDestination(CurrentTask.Target.position);
			}
		}
		
		UpdateVisuals();
		base.Update();
	}

	ActorTask GetTarget()
	{
		if (Theatre.Instance.CurrentScript?.CurrentScene == null)
		{
			return null;
		}

		var currentScene = Theatre.Instance.CurrentScript.CurrentScene;
		foreach (var task in currentScene.Tasks)
		{
			var actorTask = task as ActorTask;

			if (actorTask != null &&
				actorTask.State != eTaskState.Completed &&
				actorTask.StartConditionsMet() &&
				actorTask.Actor == this)
			{
				return actorTask;
			}
		}
		return null;
	}
}
