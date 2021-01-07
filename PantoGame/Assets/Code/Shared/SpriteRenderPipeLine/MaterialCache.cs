using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MaterialCache: MonoBehaviour
{
	[SerializeField] Material DefaultMaterial;
	static MaterialCache Instance;

	Dictionary<string, MaterialData> Cache = new Dictionary<string, MaterialData>();
	
	public class MaterialData
	{
		public Material Material;
		public float Width;
		public float Height;
		public float XPos;
		public float YPos;
	}

	void Awake()
	{
		if (Instance != null)
		{
			Logger.LogWarning("MaterialCache.Instance != null but awake called");
			return;
		}
		Instance = this;
	}

	void OnDestroy()
	{
		if (Instance == this)
		{
			Logger.LogError("MaterialCache OnDestroy called so Instance == null");
			Instance = null;
		}
	}

	public static MaterialData GetMaterial(Sprite sprite)
	{
		if (Instance == null)
		{
			return null;
		}

		var key = sprite.name;
		if (!Instance.Cache.ContainsKey(key))
		{
			var newMaterial = Instance.MakeNewMaterial(sprite);
			newMaterial.Material.name = key;

			Instance.Cache[key] = newMaterial;
			Logger.LogWarning($"GetMaterial key: {key} not in cache creating new one");
		}

		return Instance.Cache[key];
	}

	MaterialData MakeNewMaterial(Sprite sprite)
	{
		var newMaterial = new Material(DefaultMaterial);
		var materialData = new MaterialData();
		materialData.Material = newMaterial;

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
