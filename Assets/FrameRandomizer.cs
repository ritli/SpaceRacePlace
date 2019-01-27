using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRandomizer : MonoBehaviour
{
	public Material[] mats;

    void Start()
    {
		if (Random.value > 0.25f)
		{
			Destroy(gameObject);
		}

		GetComponent<MeshRenderer>().material = mats[Random.Range(0, mats.Length)];

		transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-10f, 10f));
    }
}
