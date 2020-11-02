using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteToObjectLoader : MonoBehaviour
{
	[SerializeField] bool EmissionOn;
	[SerializeField] bool RotateToCamOn;
	public SpriteToObjectRenderer Renderer {get; private set;}
	
    void Awake()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = false;

		var prefab = Resources.Load<SpriteToObjectRenderer>("SpriteTo3D");
		Renderer = Instantiate<SpriteToObjectRenderer>(prefab, transform);
		Renderer.Setup(spriteRenderer, emissionOn:EmissionOn, rotateToCamOn:RotateToCamOn);
    }
}
