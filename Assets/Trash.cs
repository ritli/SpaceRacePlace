using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
	[HideInInspector]
	public Rigidbody rigidbody;
	public SpringJoint spring;

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

    void Start()
    {
		rigidbody = GetComponent<Rigidbody>();    
    }
}
