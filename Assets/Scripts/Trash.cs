using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Trash : MonoBehaviour
{
	[HideInInspector]
	public Rigidbody rigidbody;
	public SpringJoint spring;

	public int value, valueMin = 1, valueMax = 20;

	public Destructible destructibleComponent;
	public GameObject destroyParticles;
	private bool isDestroyed;

	public bool canBlockWalls;

	public bool explosive;
	public float timeTillExplode = 10;
	public float explodeRadius = 7;

	TMPro.TextMeshPro[] countdowns;

	private void OnEnable()
	{
		if (destructibleComponent)
		{
			destructibleComponent.onDestroy += DestroyTrash;
		}
		else
		{
			destructibleComponent = GetComponent<Destructible>();
			destructibleComponent.SetMeshes(GetComponentsInChildren<MeshRenderer>());
			destructibleComponent.onDestroy += DestroyTrash;
		}
	}

	private void OnDisable()
	{
		if (destructibleComponent)
		{
			destructibleComponent.onDestroy -= DestroyTrash;
		}
	}

	public void MoveToLocation(Vector3 position, float intensity)
	{
		Vector3 dir = position - transform.position;

		rigidbody.AddForce(dir * intensity, ForceMode.Impulse);
	}

	public void GetCarried()
	{
		spring = gameObject.AddComponent<SpringJoint>();
		spring.autoConfigureConnectedAnchor = false;
		spring.spring = 1000;
		spring.damper = 0;
		rigidbody.angularDrag = 10;
		rigidbody.drag = 10;
	}

	public void StopGetCarried()
	{
		rigidbody.angularDrag = 0.05f;
		rigidbody.drag = 0.5f;
		Destroy(spring);
	}

	void DestroyTrash()
	{
		if (!isDestroyed)
		{
			isDestroyed = true;
			StartCoroutine(DestroyRoutine());
		}
	}

	IEnumerator DestroyRoutine()
	{
		float time = 0;

		Vector3 scale = transform.localScale;

		while (time < 0.04f)
		{
			yield return new WaitForEndOfFrame();

			time += Time.deltaTime;

			transform.localScale = Vector3.Lerp(transform.localScale, scale * 2,  Time.deltaTime * 10);
		}

		Instantiate(destroyParticles, transform.position, transform.rotation);

		Manager.UpdateCash(value);

		Manager.instance.trashCount++;

		if (explosive)
		{
			Explode();
		}

		Destroy(gameObject);
	}

    void Start()
    {
		destructibleComponent = GetComponent<Destructible>();

		value = Random.Range(valueMin, valueMax);
		rigidbody = GetComponent<Rigidbody>();

		if (explosive)
		{
			countdowns = GetComponentsInChildren<TMPro.TextMeshPro>();
		}
    }

	public void Explode()
	{
		var collision = Physics.OverlapSphere(transform.position, explodeRadius);

		foreach (var item in collision)
		{
			if (item.GetComponent<Destructible>())
			{
				if (item.GetComponent<Trash>()){
					item.GetComponent<Trash>().value = 0;
				}
				item.GetComponent<Destructible>().onDestroy();
			}
		}

	}

	private void Update()
	{
		if (explosive)
		{
			foreach (var item in countdowns)
			{
				item.text = Mathf.CeilToInt(timeTillExplode).ToString();
			}

			timeTillExplode -= Time.deltaTime;

			if (timeTillExplode < -0.5f)
			{
				DestroyTrash();
			}
		}
	}
}
