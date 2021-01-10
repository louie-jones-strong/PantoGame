#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(Sound))]
public class SoundEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var targetSound = (Sound)target;
		EditorUtility.SetDirty(targetSound);

		//play button
		GUI.backgroundColor = Application.isPlaying ? Color.green : Color.grey;

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Play Sound"))
		{
			AudioManger.PlayEvent(targetSound.Name);
		}
		EditorGUILayout.EndHorizontal();

		GUI.backgroundColor = Color.white;

		EditorGUILayout.Space(20);

		DrawDefaultInspector();

		EditorGUILayout.Space(20);

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Add");
		var clip = (AudioClip)EditorGUILayout.ObjectField(null, typeof(AudioClip), false);
		if (clip != null)
		{
			var soundClip = new Sound.ClipAndVolume();
			soundClip.Clip = clip;
			targetSound.Clips.Add(soundClip);
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space(10);

		//draw clip list
		DrawClipList(targetSound);

		//update name
		if (targetSound.name != targetSound.Name)
		{
			if (string.IsNullOrEmpty(targetSound.Name))
			{
				targetSound.Name = targetSound.name;
			}
			else
			{
				string path = AssetDatabase.GetAssetPath(target);
				AssetDatabase.RenameAsset(path, targetSound.Name);
			}
		}
	}

	void DrawClipList(Sound targetSound)
	{
		int index = 0;
		while (index < targetSound.Clips.Count)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Separator();

			var volumeClip = targetSound.Clips[index];

			GUI.backgroundColor = Application.isPlaying ? Color.green : Color.grey;
			if (GUILayout.Button($"Play Clip {index}") && Application.isPlaying)
			{
				AudioSource.PlayClipAtPoint(volumeClip.Clip, Vector3.zero, volumeClip.Volume);
			}
			GUI.backgroundColor = Color.white;

			volumeClip.Clip = (AudioClip)EditorGUILayout.ObjectField(volumeClip.Clip, typeof(AudioClip), false);
			
			GUILayout.Label("Volume");
			volumeClip.Volume = EditorGUILayout.Slider(volumeClip.Volume, 0f, 1f);

			GUI.backgroundColor = Color.red;
			if (GUILayout.Button($"Remove"))
			{
				targetSound.Clips.RemoveAt(index);
				continue;
			}
			GUI.backgroundColor = Color.white;

			index += 1;

			EditorGUILayout.EndHorizontal();
		}
	}
}
#endif