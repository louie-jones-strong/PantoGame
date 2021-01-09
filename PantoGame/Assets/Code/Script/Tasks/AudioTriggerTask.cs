using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class AudioTriggerTask : Task
{
	public string AudioEvent;
	AudioSource Source;

	public override bool EndConditionsMet()
	{
		return Source != null && base.EndConditionsMet();
	}

	public override void Update()
	{
		if (Source == null && State == eTaskState.CanStart)
		{
			Logger.Log($"AudioTriggerTask play audio: {AudioEvent}");
			Source = AudioManger.PlayEvent(AudioEvent);
		}
		base.Update();
	}

#if UNITY_EDITOR
	public override void DrawTask(Scene scene)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("AudioEvent");
		AudioEvent = EditorGUILayout.TextField(AudioEvent);
		EditorGUILayout.EndHorizontal();

		if(!AudioManger.IsPathValid(AudioEvent))
		{
			EditorGUILayout.HelpBox($"AudioEvent path: \"{AudioEvent}\" is not valid", MessageType.Error);
		}

		base.DrawTask(scene);
	}
#endif
}