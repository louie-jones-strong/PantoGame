using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToObjectRenderer : RotateToCam
{

	[SerializeField] MeshRenderer FrontRenderer;
	[SerializeField] MeshRenderer BackRenderer;

	[SerializeField] bool EmissionOn;
	[SerializeField] bool RotateToCamOn;

	SpriteRenderer SpriteRenderer;
	Sprite LastSprite = null;

	protected override void LateUpdate()
	{
		if (RotateToCamOn)
		{
			PointAtCam();
		}
		UpdateImage();
	}

	public void SetImage(SpriteRenderer spriteRenderer, bool emissionOn=false, bool rotateToCamOn=true)
	{
		SpriteRenderer = spriteRenderer;
		EmissionOn = emissionOn;
		RotateToCamOn = rotateToCamOn;

		var newImageMaterial = new Material(FrontRenderer.material);
		
		var texName = SpriteRenderer.sprite.name;
		newImageMaterial.name = texName;

		if (EmissionOn)
		{
			newImageMaterial.EnableKeyword("_EMISSION");
		}
		else
		{
			newImageMaterial.DisableKeyword("_EMISSION");
		}

		UpdateImage();
	}

	void UpdateImage()
	{
		if (SpriteRenderer?.sprite == null)
		{
			return;
		}
		
		if (LastSprite == SpriteRenderer?.sprite)
		{
			return;
		}

		var material = FrontRenderer.material;

		var sprite = SpriteRenderer.sprite;
		Texture spriteTexture = GetSpriteTexture(sprite);

		if (spriteTexture == null)
		{
			return;
		}
		
		material.mainTexture = spriteTexture;
		material.SetTexture("_EmissionMap", spriteTexture);

		//set pos
		float width = spriteTexture.width / sprite.pixelsPerUnit;
		float height = spriteTexture.height / sprite.pixelsPerUnit;

		var xPos = ((spriteTexture.width/2) - sprite.pivot.x) / sprite.pixelsPerUnit;
		var yPos = ((spriteTexture.height/2) - sprite.pivot.y) / sprite.pixelsPerUnit;

		if (SpriteRenderer.flipX)
		{
			width *= -1;
			xPos *= -1;
		}
		
		if (SpriteRenderer.flipY)
		{
			height *= -1;
			yPos *= -1;
		}

		transform.localScale = new Vector3(width, height, 1);
		transform.localPosition = new Vector3(xPos, yPos, 0);
	}

	Texture GetSpriteTexture(Sprite sprite)
	{
		var spriteRect = sprite.rect;
		int width = (int)spriteRect.width;
		int height = (int)spriteRect.height;

		var spriteTexture = new Texture2D(width, height, sprite.texture.format, false);

		Graphics.CopyTexture(sprite.texture, 0, 0, (int)spriteRect.x, (int)spriteRect.y, width, height, spriteTexture, 0, 0, 0, 0);

		return spriteTexture;
	}
}
