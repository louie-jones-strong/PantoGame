using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class InteractableIcon : MonoBehaviour
{
	const string PrefabName = "InteractableIcon";
	[SerializeField] Animator IconAnimator;
	[SerializeField] AnimationCurve ScaleCurve;

	RectTransform IconRect;

	Interactable Target;
	

	void Awake()
	{
		IconRect = GetComponent<RectTransform>();
	}

	public static InteractableIcon Create(Interactable target)
	{
		if (HudManger.Instance == null)
		{
			Logger.LogError($"HudManger.Instance == null");
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
			gameObject.SetActive(false);
			Destroy(this);
			return;
		}

		IconAnimator.SetBool("Show", Theatre.Instance == null ||
			Theatre.Instance.State == eTheatreState.ShowInProgress);

		UpdateScreenPos();

		//update screen size
		var distance = CameraController.GetDistanceToPoint(Target.transform.position);
		transform.localScale = Vector3.one * ScaleCurve.Evaluate(distance);
	}

	void UpdateScreenPos()
	{
		var worldPos = Target.transform.position;
		
		var normalizedScreenPos = CameraController.Instance.Camera.WorldToViewportPoint(worldPos);

		var clampedNormalizedScreenPos = new Vector2(
			Mathf.Clamp01(normalizedScreenPos.x),
			Mathf.Clamp01(normalizedScreenPos.y));

		var canvasRect = HudManger.Instance.CanvasRect;
		var canvasPos = new Vector2(
			((clampedNormalizedScreenPos.x * canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x*0.5f)),
			((clampedNormalizedScreenPos.y * canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y*0.5f)));

		transform.localPosition = canvasPos;
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
