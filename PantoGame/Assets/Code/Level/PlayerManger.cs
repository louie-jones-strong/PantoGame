using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManger : MonoBehaviour
{
	public Transform PropsRoot;
	[SerializeField] PlayerAgent Player;
	public static Dictionary<int, PlayerAgent> Players { get; private set; } = new Dictionary<int, PlayerAgent>();
	public static PlayerManger MangerInstance;

	protected virtual void Awake()
	{
		MangerInstance = this;
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

		var player = Instantiate<PlayerAgent>(Player, transform);
		Players[controlType] = player;
		Players[controlType].ControlType = controlType;

		AudioManger.PlayEvent("PlayerJoin", player.transform);
	}

	void RemovePlayer(int controlType)
	{
		//AudioManger.PlayEvent("PlayerLeave", player.transform);
	}
}