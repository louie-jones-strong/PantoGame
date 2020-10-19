using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Menu : PlayerManger
{
	[SerializeField] MenuButton MenuButtonPrefab;

	protected override void Awake()
	{
		MainMenu();
		base.Awake();
	}

	void MainMenu()
	{
		AddButton("Play", true, LevelPickerMenu);
	}

	void LevelPickerMenu()
	{
		//todo when we have more levels add them in here
		MainManager.Instance.TransToScreen(Settings.WalkingTestScreenName, Settings.MenuScreenName);
	}



	void AddButton(string label, bool triggerNeedsEveryone, Action onClick, float xSize=10, float ySize=4)
	{
		var button = Instantiate<MenuButton>(MenuButtonPrefab);
		button.Setup(this, label, triggerNeedsEveryone, xSize, ySize, onClick);
	}
}