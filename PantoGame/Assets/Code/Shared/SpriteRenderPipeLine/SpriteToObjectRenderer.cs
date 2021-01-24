﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteToObjectRenderer : MonoBehaviour
{

	[SerializeField] MeshRenderer MeshRenderer;

	[SerializeField] bool EmissionOn;
	public bool RotateToCamOn;

	Material CurrentMaterial;
	Color CurrentColour = Color.white;

	SpriteRenderer SpriteRenderer;
	Sprite LastSprite = null;

	void LateUpdate()
	{
		if (RotateToCamOn)
		{
			RotateToCam.PointAtCam(transform);
		}

		UpdateMaterial();
	}

	public void Setup(SpriteRenderer spriteRenderer, bool emissionOn=false, bool rotateToCamOn=true)
	{
		SpriteRenderer = spriteRenderer;
		EmissionOn = emissionOn;
		RotateToCamOn = rotateToCamOn;

		CurrentColour = spriteRenderer.color;

		UpdateMaterial();
	}

	public void SetColour(Color colour)
	{
		if (CurrentColour == colour)
		{
			return;
		}
		CurrentColour = colour;
		UpdateMaterialColour();
	}

	void UpdateMaterial()
	{
		if (SpriteRenderer?.sprite == null)
		{
			return;
		}
		var sprite = SpriteRenderer.sprite;

		if (LastSprite == sprite)
		{
			return;
		}

		var data = MaterialCache.GetMaterial(sprite);
		if (data == null)
		{
			Logger.LogWarning($"GetMaterial returned null");
			return;
		}

		CurrentMaterial = new Material(data.Material);
		
		MeshRenderer.material = CurrentMaterial;

		UpdateMaterialColour();

		if (SpriteRenderer.flipX)
		{
			data.Width *= -1;
			data.XPos *= -1;
		}
		
		if (SpriteRenderer.flipY)
		{
			data.Height *= -1;
			data.YPos *= -1;
		}

		transform.localScale = new Vector3(data.Width, data.Height, 1);
		transform.localPosition = new Vector3(data.XPos, data.YPos, 0);
		LastSprite = sprite;
	}

	void UpdateMaterialColour()
	{
		if (CurrentMaterial == null)
		{
			Logger.LogWarning($"CurrentColour == null");
			return;
		}

		CurrentMaterial.color = CurrentColour;
		CurrentMaterial.SetColor("_Color", CurrentColour);

		if (EmissionOn)
		{
			CurrentMaterial.EnableKeyword("_EMISSION");
		}
		else
		{
			CurrentMaterial.DisableKeyword("_EMISSION");
		}
		CurrentMaterial.SetColor("_EmissionColor", CurrentColour);
	}
}
