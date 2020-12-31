using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropHolder : MonoBehaviour
{
	public static List<PropHolder> PropHoldersList {get; private set;} = new List<PropHolder>();

	[SerializeField] Transform PropSlotRoot;
	protected Prop PropSlot;

	protected virtual void Awake()
	{
		PropHoldersList.Add(this);
		Logger.Log($"Added Prop holder: {this} to list len = {PropHoldersList.Count}");
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
	}
	protected virtual void OnTriggerExit(Collider collider)
	{
	}

	protected virtual void OnDestroy()
	{
		PropHoldersList.Remove(this);
		Logger.Log($"Remove Prop holder: {this} from list len = {PropHoldersList.Count}");
	}

	public virtual bool CanHoldProp(Prop prop, float distance=-1)
	{
		if (prop == null)
		{
			return false;
		}
		if (PropSlot != null)
		{
			return false;
		}
		
		if (distance < 0)
		{
			distance = DistanceUtility.Get2d(prop.transform, transform);
		}
		return distance <= prop.PropSize;
	}

	public virtual void AddProp(Prop prop)
	{
		if (CanHoldProp(prop))
		{
			PropSlot = prop;
			PropSlot.transform.parent = PropSlotRoot;
			PropSlot.transform.localPosition = Vector3.zero;
			PropSlot.transform.localEulerAngles = Vector3.zero;
			Logger.Log($"Added Prop: {prop} to holder: {this}");
		}
	}
	
	public virtual void RemoveProp(Prop prop)
	{
		if (prop == PropSlot)
		{
			PropSlot = null;
			Logger.Log($"removed Prop: {prop} from holder: {this}");
		}
		else
		{
			Logger.LogError($"Trying to remove prop: {prop} != PropSlot: {PropSlot}");
		}
	}

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, 1f);
	}
#endif
}