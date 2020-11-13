using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scene", menuName = "ScriptableObjects/Scene")]
public class Scene : ScriptableObject
{
	public List<Task> Tasks;
	
	public Scene()
	{
		Tasks = new List<Task>();
	}

	public Scene(Scene sceneToCopy)
	{
		Tasks = sceneToCopy.Tasks;
	}
}