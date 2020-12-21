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
		var prefab = Resources.Load<InteractableIcon>(InteractableIcon.PrefabName);
		Icon = Instantiate<InteractableIcon>(prefab, transform);
	}

	protected virtual void Start()
	{
		Interactables.Add(this);
	}

	protected virtual void Update()
	{

	}

	protected void OnDestroy()
	{
		Interactables.Remove(this);
	}

	public virtual void SetHighlight(bool value)
	{
		Icon.SetHighlight(value);
	}

	public virtual bool CanInteract(Vector3 pos)
	{
		var distance = transform.position - pos;
		return Mathf.Abs(distance.x) <= TriggerXDistance &&
				Mathf.Abs(distance.z) <= TriggerYDistance;
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
		Icon.SetBeingUsed(true);
	}

	public virtual void EndInteraction()
	{
		Icon.SetBeingUsed(false);
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
