using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuButton : MenuInteractable
{
	[SerializeField] bool TriggerNeedsEveryone;
	[SerializeField] float LoadTime = 2f;

	[SerializeField] Transform LoadingBar;
	[SerializeField] TextMesh Label;
	[SerializeField] AnimationCurve LoadBarCurve;

	Action OnClick;
	Menu Menu;
	float LoadAmount;

	public void Setup(Menu menu, string label, bool triggerNeedsEveryone, Vector2 pos, float xSize, float ySize, Action onClick=null)
	{
		TriggerNeedsEveryone = triggerNeedsEveryone;
		Label.text = label;
		LoadAmount = 0;
		OnClick = onClick;

		LoadingBar.localPosition = new Vector3(0, -(ySize-1), 0);

		base.Setup(menu, pos, xSize, ySize);
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

	public override void StartInteraction(PlayerAgent playerAgent)
	{
		if (OnClick != null)
		{
			OnClick();
		}
		base.StartInteraction(playerAgent);
	}

	public override bool CanInteract(Vector3 pos)
	{
		if (CurrentUser != null)
		{
			return false;
		}
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		return base.CanInteract(pos);
#else
		return base.CanInteract(pos) && !TriggerNeedsEveryone;
#endif
	}
}