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
		DrawDefaultInspector();
		
		var targetSound = (Sound)target;

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
}
#endif