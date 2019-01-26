using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashHatch : MonoBehaviour
{
	private Animator animator;
	public Transform spawnPoint;

	void Start()
    {
		animator = GetComponent<Animator>();    
    }

	public void OpenHatch()
	{
		animator.Play("Open");
	}

	public void CloseHatch()
	{
		animator.Play("Close");
	}

}
