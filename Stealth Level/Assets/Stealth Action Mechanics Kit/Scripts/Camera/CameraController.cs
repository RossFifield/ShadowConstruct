using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;

	private float distance;
	private float currentX;
	private float currentY;
	private float sensivityX;
	private float sensivityY;

	void Start()
	{
		distance = 0.5f;
		sensivityX = 3f;
		sensivityY = 2f;
	}

	void Update()
	{
		currentX += Input.GetAxis ("Mouse X");
		currentY += Input.GetAxis ("Mouse Y");
		currentY = Mathf.Clamp (currentY, -50f, 50f);
	}

	void LateUpdate()
	{
		Vector3 dir = new Vector3 (0, 0, -distance);
		Quaternion rotation = Quaternion.Euler (-currentY * sensivityY, currentX * sensivityX, 0);
		transform.position = target.position + rotation * dir;
		transform.LookAt (target.position);
	}
}
