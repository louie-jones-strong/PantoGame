using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManger : MonoBehaviour
{
	public static HudManger Instance {get; private set;}
	public RectTransform CanvasRect;

	void Awake()
	{
		if (Instance != null)
		{
			Logger.LogWarning($"HudManger.Instance already set but awake called again");
			return;
		}

		Instance = this;
	}

	void Update()
	{
	}
}