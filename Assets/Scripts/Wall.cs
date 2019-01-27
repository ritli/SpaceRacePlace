using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	bool breached;

	public MeshRenderer destructibleMesh;
	Destructible destructibleComponent;
	BoxCollider collider;
	private GameObject stuckTrash;

	public GameObject breachParticles;
	GameObject spawnedBreachParticles;

	public GameObject explosion;

	public bool rightSideOut = true;

	private void OnDrawGizmos()
	{
		Vector3 offset = rightSideOut ? transform.right : -transform.right;

		Gizmos.DrawCube(transform.position + offset * 5, Vector3.one);
	}

	private void OnEnable()
	{
		if (destructibleComponent)
		{
			destructibleComponent.onDestroy += DestroyWall;
		}
		else
		{
			destructibleComponent = GetComponent<Destructible>();
			destructibleComponent.SetMeshes(new MeshRenderer[] {destructibleMesh});

			destructibleComponent.onDestroy += DestroyWall;
		}
	}

	private void OnDisable()
	{
		if (destructibleComponent)
		{
			destructibleComponent.onDestroy -= DestroyWall;
		}
	}

	void Start()
    {
		collider = GetComponent<BoxCollider>();
	}

	void DestroyWall()
	{
		if (!breached)
		{
			spawnedBreachParticles = Instantiate(breachParticles, transform.position + (rightSideOut ? -transform.right : transform.right) * 2, transform.localRotation * Quaternion.Euler(0, 90, 0));

			Instantiate(explosion, destructibleMesh.transform.position, explosion.transform.rotation);
			destructibleMesh.enabled = false;

			if (stuckTrash)
			{
				Destroy(stuckTrash);
				stuckTrash = null;
			}

			destructibleComponent.SetMeshes(new MeshRenderer[] {});

			collider.isTrigger = true;
			breached = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		var collidingTrash = other.GetComponent<Trash>();

		if (collidingTrash.canBlockWalls && !stuckTrash)
		{
			stuckTrash = collidingTrash.gameObject;
			Destroy(collidingTrash.destructibleComponent);
			collidingTrash.rigidbody.isKinematic = true;
			collidingTrash.tag = "Untagged";
			Destroy(collidingTrash.GetComponent<Collider>());
			Destroy(collidingTrash);

			destructibleComponent.SetMeshes(stuckTrash.GetComponentsInChildren<MeshRenderer>());

			destructibleComponent.health = destructibleComponent.maxHealth;

			Destroy(spawnedBreachParticles);

			collider.isTrigger = false;
			breached = false;
		}
	}

	void Update()
    {
        if (stuckTrash)
		{
			stuckTrash.transform.position = Vector3.Lerp(stuckTrash.transform.position, transform.position, Time.deltaTime * 4);
		}
		if (breached)
		{
			Manager.AddOxygen(-Time.deltaTime * 0.15f);
		}
    }
}
