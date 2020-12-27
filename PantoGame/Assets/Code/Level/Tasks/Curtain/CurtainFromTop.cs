using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainFromTop : Curtain
{
	[SerializeField] GameObject Root;
	[SerializeField] float Min;
	[SerializeField] float Max;
	
	public override void SetOpenAmount()
	{
		var pos = Root.transform.localPosition;
		
		Root.transform.localPosition = new Vector3(pos.x, Min + (Max-Min)*OpenAmount, pos.z);
	}

}
