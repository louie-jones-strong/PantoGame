using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteToObjectRenderer : RotateToCam
{

	[SerializeField] MeshRenderer FrontRenderer;
	[SerializeField] MeshRenderer BackRenderer;

	[SerializeField] bool EmissionOn;
	[SerializeField] bool RotateToCamOn;

	Material CurrentMaterial;
	Color CurrentColour = Color.white;


	static Dictionary<string, MaterialData> MaterialCache = new Dictionary<string, MaterialData>();
	class MaterialData
	{
		public Material Material;
		public float Width;
		public float Height;
		public float XPos;
		public float YPos;
	}

	SpriteRenderer SpriteRenderer;
	Sprite LastSprite = null;

	protected override void LateUpdate()
	{
		if (RotateToCamOn)
		{
			PointAtCam();
		}

		UpdateMaterial();
	}

	public void Setup(SpriteRenderer spriteRenderer, bool emissionOn=false, bool rotateToCamOn=true)
	{
		SpriteRenderer = spriteRenderer;
		EmissionOn = emissionOn;
		RotateToCamOn = rotateToCamOn;

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

	void UpdateMaterialColour()
	{
		CurrentMaterial.color = CurrentColour;
		CurrentMaterial.SetColor("_Color", CurrentColour);
		CurrentMaterial.SetColor("_EmissionColor", CurrentColour);
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

		var texName = sprite.name;
		if (!MaterialCache.ContainsKey(texName))
		{
			var newMaterial = MakeNewMaterial();
			newMaterial.Material.name = texName;

			MaterialCache[texName] = newMaterial;
		}

		var data = MaterialCache[texName];
		CurrentMaterial = new Material(data.Material);
		FrontRenderer.material = CurrentMaterial;
		BackRenderer.material = CurrentMaterial;

		UpdateMaterialColour();

		transform.localScale = new Vector3(data.Width, data.Height, 1);
		transform.localPosition = new Vector3(data.XPos, data.YPos, 0);
		LastSprite = sprite;
	}

	MaterialData MakeNewMaterial()
	{
		var sprite = SpriteRenderer.sprite;
		var newMaterial = new Material(FrontRenderer.material);
		var materialData = new MaterialData();
		materialData.Material = newMaterial;

		if (EmissionOn)
		{
			newMaterial.EnableKeyword("_EMISSION");
		}
		else
		{
			newMaterial.DisableKeyword("_EMISSION");
		}

		Texture spriteTexture = GetSpriteTexture(sprite);

		if (spriteTexture == null)
		{
			return materialData;
		}
		
		newMaterial.mainTexture = spriteTexture;
		newMaterial.SetTexture("_EmissionMap", spriteTexture);

		//set pos
		materialData.Width = spriteTexture.width / sprite.pixelsPerUnit;
		materialData.Height = spriteTexture.height / sprite.pixelsPerUnit;

		materialData.XPos = ((spriteTexture.width/2) - sprite.pivot.x) / sprite.pixelsPerUnit;
		materialData.YPos = ((spriteTexture.height/2) - sprite.pivot.y) / sprite.pixelsPerUnit;

		if (SpriteRenderer.flipX)
		{
			materialData.Width *= -1;
			materialData.XPos *= -1;
		}
		
		if (SpriteRenderer.flipY)
		{
			materialData.Height *= -1;
			materialData.YPos *= -1;
		}

		return materialData;
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
