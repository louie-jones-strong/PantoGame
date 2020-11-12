using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theatre : PlayerManger
{
	public static Theatre Instance;
	public Transform Toilet;

	[SerializeField] AudienceAgent Audience;

	Chair[] Chairs;
	List<AudienceAgent> AudienceAgents = new List<AudienceAgent>();


	protected override void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		base.Awake();
	}

	protected override void Start()
	{
		Chairs = GetComponentsInChildren<Chair>();
		for (int i = 0; i < Chairs.Length; i++)
			{
			var agent = Instantiate<AudienceAgent>(Audience, transform);
			agent.Setup(AddAudienceAgent(agent));
			}
		}

	void OnDestroy()
	{
		Instance = null;
	}

	Chair AddAudienceAgent(AudienceAgent agent)
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
