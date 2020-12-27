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
	}

	protected virtual void OnDestroy()
	{
		PropHoldersList.Remove(this);
	}

	public virtual bool CanHoldProp(Prop prop)
	{
		if (prop == null)
		{
			return false;
		}
		if (PropSlot != null)
		{
			return false;
		}
		
		var distance = (prop.transform.position - transform.position).magnitude;
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