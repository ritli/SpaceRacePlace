using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void DestructibleDestroy();

public class Destructible : MonoBehaviour
{
	public float health, maxHealth = 1;

	public DestructibleDestroy onDestroy;

	MeshRenderer[] meshes;

	Material heatMat;

	public void Damage(float amount)
	{
		health -= amount;

		for (int i = 0; i < meshes.Length; i++)
		{
			meshes[i].material = heatMat;

			meshes[i].material.color = Color.Lerp(Color.red, Color.white, health / maxHealth);
		}

		if (health < 0)
		{
			onDestroy?.Invoke();
		}
	}

	void Start()
    {
		meshes = GetComponentsInChildren<MeshRenderer>();
		heatMat = Resources.Load<Material>("HeatMat");

		health = maxHealth;
	}
}
