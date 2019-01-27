using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class Player : MonoBehaviour
{
	public float speed = 1;
	public float jumpForce = 1;
	private Weapon weapon;
	private Vector3 weaponPos;
	private Collider capsuleCollider;
	new Camera camera;
	new Rigidbody rigidbody;
	private SpringJoint spring;
	public LayerMask layerMask;
	private Trash carriedObject;
	const float footLength = 0.55f;

	MeshRenderer previewModel;
	private MeshFilter previewModelFilter;
	private float simGravMultiplier;
	public float simGravForce;

	bool isOnGround;
	private bool chargingWeapon;
	private float chargeTime;
	private float chargeTimeMax = 0.75f;
	private Vector2 shakeVector;

	void Start()
    {
		weapon = GetComponentInChildren<Weapon>();
		weaponPos = weapon.transform.localPosition;

		capsuleCollider = GetComponent<Collider>();

		camera = Camera.main;
		rigidbody = GetComponent<Rigidbody>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

    void Update()
    {
		InputUpdate();
		CarryUpdate();
		SimulateGravityUpdate();
	}

	void InputUpdate()
	{
		Vector2 mouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) + shakeVector;

		shakeVector = Vector2.zero;
		transform.Rotate(transform.up, mouse.x);
		camera.transform.Rotate(camera.transform.right, -mouse.y, Space.World);

		Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		rigidbody.AddForce(movementInput.z * transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
		rigidbody.AddForce(movementInput.x * transform.right * speed* Time.deltaTime, ForceMode.Impulse);

		//JUMP
		if (Input.GetButtonDown("Jump"))
		{
			Jump();
		}

		if (!carriedObject)
		{
			//FIRE
			if (Input.GetMouseButton(0))
			{
				Fire();
			}
			if (Input.GetMouseButtonUp(0))
			{
				StopFire();
			}
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
				else if (Input.GetButtonDown("Fire1"))
				{
					var thrownObject = carriedObject;
					PickUp(null);
					thrownObject.rigidbody.AddForce(camera.transform.forward * 15, ForceMode.Impulse);
				}
			}

			PreviewHandler.StopPreview();
		}
	}

	void ShakeScreen(float amount)
	{
		shakeVector = Random.insideUnitCircle * amount;
	}

	void CarryUpdate()
	{
		if (carriedObject)
		{
			Vector3 origin = transform.position + camera.transform.forward * 4;

			carriedObject.spring.connectedAnchor = origin;

			//carriedObject.rigidbody.rotation = Quaternion.Lerp(carriedObject.rigidbody.rotation, Quaternion.identity, Time.deltaTime * 12f);
			weapon.transform.localPosition = Vector3.Slerp(weapon.transform.localPosition, weaponPos - weapon.transform.up * 2f, Time.deltaTime * 10f);
		}
		else
		{
			weapon.transform.localPosition = Vector3.Slerp(weapon.transform.localPosition, weaponPos, Time.deltaTime * 10f);

		}
	}
	void SimulateGravityUpdate()
	{
		if (OnGround)
		{
			if (!isOnGround)
			{
				OnLand();
			}

			isOnGround = true;
		}
		else
		{
			simGravMultiplier = Mathf.Clamp01(simGravMultiplier + Time.deltaTime);

			isOnGround = false;
		}

		rigidbody.AddForce(Vector3.down * simGravMultiplier * simGravForce, ForceMode.Impulse);

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

		else
		{
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
		float damage = chargeTime - chargeTimeMax;

		if (!chargingWeapon)
		{
			weapon.StartChargeParticles();
			chargingWeapon = true;
		}

		ShakeScreen(chargeTime * Mathf.SmoothStep(1, 2, Mathf.Clamp01(chargeTime - 1)));

		if (chargeTime > chargeTimeMax)
		{
			var hitObject = hitScan(100);

			if (hitObject.collider)
			{
				if (hitObject.collider.GetComponent<Destructible>())
				{
					var destructible = hitObject.collider.GetComponent<Destructible>();

					destructible.Damage(damage);
				}

				weapon.FireParticles(camera.transform.forward * 100 + transform.position);
			}
		}

		chargeTime += Time.deltaTime;
	}

	void StopFire()
	{
		if (chargingWeapon)
		{
			chargeTime = 0;

			chargingWeapon = false;
			weapon.StopChargeParticles();
		}
	}

	void Jump()
	{
		if (OnGround)
		{
			rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}
	}

	void OnLand()
	{
		simGravMultiplier = 0;
	}

	RaycastHit hitScan(float distance)
	{
		RaycastHit hitObject;
		Physics.Raycast(new Ray(transform.position, camera.transform.forward), out hitObject, distance, layerMask);

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


