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

	private void OnEnable()
	{
		if (destructibleComponent)
		{
			destructibleComponent.onDestroy += DestroyTrash;
		}
		else
		{
			destructibleComponent = GetComponent<Destructible>();
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

		Destroy(gameObject);
	}

    void Start()
    {
		destructibleComponent = GetComponent<Destructible>();

		value = Random.Range(valueMin, valueMax);
		rigidbody = GetComponent<Rigidbody>();
    }
}
