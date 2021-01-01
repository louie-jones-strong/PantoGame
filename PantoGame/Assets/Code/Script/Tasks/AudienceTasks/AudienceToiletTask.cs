using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class AudienceToiletTask : Task
{
	public override bool EndConditionsMet()
	{
		return GetProgress() >= 1f && base.EndConditionsMet();
	}

	public override float GetProgress()
	{
		var audience = Theatre.Instance.AudienceAgents;
		var numFinished = 0;
		foreach (var agent in audience)
		{
			if (agent.TimeSinceLastToilet < 10f)
			{
				numFinished += 1;
			}
		}
		return numFinished / audience.Count;
	}

#if UNITY_EDITOR
	public override void DrawTask(Scene scene)
	{

		base.DrawTask(scene);
	}
#endif
}