using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableIcon : MonoBehaviour
{
	const string PrefabName = "InteractableIcon";
	[SerializeField] Animator IconAnimator;
	[SerializeField] AnimationCurve ScaleCurve;

	Interactable Target;

	public static InteractableIcon Create(Interactable target)
	{
		if (HudManger.Instance == null)
		{
			return null;
		}

		var prefab = Resources.Load<InteractableIcon>(PrefabName);
		var parent = HudManger.Instance.CanvasRect.transform;
		var icon = Instantiate<InteractableIcon>(prefab, parent);
		icon.Setup(target);

		return icon;
	}

	public void Setup(Interactable target)
	{
		Target = target;
	}

	void Update()
	{
		if (CameraController.Instance?.Camera == null)
		{
			Logger.LogError($"InteractableIcon Camera == null");
			return;
		}
		if (HudManger.Instance?.CanvasRect == null)
		{
			Logger.LogError($"InteractableIcon CanvasRect == null");
			return;
		}
		if (Target == null)
		{
			Logger.LogError($"InteractableIcon Target == null");
			return;
		}
		
		var worldPos = Target.transform.position;
		
		var screenPos = CameraController.Instance.Camera.WorldToViewportPoint(worldPos);

		var canvasRect = HudManger.Instance.CanvasRect;
		var canvasPos = new Vector2(
				((screenPos.x*canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x*0.5f)),
				((screenPos.y*canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y*0.5f))
 			);

		transform.localPosition = canvasPos;

		var distance = CameraController.GetDistanceToPoint(worldPos);
		transform.localScale =Vector3.one * ScaleCurve.Evaluate(distance);
		
	}

	public void SetHighlight(bool value)
	{
		IconAnimator.SetBool("Highlight", value);
	}

	public void SetCanUse(bool value)
	{
		IconAnimator.SetBool("CanUse", value);
	}

	public void SetBeingUsed(bool value)
	{
		IconAnimator.SetBool("BeingUsed", value);
	}
}
