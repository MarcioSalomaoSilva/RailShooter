using UnityEngine;
using System.Collections;
//
public class Player_LookAt : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
	{
		//
		this.transform.LookAt (UnityEngine.Camera.main.transform.position);
	}
}
