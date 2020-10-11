using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theatre : MonoBehaviour
{
	public static Theatre Instance;
	public Transform Toilet;
	public List<Task> Tasks = new List<Task>();

	[SerializeField] PlayerAgent Player;
	Dictionary<int, PlayerAgent> Players = new Dictionary<int, PlayerAgent>();

	Chair[] Chairs;
	List<AudienceAgent> AudienceAgents = new List<AudienceAgent>();


	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			Chairs = GetComponentsInChildren<Chair>();
			Players[0] = Player;
			Player.ControlType = 0;
		}
		else
		{
			Logger.LogError("Theatre.Instance already set");
		}
	}

	void Update()
	{
		for (int loop = 0; loop < SimpleInput.ControlSetCount; loop++)
		{
			if (!Players.ContainsKey(loop) &&
				SimpleInput.GetInputActive(eInput.Dash, loop))
			{
				Players[loop] = Instantiate<PlayerAgent>(Player);
				Players[loop].ControlType = loop;
			}
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
