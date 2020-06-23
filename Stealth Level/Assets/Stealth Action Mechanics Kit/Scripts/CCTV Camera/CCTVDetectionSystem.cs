using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (CCTVController))]
public class CCTVDetectionSystem : MonoBehaviour {

	[Tooltip("This object sets the local forward dirrection of the detector")]
	public GameObject detectorOrigin;
	[Header("Detection Settings")]
	[Tooltip("Max distance from which player can be detected")]
	public float detectionRange;
	[Tooltip("Max angle of detection (Detector's field of view)")]
	public float detectionAngle;

	[HideInInspector]
	public float detectionLevel; //CURRENT LEVEL OF DETECTION (IF = 1 - PLAYER DETECTED)
	[Tooltip("How fast player can be detected (How fast detection level increases) Lower values - lower speed")]
	[Range(0.1f, 5f)]
	public float detectionSpeed;

	[Tooltip("Tag that helps to find a player on the scene")]
	public string playerTag;
	private GameObject player;

	[Tooltip("You can choose what alarm lights will be turned on if CCTV Camera detects player")]
	[Header("List of the Alarm Lights")]
	public GameObject[] alarms;
	private bool alarmActive;

	void Start(){
		player = GameObject.FindGameObjectWithTag (playerTag);
	}

	void Update()
	{
		Detection ();
		Detected ();
	}

	private void Detection()
	{
		float playerDistance = Vector3.Distance (transform.root.position, player.transform.root.position);
		Vector3 playerDirection = player.transform.position - detectorOrigin.transform.position;
		float playerAngle = Vector3.Angle (detectorOrigin.transform.forward, playerDirection);

		if (playerAngle <= detectionAngle / 2 && playerDistance <= detectionRange) 
		{
			Raycasting (playerDirection);
		} 
		else 
		{
			detectionLevel = 0f;
			detectionLevel = Mathf.Clamp (detectionLevel, 0f, 1f);
		}
	}

	private void Raycasting(Vector3 _playerDir)
	{
		RaycastHit hit;
		if (Physics.Raycast (detectorOrigin.transform.position, _playerDir, out hit, detectionRange)) 
		{
			Debug.DrawRay (detectorOrigin.transform.position, _playerDir, Color.yellow);

			if (hit.transform.tag == "LightHit") 
			{
				detectionLevel += detectionSpeed * Time.deltaTime;
				detectionLevel = Mathf.Clamp (detectionLevel, 0f, 1f);
			} 
			else 
			{
				detectionLevel = 0f;
				detectionLevel = Mathf.Clamp (detectionLevel, 0f, 1f);
			}
		}
	}

	private void Detected()
	{
		if (alarms.Length > 0)
		{
			alarmActive = alarms [0].GetComponent<AlarmLightController> ().alarmActive;
		}

		if (detectionLevel == 1f && gameObject.GetComponent<CCTVController> ().isEnabled) 
		{
			foreach (GameObject alarm in alarms) 
			{
				alarm.GetComponent<AlarmLightController> ().ActivateAlarm();
			}
		}

		if (alarmActive) 
		{
			gameObject.GetComponent<CCTVController> ().isDynamic = false;
		} 
		else 
		{
			gameObject.GetComponent<CCTVController> ().isDynamic = true;
		}
	}

}
