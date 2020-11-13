using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptGenerator", menuName = "ScriptableObjects/ScriptGenerator")]
public class ScriptGenerator : ScriptableObject
{
	public List<Scene> Scenes;


	public Script CreateScript()
	{
		var script = new Script();
		script.Scenes = Scenes;
		return script;
	}
}