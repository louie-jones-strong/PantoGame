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
			base.LateUpdate();
		}
	}

	public void SetImage(Sprite image, bool emissionOn=false, bool rotateToCamOn=true)
	{
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
		SetImageSizePos(image);
	}

	void SetImageSizePos(Sprite image)
	{
		float width = image.texture.width / image.pixelsPerUnit;
		float height = image.texture.height / image.pixelsPerUnit;

		transform.localScale = new Vector3(width, height, 0);

		var xPos = (image.pivot.x - (image.texture.width/2)) / image.pixelsPerUnit;
		var yPos = (image.pivot.y + (image.texture.height/2)) / image.pixelsPerUnit;
		transform.localPosition = new Vector3(xPos, yPos, 0);
	}
}
