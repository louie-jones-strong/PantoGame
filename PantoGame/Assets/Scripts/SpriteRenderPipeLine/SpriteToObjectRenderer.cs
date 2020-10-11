using System.Collections;
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
			PointAtCam();
		}
	}

	public void SetImage(SpriteRenderer spriteRenderer, bool emissionOn=false, bool rotateToCamOn=true)
	{
		var sprite = spriteRenderer.sprite;
		EmissionOn = emissionOn;
		RotateToCamOn = rotateToCamOn;

		var newImageMaterial = new Material(FrontRenderer.material);

		var texName = sprite.name;
		newImageMaterial.name = texName;
		newImageMaterial.mainTexture = sprite.texture;

		if (EmissionOn)
		{
			newImageMaterial.EnableKeyword("_EMISSION");
			newImageMaterial.SetTexture("_EmissionMap", sprite.texture);
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
		var sprite = spriteRenderer.sprite;
		float width = sprite.texture.width / sprite.pixelsPerUnit;
		float height = sprite.texture.height / sprite.pixelsPerUnit;

		var xPos = ((sprite.texture.width/2) - sprite.pivot.x) / sprite.pixelsPerUnit;
		var yPos = ((sprite.texture.height/2) - sprite.pivot.y) / sprite.pixelsPerUnit;

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
