using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuInteractable : Interactable
{
	[SerializeField] Transform Root;
	[SerializeField] AnimationCurve FadeCurve;

	protected override void Awake()
	{
	}

	public void Setup(Vector2 Pos, float xSize, float ySize)
	{


		TriggerXDistance = xSize / 2;
		TriggerYDistance = ySize / 2;
		Root.localScale = new Vector3(xSize, ySize, 1);

		transform.position = new Vector3(Pos.x, 0, Pos.y);
		CameraController.AddTarget(transform, 1);
		SetFade(0f);
	}

	public virtual void SetFade(float value)
	{
		var size = FadeCurve.Evaluate(1-value);
		transform.localScale = new Vector3(size, 1, size);
	}
}