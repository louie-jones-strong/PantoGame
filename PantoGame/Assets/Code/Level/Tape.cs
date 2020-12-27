using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tape : MonoBehaviour
{
	public Color TapeColour;
	[SerializeField] SpriteRenderer Sprite;


#if UNITY_EDITOR

	void OnDrawGizmos()
	{
		Sprite.color = TapeColour;
	}
	
#endif
}
