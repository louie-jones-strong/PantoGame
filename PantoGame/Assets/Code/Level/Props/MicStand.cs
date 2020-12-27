using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MicStand : Prop, PropHolderInterface
{
	[SerializeField] Transform MicRoot;

	PropInterface PropSlot;

	public bool CanHoldProp(PropInterface prop)
	{
		var mic = prop as Microphone;
		return mic != null && PropSlot == null;
	}

	public void AddProp(PropInterface prop)
	{
		if (CanHoldProp(prop))
		{
			PropSlot = prop;
		}
	}
	
	public void RemoveProp(PropInterface prop)
	{
		if (prop == PropSlot)
		{
			PropSlot = null;
		}
		else
		{
			Logger.LogError($"Trying to remove prop: {prop} != PropSlot: {PropSlot}");
		}
	}
}