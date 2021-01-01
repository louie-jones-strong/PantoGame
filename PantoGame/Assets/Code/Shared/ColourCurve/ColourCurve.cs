using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ColourCurve
{
	[Serializable]
	public class ColourCurvePoint
	{
		public float Value;
		public Color Colour;
	}

	public List<ColourCurvePoint> PointList = new List<ColourCurvePoint>();

	public Color GetColor(float value)
	{
		if (PointList.Count == 0)
		{
			return Color.magenta;
		}

		ColourCurvePoint Before = PointList[0];
		ColourCurvePoint Next = PointList[0];

		for (int i = 1; i < PointList.Count; i++)
		{
			var item = PointList[i];

			if (item.Value >= value && 
				(item.Value <= Next.Value ||
				Next.Value < value))
			{
				Next = item; 
			}
			
			if (item.Value <= value && 
				(item.Value >= Before.Value ||
				Before.Value > value))
			{
				Before = item;
			}
		}

		if (Before == Next)
		{
			return Before.Colour;
		}

		float blend = (value - Before.Value) / (Next.Value - Before.Value);
		return Color.Lerp(Before.Colour, Next.Colour, blend);
	}
}