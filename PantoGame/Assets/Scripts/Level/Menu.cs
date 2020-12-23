using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Menu : PlayerManger
{

	enum eMenuState
	{
		Main,
		Settings,
		LevelSelect
	}

	[SerializeField] AnimationCurve FadeDown;
	[SerializeField] AnimationCurve FadeUp;
	[SerializeField] MenuButton MenuButtonPrefab;
	List<MenuButton> Buttons = new List<MenuButton>();
	eMenuState CurrentState;
	eMenuState TargetState;
	float TimeInState = 1;

	protected override void Start()
	{
		MainMenu();
		base.Start();
	}

	void SetAllButtonFade(float value)
	{
		foreach (var button in Buttons)
		{
			button.SetFade(value);
		}
	}


	void AddButton(string label, bool triggerNeedsEveryone, Action onClick, Vector2 pos, float xSize=10, float ySize=4)
	{
		MenuButton button = null;
		foreach (var item in Buttons)
		{
			if (!item.gameObject.activeSelf)
			{
				button = item;
				button.gameObject.SetActive(true);
				break;
			}
		}

		if (button == null)
		{
			button = Instantiate<MenuButton>(MenuButtonPrefab, transform);
			Buttons.Add(button);
		}

		button.Setup(this, label, triggerNeedsEveryone, pos, xSize, ySize, onClick);
	}

	void SetTarget(eMenuState state)
	{
		if (TargetState != CurrentState)
		{
			return;
		}

		TargetState = state;
		TimeInState = 0;
	}

	void RemoveAllButtons()
	{
		foreach (var button in Buttons)
		{
			button.gameObject.SetActive(false);
		}
	}

	protected override void Update()
	{
		base.Update();

		TimeInState += Time.deltaTime;

		if (CurrentState != TargetState)
		{
			var progress = FadeDown.Evaluate(TimeInState);
			SetAllButtonFade(progress);

			if (progress <= 0f)
			{
				CurrentState = TargetState;
				RemoveAllButtons();
				switch (TargetState)
				{
					case eMenuState.Main: 
						MainMenu();
						break;
					case eMenuState.Settings: 
						SettingsMenu();
						break;
					case eMenuState.LevelSelect: 
						LevelPickerMenu();
						break;
					default:
						Logger.LogError($"unexpected state: {TargetState}");
						break;
				}
				TimeInState = 0;
			}
		}
		else
		{
			var progress = FadeUp.Evaluate(TimeInState);
			SetAllButtonFade(progress);
		}
	}


#region Menu screens

	void MainMenu()
	{
		var pos = new Vector2(-10, 10);

		AddButton("Play", true, () => {SetTarget(eMenuState.LevelSelect);}, pos);
		pos.y -= 10;
		AddButton("Settings", false, () => {SetTarget(eMenuState.Settings);}, pos);
	}

	void SettingsMenu()
	{
		var pos = new Vector2(-10, 10);

		AddButton("Back", false, () => SetTarget(eMenuState.Main), pos);
		pos.y -= 10;
	}

	void LevelPickerMenu()
	{
		//todo when we have more levels add them in here
		MainManager.Instance.TransToScreen(Settings.WalkingTestScreenName, Settings.MenuScreenName);
	}
#endregion

}