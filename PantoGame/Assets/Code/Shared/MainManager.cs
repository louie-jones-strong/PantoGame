using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
	[SerializeField] Animator ScreenTransition;
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
		SetShowBlack(false);
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
		yield return StartCoroutine(WaitForSetBlack(true));
		if (!string.IsNullOrEmpty(sceneFrom))
		{
			yield return StartCoroutine(SubtractSceneCo(sceneFrom));
		}

		yield return StartCoroutine(AddSceneCo(screenTo));
		SetShowBlack(false);
	}

	public void LoadLevel(string theatreTo, int levelIndex, string sceneFrom="")
	{
		StartCoroutine(LoadLevelCo(theatreTo, levelIndex, sceneFrom));
	}
	IEnumerator LoadLevelCo(string theatreTo, int levelIndex, string sceneFrom)
	{
		yield return StartCoroutine(WaitForSetBlack(true));
		if (!string.IsNullOrEmpty(sceneFrom))
		{
			yield return StartCoroutine(SubtractSceneCo(sceneFrom));
		}

		yield return StartCoroutine(AddSceneCo(Settings.HudScreenName));
		yield return StartCoroutine(AddSceneCo(theatreTo));

		while (Theatre.Instance == null)
		{
			yield return null;
		}

		Theatre.Instance.SetLevel(levelIndex);
		SetShowBlack(false);
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

	public static void SubtractScene(string scene)
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

#region ScreenTransition

	bool IsAnimating
	{get
		{
			var stateInfo = ScreenTransition.GetCurrentAnimatorStateInfo(0);
			return !stateInfo.IsName("Black") && !stateInfo.IsName("Open");
		}
	}

	bool ShowingBlack;

	void SetShowBlack(bool showBlack)
	{
		ScreenTransition.SetBool("Open", !showBlack);

		Logger.Log($"setting show black to: {showBlack} was {ShowingBlack}");
		ShowingBlack = showBlack;

		ScreenTransition.speed = 1;
	}

	IEnumerator WaitForSetBlack(bool showBlack)
	{
		SetShowBlack(showBlack);
		yield return null;

		do
		{
			yield return null;
		} while (IsAnimating);

		yield break;
	}
#endregion
}
