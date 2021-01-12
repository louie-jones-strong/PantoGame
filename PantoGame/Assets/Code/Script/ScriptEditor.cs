#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Script))]
public class ScriptEditor : Editor
{
	Dictionary<int, bool> CurrentEditActions = new Dictionary<int, bool>();

	public override void OnInspectorGUI()
	{
		var targetScript = (Script)target;
		EditorUtility.SetDirty(target);

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Scene"))
		{
			var scene = new Scene();
			targetScript.Scenes.Add(scene);
		}
		EditorGUILayout.EndHorizontal();

		int loop = 0;
		while (loop < targetScript.Scenes.Count)
		{
			var scene = targetScript.Scenes[loop];
			
			EditorGUILayout.Space(10);

			EditorGUILayout.BeginHorizontal();
			
			if (!CurrentEditActions.ContainsKey(loop))
			{
				CurrentEditActions[loop] = false;
			}
			CurrentEditActions[loop] = EditorGUILayout.BeginFoldoutHeaderGroup(CurrentEditActions[loop], $"SceneName:");

			scene.SceneName = GUILayout.TextField(scene.SceneName);

			if (GUILayout.Button("Sort"))
			{
				scene.SortTasks();
			}

			GUI.backgroundColor = Color.red;
			if (GUILayout.Button("Remove Scene"))
			{
				targetScript.Scenes.RemoveAt(loop);
				break;
			}
			GUI.backgroundColor = Color.white;

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndFoldoutHeaderGroup();

			if(CurrentEditActions[loop])
			{
				EditorGUILayout.BeginVertical();

				scene.OnDrawScene();
				EditorGUILayout.EndVertical();
			}
			
			loop += 1;
		}

		EditorGUILayout.Space();
		if (Application.isPlaying)
		{
			DrawDebugSettings(targetScript);
			EditorGUILayout.Space();
			DrawAudienceReviews();
		}
	}

	void DrawDebugSettings(Script script)
	{
		EditorGUILayout.BeginHorizontal();

		GUILayout.Label($"Rating: {script.Rating}");
		GUILayout.Label($"Finished: {script.Finished}");


		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();

		GUILayout.Label("Scene");

		if (GUILayout.Button("previous Scene") && script.SceneIndex > 0)
		{
			script.CurrentScene.SetState(eSceneState.NotStarted);
			foreach (var task in script.CurrentScene.Tasks)
			{
				task.SetState(eTaskState.CannotStart, force:true);
			}
			script.SceneIndex -= 1;

			script.CurrentScene.SetState(eSceneState.NotStarted);
			foreach (var task in script.CurrentScene.Tasks)
			{
				task.SetState(eTaskState.CannotStart, force:true);
			}
		}

		if (GUILayout.Button("Reset Scene"))
		{
			script.CurrentScene.SetState(eSceneState.NotStarted);
			foreach (var task in script.CurrentScene.Tasks)
			{
				task.SetState(eTaskState.CannotStart, force:true);
			}
		}

		if (GUILayout.Button("Skip Scene"))
		{
			foreach (var task in script.CurrentScene.Tasks)
			{
				task.SetState(eTaskState.Completed, force:true);
			}
		}

		EditorGUILayout.EndHorizontal();

	}

	void DrawAudienceReviews()
	{
		if (Theatre.Instance?.AudienceAgents == null)
		{
			return;
		}

		EditorGUILayout.BeginVertical();

		var audienceAgents = Theatre.Instance.AudienceAgents;
		for (int index = 0; index < audienceAgents.Count; index++)
		{
			var audienceAgent = audienceAgents[index];

			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label($"audience: {index}");
			EditorGUILayout.ObjectField(audienceAgent, typeof(AudienceAgent), true);
			EditorGUILayout.ColorField(audienceAgent.Colour);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			
			var rating = audienceAgent.ProfileData.GetRatingValue();
			var review = audienceAgent.ProfileData.GetReviewText(rating);

			GUILayout.Label($"Rating: {rating} Review: {review}");

			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
		}
		EditorGUILayout.EndVertical();
	}
}
#endif