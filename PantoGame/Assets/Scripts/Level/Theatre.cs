using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theatre : PlayerManger
{
	public static Theatre Instance;
	[SerializeField] AudienceAgent Audience;
	public Script CurrentScript {get; private set;}
	public Transform Toilet;
	public Script Generator = new Script();//todo make this Generator not script

	Chair[] Chairs;
	List<AudienceAgent> AudienceAgents = new List<AudienceAgent>();


	protected override void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		
		CurrentScript = Generator;
		
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();

		Chairs = GetComponentsInChildren<Chair>();
		for (int i = 0; i < Chairs.Length; i++)
		{
			var agent = Instantiate<AudienceAgent>(Audience, transform);
			agent.Setup(AddAudienceAgent(agent));
		}
	}

	protected override void Update()
	{
		base.Update();
		CurrentScript.Update();
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
