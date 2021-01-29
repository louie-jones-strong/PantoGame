using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	public static List<Interactable> Interactables = new List<Interactable>();

	[SerializeField] protected float TriggerXDistance = 2;
	[SerializeField] protected float TriggerYDistance = 2;
	
	public PlayerAgent CurrentUser { get; private set; }
	InteractableIcon Icon;

	protected virtual void Awake()
	{
		CurrentUser = null;
		Icon = InteractableIcon.Create(this);
	}

	protected virtual void Start()
	{
		Interactables.Add(this);
	}

	protected virtual void Update()
	{
		//check if any players are near enough to use this task
		bool canUse = false;
		foreach (var player in PlayerManger.Players.Values)
		{
			if (player == null)
			{
				continue;
			}
			
			if (CanInteract(player.transform.position))
			{
				canUse = true;
				break;
			}
		}

		if (Icon != null)
		{
			Icon.SetCanUse(canUse);
		}
	}

	protected void OnDestroy()
	{
		Interactables.Remove(this);
	}

	public virtual void SetHighlight(bool value)
	{
		if (Icon != null)
		{
			Icon.SetHighlight(value);
		}
	}

	public virtual bool CanInteract(Vector3 pos)
	{
		var distance = transform.position - pos;
		return Mathf.Abs(distance.x) <= TriggerXDistance &&
				Mathf.Abs(distance.z) <= TriggerYDistance &&
				Mathf.Abs(distance.y) <= 2;
	}

	public virtual void StartInteraction(PlayerAgent playerAgent)
	{
		if (CurrentUser != null)
		{
			Logger.Log($"Cannot start interaction as user({CurrentUser}) already using it");
			return;
		}

		Logger.Log($"StartInteraction user \"{playerAgent}\"");
		CurrentUser = playerAgent;
		if (Icon != null)
		{
			Icon.SetBeingUsed(true);
		}

		AudioManger.PlayEvent("Interact");
	}

	public virtual void EndInteraction()
	{
		if (Icon != null)
		{
			Icon.SetBeingUsed(false);
		}
		Logger.Log($"EndInteraction user \"{CurrentUser}\" -> null");
		CurrentUser = null;
	}

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		var size = new Vector3(TriggerXDistance, 2, TriggerYDistance);
		size *= 2;
		Gizmos.DrawWireCube(transform.position, size);
	}

	protected virtual void OnDrawGizmosSelected()
	{
	}
#endif
}
