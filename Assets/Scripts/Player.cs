using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class Player : MonoBehaviour
{
	public float speed = 1;
	public float jumpForce = 1;
	private Weapon weapon;
	private Collider capsuleCollider;
	new Camera camera;
	new Rigidbody rigidbody;
	private SpringJoint spring;
	public LayerMask layerMask;
	private Trash carriedObject;
	const float footLength = 0.55f;

	MeshRenderer previewModel;
	private MeshFilter previewModelFilter;

	void Start()
    {
		weapon = GetComponentInChildren<Weapon>();

		capsuleCollider = GetComponent<Collider>();

		camera = Camera.main;
		print(camera.name);
		rigidbody = GetComponent<Rigidbody>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

    void Update()
    {
		InputUpdate();
		CarryUpdate();
    }

	void CarryUpdate()
	{
		if (carriedObject)
		{
			Vector3 origin = transform.position + camera.transform.forward * 4;

			carriedObject.spring.connectedAnchor = origin;

			//carriedObject.rigidbody.rotation = Quaternion.Lerp(carriedObject.rigidbody.rotation, Quaternion.identity, Time.deltaTime * 12f);
		}
	}

	void PickUp(GameObject pickedUpObject)
	{
		const int dragMultilpier = 200;

		if (carriedObject)
		{
			carriedObject.rigidbody.useGravity = true;
			carriedObject.rigidbody.drag /= dragMultilpier;
			carriedObject.StopGetCarried();

			carriedObject = null;
		}

		else {
			if (pickedUpObject && pickedUpObject.CompareTag("Carryable"))
			{
				carriedObject = pickedUpObject.GetComponent<Trash>();

				carriedObject.GetCarried();

				carriedObject.rigidbody.drag *= dragMultilpier;
				carriedObject.rigidbody.useGravity = false;
			}
		}


	}

	void Fire()
	{
		var hitObject = hitScan(100);

		if (hitObject.collider)
		{
			Destroy(hitObject.collider.gameObject);

			weapon.FireParticles(hitObject.point);
		}

		else
		{
			weapon.FireParticles(camera.transform.forward * 100 + transform.position);
		}

	}

	void InputUpdate()
	{
		Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		transform.Rotate(transform.up, mouse.x);
		camera.transform.Rotate(camera.transform.right, -mouse.y, Space.World);

		Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));

		rigidbody.AddForce(movementInput.z * transform.forward * speed, ForceMode.Impulse);
		rigidbody.AddForce(movementInput.x * transform.right * speed, ForceMode.Impulse);

		if (Input.GetButtonDown("Jump"))
		{
			Jump();
		}

		if (Input.GetMouseButtonDown(0)){
			Fire();
		}

		var interactable = hitScan(5).collider;

		if (interactable && interactable.CompareTag("Carryable") && !carriedObject)
		{
			PreviewHandler.PreviewObject(interactable.gameObject);

			if (Input.GetButtonDown("Interact"))
			{
				PickUp(interactable.gameObject);
			}
		}
		else
		{
			if (carriedObject)
			{
				if (Input.GetButtonDown("Interact"))
				{
					PickUp(null);
				}
				else if (Input.GetButtonDown("Fire2"))
				{
					var thrownObject = carriedObject;
					PickUp(null);
					thrownObject.rigidbody.AddForce(camera.transform.forward * 15, ForceMode.Impulse);
				}
			}

			PreviewHandler.StopPreview();
		}

	}

	void Jump()
	{
		if (OnGround)
		{
			rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
	}

	RaycastHit hitScan(float distance)
	{
			RaycastHit hitObject;

			Physics.Raycast(new Ray(transform.position, transform.forward), out hitObject, distance, layerMask);

			return hitObject;
	}

bool OnGround
	{
		get
		{
			Debug.DrawRay(transform.position + capsuleCollider.bounds.extents.y * Vector3.down, Vector3.down * footLength, Color.red);

			return Physics.Raycast(transform.position + capsuleCollider.bounds.extents.y * Vector3.down * 0.9f, Vector3.down, footLength, layerMask);
		}
	}
}


