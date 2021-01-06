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
			StartCoroutine(BootCo());
		}
		else
		{
			enabled = false;
			return;
		}
	}

	IEnumerator BootCo()
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			var scene = SceneManager.GetSceneAt(i);
			if (scene.name != Settings.BootScreenName)
			{
				yield return SceneManager.UnloadSceneAsync(scene);
			}
		}
		AddScene(Settings.MenuScreenName);
		AddScene(Settings.HudScreenName);
	}

	void Update()
	{

		if (Input.GetButton("Exit"))
		{
			CloseGame();
		}
	}

	public void CloseGame()
	{
		Debug.Log("Quitting");
		Application.Quit();
#if UNITY_EDITOR
		// Application.Quit() does not work in the editor so
		// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

#region screen stuff

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

	public void LoadLevel(int levelIndex, string sceneFrom="")
	{
		StartCoroutine(LoadLevelCo(levelIndex, sceneFrom));
	}
	IEnumerator LoadLevelCo(int levelIndex, string sceneFrom)
	{
		if (!string.IsNullOrEmpty(sceneFrom))
		{
			yield return StartCoroutine(SubtractSceneCo(sceneFrom));
		}

		yield return StartCoroutine(AddSceneCo(Settings.TheatreScreenName));

		while (Theatre.Instance == null)
		{
			yield return null;
		}

		Theatre.Instance.SetLevel(levelIndex);
	}

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
