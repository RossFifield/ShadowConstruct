using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {

	[Header("Is laser active?")]
	public bool laserActive;
	[Tooltip("Object with LineRenderer Component")]
	[Header("Beam Object")]
	public LineRenderer beam;
	[Tooltip("You can choose what alarm lights will be turned on if the length of the beam will change in cause of interruption")]
	[Header("List of the Alarm Lights")]
	public GameObject[] alarms;
	private float beamBasicLength;

	void Start()
	{
		BasicLength ();
	}

	void Update()
	{
		ActiveControl ();
		LengthControl ();
	}

	private void ActiveControl()
	{
		if (laserActive) 
		{
			beam.enabled = true;
		} else 
		{
			beam.enabled = false;
		}
	}

	//Basic length of the laser beam (to check the interruption)
	private void BasicLength()
	{
		RaycastHit basicHit;
		if (Physics.Raycast (beam.transform.position, beam.transform.up, out basicHit)) 
		{
			beamBasicLength = basicHit.distance;
		}

	}

	//Automaticaly adjust length of the beam when it hits collider
	private void LengthControl()
	{
		if (!laserActive) 
		{
			return;
		}

		RaycastHit hit;
		if (Physics.Raycast (beam.transform.position, beam.transform.up, out hit)) 
		{
			if (hit.collider) 
			{
				beam.SetPosition (1, new Vector3 (0f, hit.distance, 0f));
			}

			if (hit.distance != beamBasicLength) 
			{
				BeamInterruption ();
			}
		}
		else 
		{
			beam.SetPosition (1, new Vector3 (0f, 3000f, 0f));
		}
	}

	//What if laser beam was interrupted by something
	private void BeamInterruption()
	{
		foreach (GameObject alarm in alarms) 
		{
			alarm.GetComponent<AlarmLightController> ().ActivateAlarm();
		}
	}
}
