using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Script
{
	public Scene CurrentScene {get {return Scenes[SceneIndex];}}
	public int SceneIndex;
	public List<Scene> Scenes;

	//todo add textures to add to theater walls
	//todo add costume sprites

	public void Update()
	{
		foreach (var scene in Scenes)
		{
			scene.Update();
		}
	}
}