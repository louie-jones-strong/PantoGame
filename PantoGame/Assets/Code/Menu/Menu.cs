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
		DebugSettings,
		Credits,
		LevelSelect,
		ExitConfirm
	}

	[SerializeField] AnimationCurve FadeDown;
	[SerializeField] AnimationCurve FadeUp;
	[SerializeField] MenuButton MenuButtonPrefab;
	[SerializeField] MenuSlider MenuSliderPrefab;
	List<MenuInteractable> MenuInteractables = new List<MenuInteractable>();
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
		foreach (var item in MenuInteractables)
		{
			item.SetFade(value);
		}
	}


	void AddButton(string label, bool triggerNeedsEveryone, Action onClick, Vector2 pos, float xSize=10, float ySize=4)
	{
		MenuButton button = null;
		foreach (var item in MenuInteractables)
		{
			var tempButton = item as MenuButton;
			if (tempButton == null)
			{
				continue;
			}

			if (!item.gameObject.activeSelf)
			{
				button = tempButton;
				button.gameObject.SetActive(true);
				break;
			}
		}

		if (button == null)
		{
			button = Instantiate<MenuButton>(MenuButtonPrefab, transform);
			MenuInteractables.Add(button);
		}

		button.Setup(this, label, triggerNeedsEveryone, pos, xSize, ySize, onClick);
	}

	void AddSlider(string label, float value, Vector2 pos, float xSize=10, float ySize=4, Action<float> changedAction=null, float min=0, float max=1)
	{
		MenuSlider slider = null;
		foreach (var item in MenuInteractables)
		{
			var temp = item as MenuSlider;
			if (temp == null)
			{
				continue;
			}

			if (!item.gameObject.activeSelf)
			{
				slider = temp;
				slider.gameObject.SetActive(true);
				break;
			}
		}

		if (slider == null)
		{
			slider = Instantiate<MenuSlider>(MenuSliderPrefab, transform);
			MenuInteractables.Add(slider);
		}

		slider.Setup(this, label, value, pos, xSize, ySize, changedAction:changedAction, min:min, max:max);
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
		foreach (var item in MenuInteractables)
		{
			item.gameObject.SetActive(false);
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
					case eMenuState.Credits: 
						CreditsMenu();
						break;
					case eMenuState.DebugSettings: 
						DebugSettingsMenu();
						break;
					case eMenuState.LevelSelect: 
						LevelPickerMenu();
						break;
					case eMenuState.ExitConfirm: 
						ExitConfirm();
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
		pos.y -= 10;
		AddButton("Exit", false, () => {SetTarget(eMenuState.ExitConfirm);}, pos);
	}

	void SettingsMenu()
	{
		var pos = new Vector2(-10, 10);

		AddButton("Back", false, () => SetTarget(eMenuState.Main), pos);
		pos.y -= 10;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
		AddButton("Debug", false, () => SetTarget(eMenuState.DebugSettings), pos);
		pos.y -= 10;
#endif

		AddButton("Credits", false, () => SetTarget(eMenuState.Credits), pos);
		pos.y -= 10;

		pos = new Vector2(10, 10);
		AddSlider("Music Volume", AudioManger.MusicVolume, pos, changedAction:(v) => 
			{
				AudioManger.MusicVolume = v;
			});

		pos.y -= 6;
		AddSlider("SFX Volume", AudioManger.SfxVolume, pos, changedAction:(v) => 
			{
				AudioManger.SfxVolume = v;
				AudioManger.PlayEvent("SfxVolChange");
			});

		pos.y -= 6;
		AddSlider("Ambience Volume", AudioManger.AmbienceVolume, pos, changedAction:(v) => 
			{
				AudioManger.AmbienceVolume = v;
			});

		pos.y -= 6;

	}

	void DebugSettingsMenu()
	{
		var pos = new Vector2(-10, 10);

		AddButton("Back", false, () => SetTarget(eMenuState.Settings), pos);
		pos.y -= 10;

		AddButton("PlaySound", false, () => {}, pos);
		pos.y -= 10;

	}

	void LevelPickerMenu()
	{
		var pos = new Vector2(-10, 10);

		AddButton("Back", false, () => SetTarget(eMenuState.Main), pos);
		pos.y -= 10;

		int index = 0;
		for (int i = 0; i < 2; i++)
		{
			var scriptIndex = i;
			AddButton($"Level: {index + 1}", true, () => 
				{
					MainManager.Instance.LoadLevel(Settings.TheatreScreenName, scriptIndex, Settings.MenuScreenName);
				}, pos);

			pos.y -= 10;
			index += 1;
		}

		pos = new Vector2(10, 0);
		for (int i = 0; i < 1; i++)
		{
			var scriptIndex = i;
			AddButton($"Level: {index + 1}", true, () => 
				{
					MainManager.Instance.LoadLevel(Settings.TheatreFlippedScreenName, scriptIndex, Settings.MenuScreenName);
				}, pos);
			pos.y -= 10;
			index += 1;
		}
	}

	void CreditsMenu()
	{
		var pos = new Vector2(-10, 10);

		AddButton("Back", false, () => SetTarget(eMenuState.Settings), pos);

	}

	void ExitConfirm()
	{
		var pos = new Vector2(-10, 0);
		AddButton("Exit", false, MainManager.Instance.CloseGame, pos);
		pos.x += 20;
		AddButton("Back", false, () => SetTarget(eMenuState.Main), pos);
	}
#endregion

}