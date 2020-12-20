using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableIcon : MonoBehaviour
{
	public const string PrefabName = "InteractableIcon";
	[SerializeField] Animator IconAnimator;

	void Awake()
	{
		var pos = transform.localPosition;
		pos.y += 2f;
		transform.localPosition = pos;
	}
}
