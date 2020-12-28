using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorAgent : Agent
{
	ActorTask CurrentTask;
	ActorTalkTask TalkingTask;
	
	protected override void Start()
	{
		CameraController.AddTarget(transform, weighting:Settings.ActorCamWeighting);
		SetColour(Color.green);
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

		if (TalkingTask == null || TalkingTask.State == eTaskState.Completed)
		{
			TalkingTask = GetTalkTask();
		}
		SpeechBubble.SetTalking(TalkingTask != null);
		
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
				NavMeshAgent.speed = actorTask.ActorSpeed;
				return actorTask;
			}
		}
		return null;
	}

	ActorTalkTask GetTalkTask()
	{
		if (Theatre.Instance.CurrentScript?.CurrentScene == null)
		{
			return null;
		}

		var currentScene = Theatre.Instance.CurrentScript.CurrentScene;
		foreach (var task in currentScene.Tasks)
		{
			var talkTask = task as ActorTalkTask;

			if (talkTask != null &&
				talkTask.State != eTaskState.Completed &&
				talkTask.StartConditionsMet() &&
				talkTask.Actor == this)
			{
				return talkTask;
			}
		}
		return null;
	}
}
