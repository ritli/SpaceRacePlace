using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirlockButton : MonoBehaviour
{
	public Door airlockDoor, airlockEntryDoor;
	public Airlock airlock;

	Destructible destructibleComponent;

	public float time = 8;

	private void OnEnable()
	{
		if (destructibleComponent)
		{
			destructibleComponent.onDestroy += Activate;
		}
		else
		{
			destructibleComponent = GetComponent<Destructible>();
			destructibleComponent.SetMeshes(new MeshRenderer[] { });
			destructibleComponent.onDestroy += Activate;
		}
	}

	private void OnDisable()
	{
		if (destructibleComponent)
		{
			destructibleComponent.onDestroy -= Activate;
		}
	}

	void Activate()
	{
		if (!airlock.activated)
		{
			destructibleComponent.health = destructibleComponent.maxHealth;

			StartCoroutine(ActivateRoutine());
		}
	}

	IEnumerator ActivateRoutine()
	{
		airlockEntryDoor.CloseDoor();

		yield return new WaitForSeconds(1f);

		airlockDoor.OpenDoor();
		airlock.Activate();

		yield return new WaitForSeconds(time);

		airlockDoor.OpenDoor();
		airlock.Deactivate();

		yield return new WaitForSeconds(1f);

		airlockEntryDoor.OpenDoor();
	}

}
