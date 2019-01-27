using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenButton : MonoBehaviour
{
	Destructible destructibleComponent;

	public GameObject buyaAirParticles;

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

		if (Manager.UpdateCash(-10))
		{
			Manager.AddOxygen(10);
			buyaAirParticles.GetComponent<ParticleSystem>().Play();
		}
	}

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
