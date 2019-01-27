using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OxygenButton : MonoBehaviour
{
	Destructible destructibleComponent;
	int oxygenValue;
	float time;
	TextMeshPro text;

	public GameObject buyaAirParticles;

	private void Start()
	{
		text = GetComponentInChildren<TextMeshPro>();
	}

	private void OnEnable()
	{
		if (destructibleComponent)
		{
			destructibleComponent.onDestroy += BuyOxygen;
		}
		else
		{
			destructibleComponent = GetComponent<Destructible>();
			destructibleComponent.SetMeshes(new MeshRenderer[] { });
			destructibleComponent.onDestroy += BuyOxygen;
		}
	}

	void BuyOxygen()
	{
		destructibleComponent.health = destructibleComponent.maxHealth;

		if (Manager.UpdateCash(-(5 + oxygenValue)))
		{
			Manager.AddOxygen(5);
			buyaAirParticles.GetComponent<ParticleSystem>().Play();
		}
	}
	
    void Update()
    {
		time += Time.deltaTime;
		if (time >= 20f)
		{
			oxygenValue += 1;
			int newValue = 5 + oxygenValue;
			text.text = "Buy Air <b>¤" + newValue.ToString();
			time = 0;
		}
    }
}
