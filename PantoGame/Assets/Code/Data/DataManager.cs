using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataManager: MonoBehaviour
{
	public static DataManager Instance {get; private set;}

	const string AudienceDataPath = "Assets\\Data\\AudienceData.json";
	public AudienceData AudienceData;
	

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public void Load()
	{
		Logger.Log("DataManager Loading");
		
		var jsonText = File.ReadAllText(AudienceDataPath);
		AudienceData = JsonUtility.FromJson<AudienceData>(jsonText);
	}

	public void Save()
	{
		Logger.Log("DataManager Save");
		
		var jsonText = JsonUtility.ToJson(AudienceData, prettyPrint:true);
		File.WriteAllText(AudienceDataPath, jsonText);
	}
}