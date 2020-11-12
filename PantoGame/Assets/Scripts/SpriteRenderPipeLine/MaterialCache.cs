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
			Logger.LogError("MaterialCache.Instance == null");
			return;
		}
		Instance = this;

		LoadSpritesInFolder("Assets/Sprites");
	}

	void LoadSpritesInFolder(string path)
	{
		Logger.Log($"LoadSpritesInFolder called with {path}");

		var files = Directory.GetFiles(path);

		foreach (var file in files)
		{
			if(File.Exists(file))
            {
                var items = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(file);
				foreach (var item in items)
				{
					var sprite = item as Sprite;
					if (sprite != null)
					{
						GetMaterial(sprite);
					}
				}
            }
			else
			{
				Logger.LogError($"Path: {file} not a file or a folder");
			}
		}

		files = Directory.GetDirectories(path);

		foreach (var file in files)
		{
			if(Directory.Exists(file))
            {
                LoadSpritesInFolder(file);
            }
			else
			{
				Logger.LogError($"Path: {file} not a file or a folder");
			}
		}
	}

	void OnDestroy()
	{
		Instance = null;
	}

	public static MaterialData GetMaterial(Sprite sprite)
	{
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
