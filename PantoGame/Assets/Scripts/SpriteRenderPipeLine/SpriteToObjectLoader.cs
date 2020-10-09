using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteToObjectLoader : MonoBehaviour
{
	[SerializeField] bool EmissionOn;
	[SerializeField] bool RotateToCamOn;
	
	SpriteRenderer SpriteRenderer;
    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
		SpriteRenderer.enabled = false;

		var prefab = Resources.Load<SpriteToObjectRenderer>("SpriteTo3D");
		var temp = Instantiate<SpriteToObjectRenderer>(prefab, transform);
		temp.SetImage(SpriteRenderer.sprite, emissionOn:EmissionOn, rotateToCamOn:RotateToCamOn);
    }
}
