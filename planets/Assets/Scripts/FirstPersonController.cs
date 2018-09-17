using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour {

	[Header("References")]
	private GameObject[] planets;
	private GravityBody gravityScript;
	private Transform cameraTransform;
	private Transform flagHolder;
	[SerializeField] private UIController myCanvas;

	private Rigidbody rb;
	private Animator myAnimator;

	[Header("Variables")]
	[SerializeField] private float mouseSensitivityX = 3.5f;
	[SerializeField] private float mouseSensitivityY = 3.5f;
	[SerializeField] private float walkSpeed = 4f;
	[SerializeField] private float jumpForce = 220f;
	[SerializeField] private LayerMask groundedMask;
	[SerializeField] private Vector3 moonRespawn;

	private float verticalLookRotation;
	private Vector3 moveAmount;
	private Vector3 smoothMoveVelocity;
	private bool grounded;
	private bool hasFlag;

	void Awake ()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (planets == null)
		{
			planets = GameObject.FindGameObjectsWithTag("Planet");
		}

		gravityScript = GetComponent<GravityBody>();
		cameraTransform = Camera.main.transform;
		flagHolder = transform.Find("FlagHolder");
		rb = GetComponent<Rigidbody>();
		myAnimator = GetComponentInChildren<Animator>();
	}

	void Update ()
	{
		if (GameController.instance.gamePaused)
		{
			return;
		}

		// Caméra
		// Horizontal (on bouge le corps)
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
		// Vertical (on bouge la caméra)
		// On clamp la rotation avant de changer le transform
		verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60f, 60f);
		cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;

		// Déplacement

		float inputX = Input.GetAxisRaw("Horizontal");
		float inputY = Input.GetAxisRaw("Vertical");
		Vector3 moveDirection = new Vector3(inputX, 0f, inputY).normalized;
		Vector3 targetMoveAmount = moveDirection * walkSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.15f);

		// Lâcher de drapeau
		if (hasFlag)
		{
			if (Input.GetButtonDown("Fire1"))
			{
				LeaveFlag();
			}
		}

		// Si le joueur est en l'air, on cherche quelle planète est la plus proche
		if (!grounded)
		{
			gravityScript.ChangePlanetAttractedTo();
		}

		UpdateAnimator();
	}

	void FixedUpdate ()
	{
		if (GameController.instance.gamePaused)
		{
			return;
		}

		// Déplacement
		// MovePosition = world space
		// Il nous faut du local space pour que le personnage se déplace par rapport à son axe propre
		// TransformDirection permet de faire cette transition
		Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
		rb.MovePosition(rb.position + localMove);

		// Saut
		if (Input.GetButtonDown("Jump"))
		{
			if (grounded)
			{
				Vector3 jump = transform.TransformDirection(new Vector3(0f, 1f, 0.15f));
				rb.AddForce(jump * jumpForce);
			}
		}

		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 1.1f, groundedMask))
		{
			grounded = true;
		}
		else
		{
			grounded = false;
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		if (!hasFlag)
		{
			if (collision.gameObject.tag == "Flag")
			{
				GetFlag(collision.gameObject);
			}
		}
	}

	private void GetFlag (GameObject flag)
	{
		hasFlag = true;
		// On rend l'objet kinematic et on désactive sa boite de collision pour qu'il suive bien le joueur
		flag.GetComponent<Rigidbody>().isKinematic = true;
		flag.GetComponent<Collider>().isTrigger = true;
		// On désactive le script de la gravité et on transforme le drapeau en enfant du joueur
		flag.GetComponent<GravityBody>().enabled = false;
		flag.transform.parent = flagHolder;
		// Puis on met le bon transform
		flag.transform.localPosition = Vector3.zero;
	}

	private void LeaveFlag ()
	{
		GameObject flag = flagHolder.Find("Flag").gameObject;
		flag.GetComponent<GravityBody>().enabled = true;
		flag.GetComponent<Rigidbody>().isKinematic = false;
		flag.GetComponent<Collider>().isTrigger = false;

		flag.GetComponent<GravityBody>().ChangePlanetAttractedTo();

		flag.transform.parent = null;
		flag.transform.position = transform.position + transform.forward*3;

		hasFlag = false;
	}

	private void UpdateAnimator()
	{
		myAnimator.SetBool("grounded", grounded);
		myAnimator.SetFloat("magnitude", rb.velocity.magnitude);

		if (myCanvas != null)
		{
			if (myAnimator.GetBool("grounded"))
			{
				// GroundState
				myCanvas.ChangePlayerStateImage("ground");
			}
			else
			{
				// JumpState
				myCanvas.ChangePlayerStateImage("jump");
			}
		}
	}

	public void MoonRespawn ()
	{
		transform.position = moonRespawn;
		transform.rotation = Quaternion.identity;
		rb.velocity = Vector3.zero;
	}
}
