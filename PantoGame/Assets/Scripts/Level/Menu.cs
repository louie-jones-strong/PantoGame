using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Menu : PlayerManger
{
	[SerializeField] MenuButton MenuButtonPrefab;
	bool Busy = false;

	protected override void Start()
	{
		MainMenu();
		base.Start();
	}

	void MainMenu()
	{
		AddButton("Play", true, LevelPickerMenu);
	}

	void LevelPickerMenu()
	{
		if (Busy)
		{
			return;
		}

		//todo when we have more levels add them in here
		MainManager.Instance.TransToScreen(Settings.WalkingTestScreenName, Settings.MenuScreenName);
		Busy = true;
	}



	void AddButton(string label, bool triggerNeedsEveryone, Action onClick, float xSize=10, float ySize=4)
	{
		var button = Instantiate<MenuButton>(MenuButtonPrefab, transform);
		button.Setup(this, label, triggerNeedsEveryone, xSize, ySize, onClick);
	}
}