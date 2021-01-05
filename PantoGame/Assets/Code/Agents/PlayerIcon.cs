using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
	[SerializeField] Image Icon;

	public void SetIcon(Agent agent)
	{
		if (agent == null)
		{
			return;
		}

		Icon.color = agent.Colour;
	}
}
