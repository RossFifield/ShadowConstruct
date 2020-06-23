using UnityEngine;
using System.Collections;

public class AlarmLightController : MonoBehaviour {

	public GameObject rotator;
	[Header("Rotation speed of the lamp")]
	[Range(0f,25f)]
	public float rotationSpeed;
	[Header("Is alaram active?")]
	public bool alarmActive;
	[Header("Cool down")]
	[Tooltip("If true - Alarm will be automaticaly turned off in specified period of time")]
	public bool useCooldown;
	[Tooltip("Period of time (In seconds)")]
	public float cooldownTime;
	private float currentTimer;

	void Update()
	{
		AlarmControl ();
		CooldownTimer ();
	}

	//Use this method to activate alarm from other scripts
	public void ActivateAlarm()
	{
		if (currentTimer <= 0) 
		{
			alarmActive = true;
			currentTimer = cooldownTime;
		}
	}

	//Controls the rotation and activates lights
	void AlarmControl()
	{
		if (alarmActive) {
			Light[] lights = rotator.GetComponentsInChildren<Light> ();
			foreach (Light source in lights) {
				source.enabled = true;
			}
			rotator.transform.Rotate (0f, 0f, rotationSpeed);
		} else {
			Light[] lights = rotator.GetComponentsInChildren<Light> ();
			foreach (Light source in lights) {
				source.enabled = false;
			}
		}
	}

	//Controls cool down timer
	private void CooldownTimer()
	{
		if (currentTimer > 0) 
		{
			currentTimer -= Time.deltaTime;
		} 
		else 
		{
			alarmActive = false;
		}
	}



}
