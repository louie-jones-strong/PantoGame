using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
	[SerializeField] Image Icon;

	public void SetIcon(PlayerAgent player)
	{
		if (player == null)
		{
			return;
		}

		Icon.color = player.Colour;
	}
}
