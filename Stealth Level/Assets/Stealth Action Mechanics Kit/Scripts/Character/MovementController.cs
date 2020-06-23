using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	Animator theAnimator;
	private float moveInput;
	public GameObject characterCamera;

	public float degreesPerSecond;
	Quaternion targetRotation;

	float inputX;
	float inputY;

	void Start()
	{
		theAnimator = GetComponent<Animator> ();
		GetComponent<Rigidbody> ().freezeRotation = true;
		degreesPerSecond = 180f;
	}

	void Update()
	{
		AnimationControl ();
		CharacterRotator ();
	}

	void CharacterRotator()
	{
		inputX = Input.GetAxis ("Horizontal");
		inputY = Input.GetAxis ("Vertical");

		if(inputY > 0 && inputX == 0){//Forward
			LookAtDirection (characterCamera.transform.forward);
		}
		else if(inputY < 0 && inputX == 0){//Backward
			LookAtDirection (-characterCamera.transform.forward);
		}
		else if(inputY == 0 && inputX < 0){//Left
			LookAtDirection (-characterCamera.transform.right);
		}
		else if(inputY == 0 && inputX > 0){//Right
			LookAtDirection (characterCamera.transform.right);
		}
		else if(inputY > 0 && inputX > 0){//ForwardRight
			LookAtDirection ((characterCamera.transform.forward + characterCamera.transform.right).normalized);
		}
		else if(inputY > 0 && inputX < 0){//ForwardLeft
			LookAtDirection ((characterCamera.transform.forward + (-characterCamera.transform.right)).normalized);
		}
		else if(inputY < 0 && inputX < 0){//BackwardRight
			LookAtDirection ((-characterCamera.transform.forward + (-characterCamera.transform.right)).normalized);
		}
		else if(inputY < 0 && inputX > 0){//BackwardLeft
			LookAtDirection ((-characterCamera.transform.forward + characterCamera.transform.right).normalized);
		}
	}

	void LookAtDirection(Vector3 moveDirection)
	{
		Vector3 xzDirection = new Vector3 (moveDirection.x, 0, moveDirection.z);
		if (xzDirection.magnitude > 0) {
			targetRotation = Quaternion.LookRotation (xzDirection);
		}

		transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRotation, degreesPerSecond * Time.deltaTime);
	}

	void AnimationControl()
	{
		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
			moveInput += 6 * Time.deltaTime;
			moveInput = Mathf.Clamp (moveInput, 0, 1);

			if (Input.GetKey (KeyCode.LeftShift)) {
				theAnimator.speed = 1.2f;
			} else {
				theAnimator.speed = 1f;
			}
		} else {
			moveInput -= 10 * Time.deltaTime;
			moveInput = Mathf.Clamp (moveInput, 0, 1);
		}
			
		theAnimator.SetFloat ("LocoBlend", moveInput);
	}
}
