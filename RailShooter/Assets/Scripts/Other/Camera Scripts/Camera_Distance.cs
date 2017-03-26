using UnityEngine;
using System.Collections;

public class Camera_Distance : MonoBehaviour {

	public static Camera_Distance Instance;

	public float cameraDistance;
	//test
	public GameObject PC; 
	public GameObject lerpReference;

	void Awake ()
	{
		Instance = this;
		PC = GameObject.Find ("Quad");
		lerpReference = GameObject.Find ("Lerp Reference");
		if (lerpReference == null) 
		{
			lerpReference = new GameObject ("Lerp Reference");
			lerpReference.transform.parent = this.transform;
			lerpReference.transform.position = new Vector3 (0,0,100);
		}
	}
	void Start () 
	{
	
	}
	void Update () 
	{
        cameraDistance = Vector3.Distance(this.transform.position, UnityEngine.Camera.main.transform.position);
	}
}
