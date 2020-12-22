using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
	[SerializeField] int ControlType;
	[SerializeField] Text Name;
	[SerializeField] Animator AnimatorUi;
	[SerializeField] PlayerIcon PlayerIcon;

	void Update()
	{
		Name.text = $"Player: {ControlType}";
		var playerActive = PlayerManger.Players.TryGetValue(ControlType, out var player);
		playerActive = playerActive && player != null;

		AnimatorUi.SetBool("Active", playerActive);

		PlayerIcon.SetIcon(player);
	}
}