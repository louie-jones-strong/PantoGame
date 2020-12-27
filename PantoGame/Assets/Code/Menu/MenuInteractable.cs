using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuInteractable : Interactable
{
	[SerializeField] Transform Root;
	Menu Menu;

	protected override void Awake()
	{
	}

	public void Setup(Menu menu, Vector2 Pos, float xSize, float ySize)
	{
		Menu = menu;


		TriggerXDistance = xSize / 2;
		TriggerYDistance = ySize / 2;
		Root.localScale = new Vector3(xSize, ySize, 0);

		transform.position = new Vector3(Pos.x, 0, Pos.y);
		CameraController.AddTarget(transform, 1);
	}

	public virtual void SetFade(float value)
	{
		var pos = transform.localPosition;
		pos.y = (1-value) * 10;
		transform.localPosition = pos;
	}
}