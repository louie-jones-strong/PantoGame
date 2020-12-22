using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FadeableSet : MonoBehaviour
{
	[SerializeField] float AttackTime = 0.25f;
	[SerializeField] float DecayTime = 0.25f;
	[SerializeField] float FadeAmount = 0.6f;

	Renderer ObjectRender;
	Material[] MaterialsNormal;
	Material[] MaterialsFade;

	float TimeSinceLastTrigger = float.MaxValue;
	float TimeSinceFirstTrigger = float.MaxValue;

	void Awake()
	{
		ObjectRender = GetComponent<Renderer>();

		MaterialsNormal = ObjectRender.materials;
		MaterialsFade = new Material[MaterialsNormal.Length];

		for (int loop = 0; loop < MaterialsNormal.Length; loop++)
		{
			var newMaterial = new Material(MaterialsNormal[loop]);
			newMaterial = new Material(MaterialsNormal[loop]);

			newMaterial.SetFloat("_Mode", 2);
			newMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			newMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			newMaterial.SetInt("_ZWrite", 0);
			newMaterial.DisableKeyword("_ALPHATEST_ON");
			newMaterial.EnableKeyword("_ALPHABLEND_ON");
			newMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			newMaterial.renderQueue = 3000;

			MaterialsFade[loop] = newMaterial;
		}
	}

	public void TriggerFade()
	{
		if (TimeSinceLastTrigger >= DecayTime)
		{
			TimeSinceFirstTrigger = 0;
		}
		TimeSinceLastTrigger = 0;
	}
	
	void Update()
	{
		TimeSinceFirstTrigger += Time.deltaTime;
		TimeSinceLastTrigger += Time.deltaTime;

		float fadeAmount = 1f;
		if (TimeSinceFirstTrigger <= AttackTime)
		{
			fadeAmount = Mathf.Lerp(1, FadeAmount, TimeSinceFirstTrigger / AttackTime);
		}
		else
		{
			fadeAmount = Mathf.Lerp(FadeAmount, 1, TimeSinceLastTrigger / DecayTime);
		}
		SetFade(fadeAmount);

	}

	void SetFade(float value)
	{
		if (value == 1)
		{
			ObjectRender.materials = MaterialsNormal;
		}
		else
		{
			ObjectRender.materials = MaterialsFade;
			foreach (var material in ObjectRender.materials)
			{
				material.color = new Color(material.color.r, material.color.g, material.color.b, value);
			}
		}
	}
}
