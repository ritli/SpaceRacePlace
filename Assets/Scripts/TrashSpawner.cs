using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
	public static TrashSpawner instance;

	GameObject[] spawnableTrash;
	TrashHatch[] hatchesInGame;

	int hatchIndex = 0;
	private float time;
	private float spawnRateModifier = 1;

	void Start()
    {
        if (FindObjectsOfType<TrashSpawner>().Length > 1)
		{
			Destroy(gameObject);
		}
		else
		{
			hatchesInGame = FindObjectsOfType<TrashHatch>();
			spawnableTrash = Resources.LoadAll<GameObject>("Trash");
			instance = this;
		}
    }

	public static void SpawnTrash(int count)
	{
		instance.SpawnTrashInternal(count);
	}

	void SpawnTrashInternal(int count)
	{
		StartCoroutine(SpawnTrashRoutine(count, 0.2f));
	}

	public IEnumerator SpawnTrashRoutine(int count, float interval)
	{
		TrashHatch currentHatch = hatchesInGame[hatchIndex];
		currentHatch.OpenHatch();

		yield return new WaitForSeconds(0.25f);

		Vector3 spawnLocation = hatchesInGame[hatchIndex].spawnPoint.position;
		hatchIndex++;
		hatchIndex = hatchIndex % hatchesInGame.Length;

		while (count > 0)
		{
			yield return new WaitForSeconds(interval);

			Instantiate(spawnableTrash[Random.Range(0, spawnableTrash.Length)], spawnLocation, Random.rotation);

			count--;
		}

		yield return new WaitForSeconds(1.5f);

		currentHatch.CloseHatch();

	}

	void Update()
    {
        if (time < 0f)
		{
			SpawnTrash(2);

			time = 5f * spawnRateModifier;
			spawnRateModifier = spawnRateModifier * 0.95f;
		}
		else
		{
			time -= Time.deltaTime;
		}
    }
}
