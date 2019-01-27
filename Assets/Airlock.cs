using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airlock : MonoBehaviour
{
	Collider collider;
	Transform target;
	public bool activated;

	List<Rigidbody> affectedRigids;

	Rigidbody playerBody;

	void Start()
    {
		collider = GetComponent<Collider>();
		collider.isTrigger = true;
		target = transform.GetChild(0);

		affectedRigids = new List<Rigidbody>();
	}

	public void Activate()
	{
		activated = true;

		var collidingObjects = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents);

		foreach (var item in collidingObjects)
		{
			if (!item.GetComponent<Player>())
			{
				item.GetComponent<Rigidbody>().useGravity = false;
				affectedRigids.Add(item.GetComponent<Rigidbody>());
			}
			else
			{
				playerBody = item.GetComponent<Rigidbody>();
			}
		}
	}

	public void Deactivate()
	{
		activated = false;

		foreach (var item in affectedRigids)
		{
			if (!item.GetComponent<Player>())
			{
				item.GetComponent<Rigidbody>().useGravity = false;
			}
		}

		playerBody = null;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (activated && other.GetComponent<Rigidbody>() && !other.GetComponent<Player>())
		{
			affectedRigids.Add(other.GetComponent<Rigidbody>());
		}
	}

	void Update()
    {
		foreach (var item in affectedRigids)
		{
			var dir = target.position - item.position;

			if (dir.magnitude > 6)
			{
				item.AddForce(dir.normalized, ForceMode.Impulse);
			}
		}

		if (playerBody)
		{
			var dir = target.position - playerBody.position;

			if (dir.magnitude > 6)
			{
				playerBody.AddForce(dir.normalized, ForceMode.Impulse);
			}
		}
    }
}
