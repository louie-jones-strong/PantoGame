using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
	public static MainManager Instance { get; private set; }

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			enabled = false;
			return;
		}
	}

	void Update()
	{

		if (Input.GetButton("Exit"))
		{
			Debug.Log("Quitting");
			Application.Quit();
		}
	}


	public static IEnumerator LoadBootCo()
	{
		yield return AddSceneCo(Settings.BootScreenName);
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			var scene = SceneManager.GetSceneAt(i);
			if (scene.name != Settings.BootScreenName)
			{
				yield return SceneManager.UnloadSceneAsync(scene);
			}
		}
	}



#region private Screen stuff
	static void AddScene(string scene)
	{
		Instance.StartCoroutine(AddSceneCo(scene));
	}
	static IEnumerator AddSceneCo(string scene)
	{
		if (!SceneManager.GetSceneByName(scene).isLoaded)
		{
			yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
		}
	}

	static void SubtractScene(string scene)
	{
		Instance.StartCoroutine(SubtractSceneCo(scene));
	}
	static IEnumerator SubtractSceneCo(string scene)
	{
		if (SceneManager.GetSceneByName(scene).isLoaded)
		{
			yield return SceneManager.UnloadSceneAsync(scene);
		}
	}
#endregion
}
