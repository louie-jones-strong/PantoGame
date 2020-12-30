using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class AudienceStandInLobbyTask : Task
{
	public override bool EndConditionsMet()
	{
		return base.EndConditionsMet();
	}

#if UNITY_EDITOR
	public override void DrawTask()
	{
		base.DrawTask();
	}
#endif
}