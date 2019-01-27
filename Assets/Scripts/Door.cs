using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	Animator animator;

	public bool startOpen = false;

    void Start()
    {
		animator = GetComponent<Animator>();

		if (startOpen)
		{
			OpenDoor();
		}
    }

	public void OpenDoor()
	{
		animator.Play("Open");
	}

	public void CloseDoor()
	{
		animator.Play("Close");
	}
}
