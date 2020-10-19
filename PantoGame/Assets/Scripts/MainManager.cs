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
			AddScene(Settings.MenuScreenName);
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

	public void TransToScreen(string screenTo, string sceneFrom="")
	{
		StartCoroutine(TransToScreenCo(screenTo, sceneFrom));
	}

	IEnumerator TransToScreenCo(string screenTo, string sceneFrom)
	{
		if (!string.IsNullOrEmpty(sceneFrom))
		{
			yield return StartCoroutine(SubtractSceneCo(sceneFrom));
		}

		yield return StartCoroutine(AddSceneCo(screenTo));
	}



#region private Screen stuff
	static void AddScene(string scene)
	{
		Instance.StartCoroutine(AddSceneCo(scene));
	}
	public static IEnumerator AddSceneCo(string scene)
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
	public static IEnumerator SubtractSceneCo(string scene)
	{
		if (SceneManager.GetSceneByName(scene).isLoaded)
		{
			yield return SceneManager.UnloadSceneAsync(scene);
		}
	}
#endregion
}
