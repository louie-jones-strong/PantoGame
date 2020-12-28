using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MicStand : PropHolder
{
	public override bool CanHoldProp(Prop prop, float distance=-1)
	{
		if (!(prop is Microphone))
		{
			return false;
		}
		return base.CanHoldProp(prop, distance:distance);
	}
}