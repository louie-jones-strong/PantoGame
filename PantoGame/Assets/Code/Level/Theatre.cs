using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theatre : PlayerManger
{
	public static Theatre Instance;
	[SerializeField] AudienceAgent Audience;
	public Script CurrentScript {get; private set;}
	public Transform Toilet;
	public Transform Lobby;
	public Script Generator = new Script();//todo make this Generator not script

	Chair[] Chairs;
	public List<AudienceAgent> AudienceAgents {get; private set;} = new List<AudienceAgent>();


	[SerializeField] float StageMinXAxis;
	[SerializeField] float StageMaxXAxis;
	[SerializeField] float StageMinZAxis;
	[SerializeField] float StageMaxZAxis;


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

	public bool IsPointOnStage(Vector3 point)
	{
		return StageMinXAxis <= point.x &&
				StageMaxXAxis >= point.x &&
				StageMinZAxis <= point.z &&
				StageMaxZAxis >= point.z;
	}

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;

		var pos = new Vector3(
			StageMinXAxis + (StageMaxXAxis - StageMinXAxis)/2, 
			5f,
			StageMinZAxis + (StageMaxZAxis - StageMinZAxis)/2);

		var size = new Vector3(StageMaxXAxis - StageMinXAxis, 10f, StageMaxZAxis - StageMinZAxis);

		Gizmos.DrawWireCube(pos, size);
	}
#endif
}
