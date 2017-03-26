using UnityEngine;
using System.Collections;

public class Laser_Destroy : MonoBehaviour 
{
	//
	public float lifeTime = 2f;
	// Use this for initialization
	void Awake () 
	{
		Destroy (this.gameObject, lifeTime );
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
	//
	void OnValidate()
	{
		if (lifeTime < 0)
		{
			//
			lifeTime = 0;
		}
	}
}
