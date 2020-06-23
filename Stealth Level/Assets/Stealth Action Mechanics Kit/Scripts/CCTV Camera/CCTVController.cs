using UnityEngine;
using System.Collections;

public class CCTVController : MonoBehaviour {

	[Tooltip("CCTV Camera pivot gameobject")]
	public GameObject cameraPivot;
	[Header("Camera Settings")]
	[Tooltip("If true - camera turned on")]
	public bool isEnabled;
	[Tooltip("If true - camera will be turning back and forth between Positive and Negative angles")]
	public bool isDynamic;
	[Tooltip("Maximum negative turn angle from 0 to -90")]
	[Range(0f,-90f)]
	public float negativeAngle;
	[Tooltip("Maximum positive turn angle from 0 to 90")]
	[Range(0f,90f)]
	public float positiveAngle;
	[Tooltip("Camera turn speed")]
	[Range(0f,1f)]
	public float turnSpeed;

	private float turnT;
	private int pongSwitch;

	void Start()
	{
		pongSwitch = 1;
	}

	void Update()
	{
		RotationCycle ();
	}

	private void RotationCycle()
	{
		
		if (isEnabled && isDynamic) 
		{
			PingPong ();
			cameraPivot.transform.localEulerAngles = new Vector3 (0f, 0f, Mathf.Lerp(negativeAngle,positiveAngle,turnT));
		}
	}

	private void PingPong()
	{
		if (turnT >= 1f) 
		{
			pongSwitch = 2;
		} 
		else if (turnT <= 0f) 
		{
			pongSwitch = 1;
		}

		switch (pongSwitch) 
		{
		case 1:
			turnT += 1 * Time.deltaTime * turnSpeed;
			break;
		case 2:
			turnT -= 1 * Time.deltaTime * turnSpeed;
			break;
		}
	}

}
