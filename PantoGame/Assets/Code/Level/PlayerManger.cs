using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManger : MonoBehaviour
{
	[SerializeField] PlayerAgent Player;
	public static Dictionary<int, PlayerAgent> Players { get; private set; } = new Dictionary<int, PlayerAgent>();

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{

	}

	protected virtual void Update()
	{
		for (int loop = 0; loop < SimpleInput.ControlSetCount; loop++)
		{
			if (Players.TryGetValue(loop, out var player))
			{
				if (player == null)
				{
					Players.Remove(loop);
					AddPlayer(loop);
				}
			}
			
			if (!Players.ContainsKey(loop) &&
				SimpleInput.GetInputActive(eInput.Dash, loop))
			{
				AddPlayer(loop);
			}
		}
	}

	void AddPlayer(int controlType)
	{
		var parent = transform;
		if (MainManager.Instance != null)
		{
			parent = MainManager.Instance.transform;
		}

		Players[controlType] = Instantiate<PlayerAgent>(Player, transform);
		Players[controlType].ControlType = controlType;
	}
}