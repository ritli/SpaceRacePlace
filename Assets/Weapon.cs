using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	private LineRenderer lineRenderer;
	ParticleSystem[] shootParticles, chargeParticles;
	
	public Transform muzzle;

	public Transform chargeParent;
	public Transform shootParent;

    void Start()
    {
		lineRenderer = GetComponentInChildren<LineRenderer>();
		shootParticles = shootParent.GetComponentsInChildren<ParticleSystem>();
		chargeParticles = chargeParent.GetComponentsInChildren<ParticleSystem>();
	}

	public void FireParticles(Vector3 hitLocation)
	{
		foreach (var item in shootParticles)
		{
			item.Play();
		}

		lineRenderer.SetPosition(0, muzzle.transform.position);
		lineRenderer.SetPosition(1, hitLocation);

		lineRenderer.startColor = Color.red;
		lineRenderer.endColor = Color.red;
	}

	public void StartChargeParticles()
	{
		foreach (var item in chargeParticles)
		{
			item.Play();
		}
	}

	public void StopChargeParticles()
	{
		foreach (var item in chargeParticles)
		{
			item.Stop();
		}
	}

	void Update()
    {
		if (chargeParticles[0].isPlaying)
		{
			var system = chargeParticles[0].main;
			system.simulationSpeed = Mathf.Clamp(system.simulationSpeed + Time.deltaTime * 6f, 0 , 10);
		}
		else
		{
			var system = chargeParticles[0].main;
			system.simulationSpeed = 1; 
		}

		Color c1 = lineRenderer.startColor;
		Color c2 = lineRenderer.endColor;

		c1.a -= Time.deltaTime * 2;
		c2.a -= Time.deltaTime * 2;

		lineRenderer.startColor = c1;
		lineRenderer.endColor = c2;
	}
}
