using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuColourPicker : MenuButton
{
	[SerializeField] Color Colour;

	public void Setup(Color colour, Vector2 pos, float xSize, float ySize, Action onClick=null)
	{
		Colour = colour;
		base.Setup("", false, pos, xSize, ySize, onClick);
	}

	public override void StartInteraction(PlayerAgent playerAgent)
	{
		playerAgent.SetColour(Colour);
		base.StartInteraction(playerAgent);
	}
}