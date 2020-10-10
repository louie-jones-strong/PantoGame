using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteToObjectLoader : MonoBehaviour
{
	[SerializeField] bool EmissionOn;
	[SerializeField] bool RotateToCamOn;
	
    void Awake()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = false;

		var prefab = Resources.Load<SpriteToObjectRenderer>("SpriteTo3D");
		var temp = Instantiate<SpriteToObjectRenderer>(prefab, transform);
		temp.SetImage(spriteRenderer, emissionOn:EmissionOn, rotateToCamOn:RotateToCamOn);
    }
}
