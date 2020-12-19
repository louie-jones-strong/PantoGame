using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	public static List<Interactable> Interactables = new List<Interactable>();

	[SerializeField] protected float TriggerXDistance = 2;
	[SerializeField] protected float TriggerYDistance = 2;

	protected void Awake()
	{
	}

	protected void Start()
	{
		Interactables.Add(this);
	}

	protected void OnDestroy()
	{
		Interactables.Remove(this);
	}

	public virtual bool CanInteract(Vector3 pos)
	{
		var distance = transform.position - pos;
		return Mathf.Abs(distance.x) <= TriggerXDistance &&
				Mathf.Abs(distance.z) <= TriggerYDistance;
	}

	public virtual void Interact(int controlIndex)
	{

	}

#if UNITY_EDITOR
	void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
		var size = new Vector3(TriggerXDistance, 2, TriggerYDistance);
		size *= 2;
        Gizmos.DrawWireCube(transform.position, size);
    }
#endif
}
