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
		var pos = Vector2.zero;

		AddButton("Play", true, LevelPickerMenu, pos);

		pos.y += 10;
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

	void AddButton(string label, bool triggerNeedsEveryone, Action onClick, Vector2 pos, float xSize=10, float ySize=4)
	{
		var button = Instantiate<MenuButton>(MenuButtonPrefab, transform);
		button.Setup(this, label, triggerNeedsEveryone, pos, xSize, ySize, onClick);
	}
}