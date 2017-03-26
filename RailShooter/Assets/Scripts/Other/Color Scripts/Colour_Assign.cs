using UnityEngine;
using System.Collections;

public class Colour_Assign : MonoBehaviour {
	//
	public static Colour_Assign Instance;
	//
	public GameObject player;
	//
	public Color colorCurrent;
	private Color colorDebug;
	private Color colorNear;
	private Color colorFar;
	private Color colorRandom;
	//private Color[] colors = new Color[2];
	//affects the distance to white
	private float cameraDistanceMultiplier = 100f;
	private float transparencyDistanceMultiplier = 10f;
	//
	//private float cameraMaxDistance;
	//private float cameraCurrentDistance;
	private float pcCurrentDistance;
	//
	private bool colorChange;
	private bool transparencyChange;
	private bool visibilityChange;

	void Awake () 
	{
		//
		Instance = this;
		//assigns a the debug color to the material to easily identify stuff in case something goes wrong
		GetComponent<Renderer> ().material.color = colorDebug;
		//assign a color to each color value back public to change in inspector
		colorDebug = Color.red;
		colorNear = Color.black;
		colorFar = Color.white;
		//for transparency divide by large number for different transparencys or different colours. Use only two blocks for black and white blocks
//		colors[0] = colorNear;
//		colors[1] = colorNear/10f;
//		colors[2] = colorNear/9f;
//		colors[3] = colorNear/8f;
//		colors[4] = colorNear/7f;
//		colors[5] = colorNear/6f;
//		colors[6] = colorNear/5f;
//		colors[7] = colorNear/4f;
//		colors[8] = colorNear/3f;
//		colors[9] = colorNear/2f;
//		colors[1] = colorFar;
//		colorRandom = colors[Random.Range(0, colors.Length)];
		//gives ieach block a random grayscale
		var randomValue = Random.Range(0.0f,1f);
		colorRandom = new Color (randomValue, randomValue, randomValue);

	}
	void Start () 
	{
		
	}
	void Update () 
	{
		//runs methods that check distances and visibilty
		determineCameraDistance ();
		determinePosition ();
		//assigns the current color to the variable so it can be seen in the inspector
		colorCurrent = GetComponent<Renderer> ().material.color;
		//different lerps to test with
		Color transparency = new Color (0,0,0,0.0f);
		Color pcDistanceLerp = Color.Lerp(colorNear, colorFar, pcCurrentDistance/cameraDistanceMultiplier);
		//Color cameraDistanceLerp = Color.Lerp(colorFar, colorNear, cameraCurrentDistance/cameraDistanceMultiplier);
		Color transparencyDistanceLerp = Color.Lerp (transparency,colorNear, 1/transparencyDistanceMultiplier);
		//Color.Lerp (colorNear, colorFar, pcCurrentDistance * cameraCurrentDistance / cameraDistanceMultiplier);
		//checks if visible 
		if (visibilityChange) 
		{
			GetComponent<Renderer> ().enabled = false;
		} 
		else
		{
			GetComponent<Renderer> ().enabled = true;
		}
		//assigns the correct stuff to each block check determine methods for what stuff its checking
		if (colorChange) 
		{
			GetComponent<Renderer> ().material.color = colorDebug;
		} 
		else if (transparencyChange)
		{
			GetComponent<Renderer> ().material.color = transparencyDistanceLerp;
		} 
		else 
		{
			GetComponent<Renderer> ().material.color = pcDistanceLerp * colorRandom;
			GetComponent<Renderer> ().enabled = true;
		}
	}
	void determineCameraDistance()
	{
		//
		//cameraCurrentDistance = Vector3.Distance (Camera_Helper.main.transform.position, this.transform.position);
		//pcCurrentDistance = Vector3.Distance (player.transform.position, this.transform.position);
		//because of player reference error
		pcCurrentDistance = Vector3.Distance (this.transform.position, this.transform.position);
		//cameraCurrentDistance = Vector3.Distance (Camera_Distance.Instance.lerpReference.transform.position,this.transform.position);
		//
		//cameraMaxDistance = Camera_Distance.Instance.cameraDistance * cameraDistanceMultiplier;
	}
	void determinePosition ()
	{
		//turn the blocks into black
		if (this.transform.position.z > Camera_Distance.Instance.PC.transform.position.z + 0.7f || this.transform.position.z < Camera_Distance.Instance.PC.transform.position.z - 0.7f) {
			colorChange = false;
		} else {
			colorChange = true;
		}
		//turn the blocks far transparent
		if (this.transform.position.z < Camera_Distance.Instance.PC.transform.position.z - 0.5f) {
			transparencyChange = true;
		} else 
		{
			transparencyChange = false;
		}
		//
		if (this.transform.position.z < Camera_Distance.Instance.PC.transform.position.z - 2.5f) {
			visibilityChange = true;
		} else 
		{
			visibilityChange = false;
		}
	}
}
