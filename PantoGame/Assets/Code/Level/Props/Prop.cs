using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
	public static List<Prop> PropsList {get; private set;} = new List<Prop>();

	public float PropSize = 2f;
	PropHolder PropHolder;

	protected virtual void Awake()
	{
		PropsList.Add(this);
	}

	protected virtual void OnDestroy()
	{
		PropsList.Remove(this);
	}

	public virtual void PickUpProp(PropHolder propholder)
	{
		if (propholder == null)
		{
			Logger.LogError($"PickUpProp called with propHolder == null");
			return;
		}

		if (PropHolder != null)
		{
			PropHolder.RemoveProp(this);
		}

		PropHolder = propholder;
		PropHolder.AddProp(this);
	}

	public virtual void DropProp()
	{
		if (PropHolder == null)
		{
			Logger.LogError($"DropProp called with PropHolder == null");
			return;
		}

		//find nearest prop holder
		PropHolder nearestPropHolder = null;
		float smallestDistance = float.MaxValue;
		foreach (var holder in PropHolder.PropHoldersList)
		{
			if (holder == PropHolder)
			{
				continue;
			}

			var distance = DistanceUtility.Get2d(transform, holder.transform);
			if (distance <= smallestDistance && 
				holder.CanHoldProp(this, distance:distance))
			{
				smallestDistance = distance;
				nearestPropHolder = holder;
			}
		}

		//remove prop from old prop holder
		PropHolder.RemoveProp(this);

		if (nearestPropHolder == null)
		{
			//set pos to scene prop root
			transform.parent = PlayerManger.MangerInstance.PropsRoot;
			transform.localEulerAngles = Vector3.zero;
			PropHolder = null;
		}
		else
		{
			nearestPropHolder.AddProp(this);
		}
		
	}

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, PropSize);
	}
#endif
}