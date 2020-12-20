using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuButton : Interactable
{
	[SerializeField] bool TriggerNeedsEveryone;
	[SerializeField] float LoadTime = 2f;

	[SerializeField] Transform Root;
	[SerializeField] Transform LoadingBar;
	[SerializeField] TextMesh Label;
	[SerializeField] AnimationCurve LoadBarCurve;

	Action OnClick;
	Menu Menu;
	float LoadAmount;

	public void Setup(Menu menu, string label, bool triggerNeedsEveryone, Vector2 Pos, float xSize, float ySize, Action onClick=null)
	{
		Menu = menu;
		TriggerNeedsEveryone = triggerNeedsEveryone;
		Label.text = label;
		OnClick = onClick;
		LoadAmount = 0;


		TriggerXDistance = xSize / 2;
		TriggerYDistance = ySize / 2;

		Root.localScale = new Vector3(xSize, ySize, 0);
		LoadingBar.localPosition = new Vector3(0, -(ySize-1), 0);

		transform.position = new Vector3(Pos.x, 0, Pos.y);
	}

	protected override void Update()
	{
		if (TriggerNeedsEveryone)
		{
			bool everyoneInRange = true;
			foreach (var player in Menu.Players.Values)
			{
				if (!base.CanInteract(player.transform.position))
				{
					everyoneInRange = false;
				}
			}
			
			if (everyoneInRange && Menu.Players.Count > 0)
			{
				LoadAmount += Time.deltaTime;
				LoadAmount = Mathf.Min(LoadAmount, LoadTime);
			}
			else
			{
				LoadAmount = 0;
			}

			if (LoadAmount >= LoadTime && OnClick != null)
			{
				OnClick();
			}
		}

		float loadRatio = (LoadAmount / LoadTime);
		loadRatio = LoadBarCurve.Evaluate(loadRatio);
		LoadingBar.transform.localScale = new Vector3(TriggerXDistance * 2 * loadRatio, 1, 1);
		base.Update();
	}

	public override bool CanInteract(Vector3 pos)
	{
		return base.CanInteract(pos) && !TriggerNeedsEveryone;
	}

	public void StartInteration(int controlIndex)
	{
		if (OnClick != null)
		{
			OnClick();
		}
	}
}