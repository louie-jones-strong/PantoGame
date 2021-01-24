using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ClothesHolder : PropHolder
{
	public eClothesType ClothesHolderType;

	public override bool CanHoldProp(Prop prop, float distance=-1)
	{
		var clothesItem = prop as ClothesItem;
		if (clothesItem == null)
		{
			return false;
		}

		if (clothesItem.ClothesType != ClothesHolderType)
		{
			return false;
		}

		return base.CanHoldProp(prop, distance:distance);
	}
}