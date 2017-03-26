using UnityEngine;
using System.Collections;
//
public class Level_Test_Controller : MonoBehaviour {
	//
	public float rotationSmooth = 10;
	private float left;
	private float right;
	// Use this for initialization
	void Awake () 
	{
		//
		CreateObjects ();
		//
		GetSetVariables ();
	}
	// Update is called once per frame
	void Update () 
	{
		//
		GetInput ();
		//
		Rotate ();
	}
	//
	void GetInput ()
	{
		//
		left = Input.GetAxisRaw ("LTrigger");
		right = Input.GetAxisRaw ("RTrigger");
	}
	//
	public void Rotate ()
	{
		//
		this.transform.RotateAround (Vector3.zero, Vector3.forward, left * rotationSmooth);
		this.transform.RotateAround (Vector3.zero, Vector3.forward, -right * rotationSmooth);
	}
	//
	public void GetSetVariables ()
	{
		//
		rotationSmooth *= 10 * Time.deltaTime; 
	}
	//
	public void CreateObjects ()
	{
		//
//		GameObject Spawner = new GameObject ("Spawner");
//		Spawner.transform.parent = this.transform;
//		Spawner.transform.position = new Vector3 (0, 0, 200);
	}
}
