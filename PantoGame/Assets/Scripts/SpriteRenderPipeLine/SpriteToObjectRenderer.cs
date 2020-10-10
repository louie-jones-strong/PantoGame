﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToObjectRenderer : RotateToCam
{

	[SerializeField] MeshRenderer FrontRenderer;
	[SerializeField] MeshRenderer BackRenderer;

	[SerializeField] bool EmissionOn;
	[SerializeField] bool RotateToCamOn;

	protected override void LateUpdate()
	{
		if (RotateToCamOn)
		{
			base.LateUpdate();
		}
	}

	public void SetImage(SpriteRenderer spriteRenderer, bool emissionOn=false, bool rotateToCamOn=true)
	{
		var image = spriteRenderer.sprite;
		EmissionOn = emissionOn;
		RotateToCamOn = rotateToCamOn;

		var newImageMaterial = new Material(FrontRenderer.material);

		var texName = image.name;
		newImageMaterial.name = texName;
		newImageMaterial.mainTexture = image.texture;

		if (EmissionOn)
		{
			newImageMaterial.EnableKeyword("_EMISSION");
			newImageMaterial.SetTexture("_EmissionMap", image.texture);
		}
		else
		{
			newImageMaterial.DisableKeyword("_EMISSION");
		}

		FrontRenderer.material = newImageMaterial;
		BackRenderer.material = newImageMaterial;
		SetImageSizePos(spriteRenderer);
	}

	void SetImageSizePos(SpriteRenderer spriteRenderer)
	{
		var image = spriteRenderer.sprite;
		float width = image.texture.width / image.pixelsPerUnit;
		float height = image.texture.height / image.pixelsPerUnit;

		var xPos = ((image.texture.width/2) - image.pivot.x) / image.pixelsPerUnit;
		var yPos = ((image.texture.height/2) - image.pivot.y) / image.pixelsPerUnit;


		if (spriteRenderer.flipX)
		{
			width *= -1;
			xPos *= -1;
		}
		
		if (spriteRenderer.flipY)
		{
			height *= -1;
			yPos *= -1;
		}

		transform.localScale = new Vector3(width, height, 1);
		transform.localPosition = new Vector3(xPos, yPos, 0);
	}
}
