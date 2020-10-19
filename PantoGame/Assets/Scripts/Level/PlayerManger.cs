using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManger : MonoBehaviour
{
	[SerializeField] PlayerAgent Player;
	public Dictionary<int, PlayerAgent> Players = new Dictionary<int, PlayerAgent>();

	protected virtual void Awake()
	{

	}

	protected virtual void Start()
	{

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
}