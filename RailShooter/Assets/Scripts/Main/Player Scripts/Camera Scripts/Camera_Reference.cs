using UnityEngine;
using System.Collections;

public class Camera_Reference : MonoBehaviour {
	//
	public static Camera_Reference instance;
	//
	private GameObject cam;
    //
    public GameObject shipReference;
    public GameObject posReference;
    public GameObject midpoint;
    //
    public GameObject cameraNearReference;
    public GameObject cameraNormalReference;
    public GameObject cameraFarReference;
    //
    public GameObject shipNearReference;
    public GameObject shipNormalReference;
    public GameObject shipFarReference;
    //
    public float distance;
    // Use this for initialization
    void Start () {
		//
		instance = this;
		//
		GetVariables ();
	}
	// Update is called once per frame
	void Update () {
        cameraNearReference.transform.position = shipReference.transform.position - shipReference.transform.forward * Player_Manager.instance.cameraDistance * 0.75f * Player_Manager.instance.universalScale;
        cameraNormalReference.transform.position = shipReference.transform.position - shipReference.transform.forward * Player_Manager.instance.cameraDistance * Player_Manager.instance.universalScale;
        cameraFarReference.transform.position = shipReference.transform.position - shipReference.transform.forward * Player_Manager.instance.cameraDistance * 1.25f * Player_Manager.instance.universalScale;
        //
        shipNearReference.transform.position = Camera_Controller.instance.pc.transform.position - Camera_Controller.instance.pc.transform.forward *
            Player_Manager.instance.cameraDistance * 0.75f * Player_Manager.instance.universalScale;
        shipNormalReference.transform.position = Camera_Controller.instance.pc.transform.position - Camera_Controller.instance.pc.transform.forward *
            Player_Manager.instance.cameraDistance * Player_Manager.instance.universalScale;
        shipFarReference.transform.position = Camera_Controller.instance.pc.transform.position - Camera_Controller.instance.pc.transform.forward *
            Player_Manager.instance.cameraDistance * 1.25f * Player_Manager.instance.universalScale;
    }
	//
	public void GetVariables ()	{
        //
        cam = GameObject.Find ("Camera_Helper");
        //
        posReference = new GameObject("position reference");
        posReference.transform.parent = this.transform;
        posReference.transform.localPosition = Vector3.zero;
        //
        midpoint = new GameObject("mid point");
        midpoint.transform.parent = this.transform;
        //
        shipReference = new GameObject("ship point");
        shipReference.transform.parent = this.transform;
        shipReference.transform.localPosition = Vector3.zero;
        //
        shipNearReference = new GameObject("Ship near reference");
        shipNearReference.transform.parent = this.transform;
        shipNearReference.transform.localPosition = Vector3.zero;
        //
        shipNormalReference = new GameObject("Ship normal reference");
        shipNormalReference.transform.parent = this.transform;
        shipNormalReference.transform.localPosition = Vector3.zero;
        //
        shipFarReference = new GameObject("Ship far reference");
        shipFarReference.transform.parent = this.transform;
        shipFarReference.transform.localPosition = Vector3.zero;
        //
        cameraNearReference = new GameObject("Camera_Helper near reference");
        cameraNearReference.transform.parent = this.transform;
        cameraNearReference.transform.position = Vector3.zero;
        //
        cameraNormalReference = new GameObject("Camera_Helper middle reference");
        cameraNormalReference.transform.parent = this.transform;
        cameraNormalReference.transform.position = Vector3.zero;
        //
        cameraFarReference = new GameObject("Camera_Helper far reference");
        cameraFarReference.transform.parent = this.transform;
        cameraFarReference.transform.position = Vector3.zero;
    }
	//
	public void CinematicCamera () {
        //
        this.transform.position = cam.transform.position;
        this.transform.rotation = this.transform.parent.rotation;
    }
}