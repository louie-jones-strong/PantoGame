using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theatre : MonoBehaviour
{
	public static Theatre Instance;
	public Transform Toilet;

	Chair[] Chairs;
	List<AudienceAgent> AudienceAgents = new List<AudienceAgent>();

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			Chairs = GetComponentsInChildren<Chair>();
		}
		else
		{
			Logger.LogError("Theatre.Instance already set");
		}
	}

	public Chair AddAudienceAgent(AudienceAgent agent)
	{
		Chair chair = null;
		if(Chairs.Length > AudienceAgents.Count)
		{
			chair = Chairs[AudienceAgents.Count];
		}
		AudienceAgents.Add(agent);

		return chair;
	}
}
