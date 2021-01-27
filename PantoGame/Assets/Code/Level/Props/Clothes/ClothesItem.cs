using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ClothesItem : Prop
{
	public eClothesType ClothesType;

	public override bool CanMoveProp()
	{
		if (PropHolder == null)
		{
			return true;
		}
		
		return !(PropHolder is ClothesHolder);
	}
}