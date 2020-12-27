using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Prop : MonoBehaviour, PropInterface
{
	public PropHolderInterface PropHolder {get; set;}

	public void PickUpProp(PropHolderInterface propholder)
	{
		PropHolder.RemoveProp(this);
		PropHolder = propholder;
	}
}