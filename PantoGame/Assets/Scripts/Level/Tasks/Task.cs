using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
	[SerializeField] float TriggerDistance = 2;

	void Start()
	{
		Theatre.Instance.Tasks.Add(this);
	}

	public virtual bool CanInteract(Vector3 pos)
	{
		Logger.Log($"Distance: {(transform.position-pos).magnitude}");
		return (transform.position-pos).magnitude <= TriggerDistance;
	}

	public virtual void Interact(int controlIndex)
	{

	}
}
