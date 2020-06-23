using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof (Collider))]
public class LightSensor : MonoBehaviour {

	[Header("Raycast Target")]
	[Tooltip("Target on the body of player for raycasting (Part of the body/for example - chest)")]
	public GameObject RayTarget;	//Target on the body of player for raycasting (Part of the body/for example - chest)

	[Header("Illumination Meter")]
	[Range (0f, 1f)]
	public float LightingTotal;		//Total illumination of the player (Clamped between 0 and 1) / GENERAL VARIABLE OF THIS SCRIPT
	private Light[] sceneLights;	//Array with all scene light sources

	//Light source variables
	private float sourceDistance; 		//Distance between player and light source
	private float sourceRange;			//Range of the light source
	private float sourceIntencity;		//Intencity of the light source
	private float actualRange; 			//Equals sourceRange multiplied by sourceIntencity
	private LightType sourceType;		//Type of the light source
	private float sourceAngle;			//Angle of the light source (for Spot lights)
	private Vector3 sourceForward;		//Facing direction of the light source in the local space
	private LayerMask directionalMask; 	//LayerMask for directional light raycast

	void Start()
	{
		directionalMask = LayerMask.NameToLayer("SpecialRay");
		sceneLights = FindObjectsOfType (typeof(Light)) as Light[];
	}

	void Update()
	{
		ErrorCallbacks ();
		CountLighting ();
	}

	//You can use this method to refresh the list of light sources on the scene.
	//FOR EXAMPLE: If you turn off/on the lights in real time.
	public void UpdateLights()
	{
		sceneLights = FindObjectsOfType (typeof(Light)) as Light[];
		Debug.Log (sceneLights.Length);
	}
		
	//Counts total illumination of the player from all light sources
	void CountLighting()
	{
		LightingTotal = 0f;

		foreach (Light source in sceneLights) 
		{
			sourceDistance = Vector3.Distance (this.transform.position, source.transform.position);
			sourceRange = source.range;
			sourceIntencity = source.intensity;
			actualRange = sourceRange * sourceIntencity;
			sourceType = source.type;

			if (sourceType == LightType.Spot) 
			{
				sourceAngle = source.spotAngle;
				sourceForward = source.transform.forward;
				Vector3 playerDirection = RayTarget.transform.position - source.transform.position;

				if (Vector3.Angle (sourceForward, playerDirection) <= (sourceAngle/2) && sourceDistance <= actualRange) 
				{
					RaycastSimple (source);
				}
			}

			if (sourceType == LightType.Point) 
			{
				if (sourceDistance <= actualRange) 
				{
					RaycastSimple (source);
				}
			}

			if (sourceType == LightType.Directional) 
			{
				RaycastDirectional (source);
			}
				
		}
	}

	//Raycasts from the light source (Spot or Point light only) to the player
	private void RaycastSimple(Light _source)
	{
		RaycastHit hit; 
		Vector3 rayDirection = RayTarget.transform.position - _source.transform.position;
		if (Physics.Raycast (_source.transform.position, rayDirection, out hit, actualRange)) 
		{
			Debug.DrawRay (_source.transform.position, rayDirection);
			if (hit.transform.tag == "LightHit" && _source.enabled == true) 
			{
				LightingTotal += LightingFactor (sourceRange, sourceDistance);
				LightingTotal = Mathf.Clamp (LightingTotal, 0f, 1f);
			}
		}
	}

	//Raycasts from the player in the oposite direction of light source (Directional light only) 
	private void RaycastDirectional(Light _source)
	{
		RaycastHit hit;
		Vector3 sunDirection = _source.transform.forward * -1;
		if (!Physics.Raycast (RayTarget.transform.position, sunDirection, out hit, directionalMask)) 
		{
			Debug.DrawRay (RayTarget.transform.position, sunDirection, Color.red);
			LightingTotal += 1f;
			LightingTotal = Mathf.Clamp (LightingTotal, 0f, 1f);
		}
	}

	//Counts "lighting factor" for spot and point lights
	private float LightingFactor(float _range, float _distance)
	{
		float shading;
		shading = (100-(_distance / (_range/100)))/100;
		return shading;
	}

	//Some error callbacks
	private void ErrorCallbacks()
	{
		if (this.gameObject.layer != directionalMask) {
			Debug.LogError("LightSensor.cs: " + "This object must be on the custom layer named SpecialRay. Please, create a new layer and name it SpecialRay. Then move only current object to this layer. (Without child objects)");
		}

		if (this.gameObject.tag != "LightHit") 
		{
			Debug.LogError("LightSensor.cs: " + "This object must have tag LightHit. Please, create a new tag and name it LightHit. Then set new tag to this object only. (Without child objects)");
		}
	}

}
