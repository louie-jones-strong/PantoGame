using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : Interactable
{
	[SerializeField] float Speed = 0.1f;

	[SerializeField] GameObject Root;
	[SerializeField] float Min;
	[SerializeField] float Max;

	public float OpenAmount;
	
	public override void Interact(int controlIndex)
	{
		var delta = SimpleInput.GetInputValue(eInput.YAxis, index: controlIndex) * Speed;

		OpenAmount += delta * Time.deltaTime;
		OpenAmount = Mathf.Clamp01(OpenAmount);
		
		var pos = Root.transform.localPosition;
		
		Root.transform.localPosition = new Vector3(pos.x, Min + (Max-Min)*OpenAmount, pos.z);
	}

}
