using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
	public static FlowManager Instance;

	void Start()
	{
		//TransToOverworld();
		Instance = this;
	}

	//public void TransToOverworld(string sceneFrom = "", bool isNewDungeon=false)
	//{
	//	//StartCoroutine(TransToOverworldCo(sceneFrom, isNewDungeon));
	//}
//
	//IEnumerator TransToOverworldCo(string sceneFrom, bool isNewDungeon)
	//{
	//	if (!sceneFrom.Equals(""))
	//	{
	//		if (sceneFrom != Settings.SceneOverworld)
	//		{
	//			yield return ScreenTransitionManager.WaitForSetBlack(true, Vector2.zero);
	//		}
	//		yield return StartCoroutine(MainManager.Instance.SubtractSceneCo(sceneFrom));
	//		
	//	}
//
	//	yield return StartCoroutine(MainManager.Instance.AddSceneCo(Settings.SceneOverworld));
	//	FindObjectOfType<CurrentRoom>().Setup(sceneFrom == Settings.SceneOverworld && !isNewDungeon);
	//	yield return null;
	//	var transPos = Vector2.zero;
	//	var player = FindObjectOfType<Player>();
	//	if (player != null)
	//	{
	//		transPos = player.transform.position;
	//	}
	//	ScreenTransitionManager.SetShowBlack(false, transPos);
//
	//}
//
	//public void TransToTitle(string sceneFrom)
	//{
	//	StartCoroutine(TransToTitleCo(sceneFrom));
	//}
//
	//IEnumerator TransToTitleCo(string sceneFrom)
	//{
	//	yield return ScreenTransitionManager.WaitForSetBlack(true, Vector2.zero);
	//	if (!sceneFrom.Equals(""))
	//	{
	//		yield return MainManager.Instance.SubtractSceneCo(sceneFrom);
	//	}
	//	OverworldMemory.ClearAll();
	//	MainManager.Instance.GoToTitle();
	//	ScreenTransitionManager.SetShowBlack(false, Vector2.zero);
	//}
}
