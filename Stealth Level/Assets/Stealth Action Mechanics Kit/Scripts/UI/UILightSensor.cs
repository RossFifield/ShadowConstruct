using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILightSensor : MonoBehaviour {

	[Header("Light Sensor UI")]
	[Tooltip("Slider of the LIGHT SENSOR UI bar")]
	public GameObject LS_slider;
	[Tooltip("Area of the LIGHT SENSOR UI where the slider moves (gradient stripe)")]
	public GameObject LS_area;
	[Tooltip("If true - slider of the light sensor moves smoothly. If False - shows the realtime value")]
	public bool smoothSensor;
	private float smoothLS;
	private float speedMod;

	private GameObject player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("LightHit");
		smoothLS = 1f;
		speedMod = 1f;
	}

	void Update()
	{
		SetLightSensor ();
	}

	void SetLightSensor()
	{
		float areaWidth = LS_area.GetComponent<RectTransform> ().sizeDelta.x;
		float currentLighting = player.GetComponent<LightSensor> ().LightingTotal;

		if (currentLighting < smoothLS) 
		{
			speedMod = Mathf.Abs (smoothLS - currentLighting);
			smoothLS -= speedMod * Time.deltaTime;
		} 

		if (currentLighting > smoothLS) 
		{
			speedMod = Mathf.Abs (smoothLS - currentLighting);
			smoothLS += speedMod * Time.deltaTime;
		}

		if (smoothSensor) 
		{
			LS_slider.GetComponent<RectTransform> ().anchoredPosition = new Vector3 ((areaWidth * Mathf.Clamp (smoothLS, 0f, 1f)), 0f);
		} 
		else 
		{
			LS_slider.GetComponent<RectTransform> ().anchoredPosition = new Vector3 ((areaWidth * Mathf.Clamp (currentLighting, 0f, 1f)), 0f);
		}
	}
}
