using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainFromSides : Curtain
{
	[SerializeField] GameObject RootLeft;
	[SerializeField] GameObject RootRight;
	
	public override void SetOpenAmount()
	{
		if (RootLeft == null ||
			RootRight == null)
		{
			return;
		}
		var scale = new Vector3(1-OpenAmount, 1, 1);
		RootLeft.transform.localScale = scale;
		RootRight.transform.localScale = scale;
	}

}
