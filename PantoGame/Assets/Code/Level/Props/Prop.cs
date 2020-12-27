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
		if (PropHolder != null)
		{
			PropHolder.RemoveProp(this);
			transform.parent = PlayerManger.MangerInstance.PropsRoot;
			transform.localEulerAngles = Vector3.zero;
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