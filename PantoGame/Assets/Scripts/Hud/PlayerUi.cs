using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
	[SerializeField] int ControlType;
	[SerializeField] Text Name;
	[SerializeField] Animator AnimatorUi;

	void Update()
	{
		Name.text = $"ControlType: {ControlType}";
		var playerActive = PlayerManger.Players.ContainsKey(ControlType);
		AnimatorUi.SetBool("Active", playerActive);
	}
}