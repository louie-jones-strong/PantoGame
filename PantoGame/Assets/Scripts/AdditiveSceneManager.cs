using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SimpleInput))]
public class AdditiveSceneManager : MonoBehaviour
{
	[SerializeField] bool OpenBootScreen = true;
	void Awake()
	{
		if (MainManager.Instance == null)
		{
			if (OpenBootScreen)
			{
				StartCoroutine(LoadBootCo());
			}
		}
	}

	IEnumerator LoadBootCo()
	{
		yield return MainManager.AddSceneCo(Settings.BootScreenName);
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			var scene = SceneManager.GetSceneAt(i);
			if (scene.name != Settings.BootScreenName &&
				scene.name != Settings.MenuScreenName)
			{
				yield return SceneManager.UnloadSceneAsync(scene);
			}
		}
	}
}
