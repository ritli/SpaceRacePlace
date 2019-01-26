using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	private LineRenderer lineRenderer;
	ParticleSystem[] particles;

	
	public Transform muzzle;

    void Start()
    {
		lineRenderer = GetComponentInChildren<LineRenderer>();
		particles = GetComponentsInChildren<ParticleSystem>();
    }

	public void FireParticles(Vector3 hitLocation)
	{
		foreach (var item in particles)
		{
			item.Play();
		}

		lineRenderer.SetPosition(0, muzzle.transform.position);
		lineRenderer.SetPosition(1, hitLocation);

		lineRenderer.startColor = Color.red;
		lineRenderer.endColor = Color.red;
	}

	void Update()
    {
		Color c1 = lineRenderer.startColor;
		Color c2 = lineRenderer.endColor;

		c1.a -= Time.deltaTime * 2;
		c2.a -= Time.deltaTime * 2;

		lineRenderer.startColor = c1;
		lineRenderer.endColor = c2;
	}
}
